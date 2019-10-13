using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 4;
    public float JumpPower = 10;
    private KinematicBody body;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<KinematicBody>();
    }

    // Update is called once per frame
    void Update()
    {
        body.TargetMovement.x = Mathf.Round(Input.GetAxis("Horizontal")) * MovementSpeed;
        if (Input.GetKeyDown("space"))
        {
            body.TargetMovement.y = JumpPower;
            body.Movement.y = JumpPower;
        }
    }
}
