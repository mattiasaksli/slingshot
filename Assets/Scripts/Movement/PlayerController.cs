using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 4;
    public float AccelerationGround = 40;
    public float AccelerationAir = 20;

    public float JumpPower = 10;
    public float ThrowPower = 20;
    public float GravityPower = 40;
    public KinematicBody OrbPrefab;

    public int FramesToBlockInput = 0;

    public State state;
    public List<State> states;
    public KinematicBody body { get; private set; }

    public bool IsGrounded = false;
    public bool IsJumping = false;
    public bool IsSlingshotting = false;


    private bool createOrb = false;
    private KinematicBody orb = null;

    // Start is called before the first frame update
    void Start()
    {
        states = new List<State>() { new StatePlayerMove()};
        state = states[0];
        body = gameObject.GetComponent<KinematicBody>();
    }


    // Update is called once per frame
    void Update()
    {
        state.Update(this);
        /*if (CD.isWalljumping)
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
        }*/
    }

    private void FixedUpdate()
    {
        state.FixedUpdate(this);
        /*body.Move(body.Movement*Time.deltaTime);

        IsGrounded = body.detection.collisionDirections[0] ? true : false;

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
        }*/
    }

    public void CreateOrb()
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
    public void RecallOrb()
    {
        GameObject.Destroy(orb.gameObject);
        orb = null;
    }

    public void OrbBehaviour()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (orb == null)
            {
                createOrb = true;
            }
            else
            {
                IsSlingshotting = true;
                body.Movement = new Vector2(orb.transform.position.x - transform.position.x, orb.transform.position.y - transform.position.y);
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
            var collisions = body.detection.collisionDirections;
            if ( (collisions[0] && body.Movement.y <= 0) || (collisions[1] && body.Movement.x >= 0) || (collisions[2] && body.Movement.y >= 0) || (collisions[3] && body.Movement.x <= 0))
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
