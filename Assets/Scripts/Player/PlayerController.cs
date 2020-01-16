using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    [Header("Player base attributes")]
    [Space]
    public float MovementSpeed = 4;
    public float JumpPower = 10;
    public float AccelerationGround = 40;
    public float AccelerationAir = 20;
    public float GravityPower = 40;
    public float DeathTime;
    public State state;
    public List<State> states;

    [Space(10)]
    [Header("Player walljump attributes")]
    public float WalljumpHorizontalPower = 8;
    public float WalljumpVerticalPower = 5;
    public float WalljumpHoldTime = 0.5f;
    public float WalljumpHoldCounter = 0;
    public float WalljumpMinHoldTime = 0.01f;
    public float WalljumpUnHugTime = 0.1f;
    [HideInInspector]
    public float JumpPadTimestamp;

    [Space(10)]
    [Header("Slingshot attributes")]
    public OrbBody OrbPrefab;
    public float ThrowPower = 20;
    public float SlingShotStartSpeed = 10;
    public float SlingShotAcceleration = 60;
    public float SlingShotMaxSpeed = 50;

    [Space(10)]
    [Header("Player current status")]
    public bool IsGrounded = false;
    public bool IsJumping = false;
    public bool IsSlingshotting = false;
    public bool IsHuggingRight = false;
    public bool IsWallJumping = false;
    public bool IsFacingRight = true;
    public bool IsOrbAvailable = true;
    public bool IsInputLocked = true;
    [SerializeField]
    private bool createOrb = false;

    [HideInInspector]
    public PlayerBody body { get; private set; }
    [HideInInspector]
    public OrbBody orb = null;
    [HideInInspector]
    public SpriteRenderer Sprite;
    [HideInInspector]
    private float DeathCooldown;

    [Space(10)]
    [Header("Player audio SFX")]
    public AudioClipGroup AudioSlingShot;
    public AudioClipGroup AudioLand;
    public AudioClipGroup AudioJump;
    public AudioClipGroup AudioDefeat;
    public AudioClipGroup AudioThrow;

    private Animator animator;
    public DefeatFade fade;
    public ParticleSystem DefeatParticle;

    void Start()
    {
        states = new List<State>() { new StatePlayerMove(), new StatePlayerSlingshot(), new StatePlayerWallHug(), new StatePlayerDead(), new StatePlayerVictory() };
        state = states[0];
        body = gameObject.GetComponent<PlayerBody>();
        Sprite = GetComponentInChildren<SpriteRenderer>();
        DeathCooldown = Time.time;
        animator = GetComponentInChildren<Animator>();
    }

    public void Respawn()
    {
        transform.position = LevelController.Instance.SpawnPoint.transform.position;
        Physics.SyncTransforms();
        Sprite.enabled = true;
        Sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
        body.Movement = new Vector2(0, 0);
        body.TargetMovement = new Vector2(0, 0);
        body.detection.InsideCollisions = new List<Transform>();
        state = states[0];
        RecallOrb();
        IsOrbAvailable = true;
        DeathCooldown = Time.time + 0.1f;
        Sprite.color = Color.white;
        Physics2D.SyncTransforms();
    }

    public void Animations()
    {
        Vector2 absMovement = new Vector2(Mathf.Abs(body.Movement.x), Mathf.Abs(body.Movement.y));
        var isMoving = absMovement[0] > 0.1f;
        animator.SetBool("IsGrounded", IsGrounded);
        animator.SetBool("IsDefeated", state == states[3]);
        animator.SetBool("IsVictorious", state == states[4]);
        animator.SetBool("IsFalling", body.Movement.y < 0);
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsWallHugging", state == states[2]);
        animator.SetBool("IsFlinging", absMovement[0] > 15 && absMovement[0] > absMovement[1] * 2);
        animator.SetBool("IsTurning", (Mathf.Sign(body.Movement.x) != Mathf.Sign(Input.GetAxisRaw("Horizontal")) && Input.GetAxisRaw("Horizontal") != 0));
    }

    void Update()
    {
        if (!IsInputLocked)
        {
            state.Update(this);
        }
        if (!IsFacingRight) Sprite.flipX = true;
        else Sprite.flipX = false;
        Animations();
    }

    private void FixedUpdate()
    {
        if (body.TargetMovement.x != 0)
        {
            IsFacingRight = body.TargetMovement.x > 0;
        }
        if (!IsInputLocked)
        {
            state.FixedUpdate(this);
        }
        if (body.detection.InsideCollisions.Count > 0 && state != states[3] && Time.time > DeathCooldown)
        {
            foreach (Transform t in body.detection.InsideCollisions)
            {
                if (t != null && t.GetComponent<OneWayPlatform>() == null)
                {
                    Defeat();
                    break;
                }
            }
            body.detection.InsideCollisions = new List<Transform>();
        }
    }

    public void CreateOrb()
    {
        if (createOrb)
        {
            if (orb != null)
            {
                GameObject.Destroy(orb.gameObject);
            }
            orb = GameObject.Instantiate<OrbBody>(OrbPrefab);
            orb.transform.position = transform.position;
            orb.Movement = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized * ThrowPower;
            createOrb = false;
        }
    }
    public void RecallOrb()
    {
        if (orb)
        {
            GameObject.Destroy(orb.gameObject);
            IsOrbAvailable = false;
            orb = null;
        }
    }

    public void Slingshot()
    {
        state = states[1];
        body.detection.collisions.Reset();
        var dir = new Vector2(orb.transform.position.x - transform.position.x, orb.transform.position.y - transform.position.y).normalized;
        body.Movement = SlingShotStartSpeed * dir;
        var st = ((StatePlayerSlingshot)states[1]);
        st.initialDirection = dir;
        st.stateStartTime = Time.time;
    }

    public void OrbBehaviour()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (orb != null)
            {
                RecallOrb();
                IsOrbAvailable = true;
                AudioThrow?.Play();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (orb == null)
            {
                if (IsOrbAvailable)
                {
                    createOrb = true;
                    AudioThrow?.Play();
                }
            }
            else
            {
                Slingshot();
            }
            if (IsSlingshotting)
            {
                var collisions = body.detection.collisions;
                if (collisions.above || collisions.below || collisions.right || collisions.left)
                {
                    IsSlingshotting = false;
                }
                body.Movement = new Vector2(orb.transform.position.x - transform.position.x, orb.transform.position.y - transform.position.y).normalized * 15;
                body.TargetMovement = body.Movement;
                if ((orb.transform.position - transform.position).magnitude < 0.4)
                {
                    IsSlingshotting = false;
                    RecallOrb();
                }
            }
        }
    }

    public void disablePlayer()
    {
        foreach (SpriteRenderer r in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            r.enabled = false;
        }
        Destroy(GameObject.FindGameObjectWithTag("OrbFollower"));
        IsInputLocked = true;
    }

    public void enablePlayer()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
        IsInputLocked = false;
    }

    public void Defeat()
    {
        if (state != states[3] && state != states[4])
        {
            state = states[3];
            DeathTime = Time.time + 0.7f;
            body.Movement.y = 10;
            body.TargetMovement.y = body.Movement.y;
            AudioDefeat?.Play();
        }
    }

    public void LockInput()
    {
        IsInputLocked = true;
    }

    public void UnlockInput()
    {
        IsInputLocked = false;
    }
}
