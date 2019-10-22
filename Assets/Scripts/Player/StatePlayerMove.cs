using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerMove : State
{
    // Start is called before the first frame update
    public void Update(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        player.body.TargetMovement.x = Mathf.Round(Input.GetAxis("Horizontal")) * player.MovementSpeed;
        if (Input.GetKeyDown("space") && player.IsGrounded)
        {
            player.body.TargetMovement.y = player.JumpPower;
            player.body.Movement.y = player.JumpPower;
            player.IsJumping = true;
        }

        if (player.body.Movement.y <= 0)
        {
            player.IsJumping = false;
        }

        player.body.Acceleration = player.AccelerationGround;
        if(Mathf.Round(Input.GetAxis("Horizontal")) == 0)
        {
            player.body.Acceleration = player.IsGrounded ? player.AccelerationGround : player.AccelerationAir;
        }

        if (Input.GetKeyUp("space") && player.IsJumping)
        {
            player.body.TargetMovement.y *= 0.6f;
            player.body.Movement.y *= 0.6f;
            player.IsJumping = false;
        }

        player.OrbBehaviour();
    }

    // Update is called once per frame
    public void FixedUpdate(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        player.body.Movement.y -= player.GravityPower* Time.deltaTime;
        player.body.TargetMovement.y = player.body.Movement.y;
        player.body.Move(player.body.Movement * Time.deltaTime);

        player.IsGrounded = player.body.detection.collisionDirections[0] ? true : false;

        player.CreateOrb();
    }
}
