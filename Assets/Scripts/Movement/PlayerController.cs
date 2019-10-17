using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<KinematicBody>();
    }

    // Update is called once per frame
    void Update()
    {
        body.TargetMovement.x = Mathf.Round(Input.GetAxis("Horizontal")) * MovementSpeed;
        if (Input.GetKey("space") && body.IsGrounded)
        {
            body.TargetMovement.y = JumpPower;
            body.Movement.y = JumpPower;
        }
        if (Input.GetMouseButtonDown(0))
        {
            createOrb = true;
        }
    }

    private void FixedUpdate()
    {
        if(createOrb)
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
