using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 4;
    public float JumpPower = 10;
    public float ThrowPower = 20;
    public KinematicBody OrbPrefab;

    private KinematicBody orb = null;
    private KinematicBody body;
    private bool createOrb = false;
    public bool IsJumping = false;
    private bool IsSlingshotting = false;

    public CollisionDetection CD;
    public int FramesToBlockInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<KinematicBody>();
        CD = gameObject.GetComponent<CollisionDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CD.isWalljumping)
        {
            if (FramesToBlockInput == 5)
            {
                CD.isWalljumping = false;
                FramesToBlockInput = 0;
            }

            if (CD.walljumpingRight)
            {
                body.TargetMovement.x = Mathf.Round(Mathf.Clamp(Input.GetAxis("Horizontal"), 0, MovementSpeed)) * MovementSpeed;
            }
            else
            {
                body.TargetMovement.x = Mathf.Round(Mathf.Clamp(Input.GetAxis("Horizontal"), -MovementSpeed, 0)) * MovementSpeed;
            }
            FramesToBlockInput++;
        }
        else
        {
            body.TargetMovement.x = Mathf.Round(Input.GetAxis("Horizontal")) * MovementSpeed;
            if (Input.GetKeyDown("space") && body.IsGrounded)
            {
                body.TargetMovement.y = JumpPower;
                body.Movement.y = JumpPower;
                IsJumping = true;
            }
            if (body.Movement.y <= 0)
            {
                IsJumping = false;
            }
            if (Input.GetKeyUp("space") && IsJumping)
            {
                body.TargetMovement.y *= 0.6f;
                body.Movement.y *= 0.6f;
                IsJumping = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(orb == null)
            {
                createOrb = true;
            } else
            {
                IsSlingshotting = true;
                body.Movement = new Vector2(orb.transform.position.x - transform.position.x, orb.transform.position.y - transform.position.y).normalized * 15;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (orb != null)
            {
                RecallOrb();
            }
        }

        if (IsSlingshotting)
        {
            if (body.Movement.magnitude < 3)
            {
                IsSlingshotting = false;
            }
            body.Movement = new Vector2(orb.transform.position.x - transform.position.x, orb.transform.position.y - transform.position.y).normalized * 15;
            body.TargetMovement = body.Movement;
            if((orb.transform.position-transform.position).magnitude < 0.4)
            {
                IsSlingshotting = false;
                RecallOrb();
            }
        }
    }

    private void RecallOrb()
    {
        GameObject.Destroy(orb.gameObject);
        orb = null;
    }

    private void FixedUpdate()
    {
        if (createOrb)
        {
            if (orb != null)
            {
                GameObject.Destroy(orb.gameObject);
            }
            orb = GameObject.Instantiate<KinematicBody>(OrbPrefab);
            orb.transform.position = transform.position;
            orb.Movement = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized * ThrowPower;
            createOrb = false;
        }
    }
}
