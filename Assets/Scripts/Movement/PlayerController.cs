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
            createOrb = true;
        }
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
