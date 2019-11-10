using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerWallHug : State
{
    public void Update(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        float input = Mathf.Round(Input.GetAxis("Horizontal"));
        if(input == 1 && !player.IsHuggingRight || input == -1 && player.IsHuggingRight)
        {
            player.WalljumpHoldCounter += Time.deltaTime;
        } else
        {
            player.WalljumpHoldCounter = 0;
        }

        if (Input.GetKeyDown("space") && !player.IsGrounded)
        {
            player.body.TargetMovement.y = player.WalljumpVerticalPower;
            player.body.Movement.y = player.WalljumpVerticalPower;
            player.body.Movement.x = player.IsHuggingRight ? -player.WalljumpHorizontalPower : player.WalljumpHorizontalPower;
            player.IsJumping = true;
            player.IsWallJumping = true;
            player.WalljumpHoldCounter = player.WalljumpHoldTime;
            player.state = player.states[0];
        }
        if (!Input.GetKey("space") && player.IsJumping)
        {
            if (player.IsJumping)
            {
                player.body.TargetMovement.y *= 0.6f;
                player.body.Movement.y *= 0.6f;
                player.IsJumping = false;
            }
        }

        player.OrbBehaviour();
    }

    public void FixedUpdate(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        player.body.Movement.y -= player.GravityPower * Time.deltaTime * (player.body.Movement.y < 0 ? 0.5f : 1f);
        player.body.TargetMovement.y = player.body.Movement.y;
        player.body.Move(player.body.Movement * Time.deltaTime);
        float walldist = 0.05f;
        Vector2 wallcheck = new Vector2(walldist, 0);
        int n = player.body.detection.Cast(player.IsHuggingRight ? wallcheck : -wallcheck);
        if (n == 0 || player.IsGrounded || player.WalljumpHoldCounter > player.WalljumpUnHugTime)
        {
            player.state = player.states[0];
            player.WalljumpHoldCounter = 0;
        }
        player.IsGrounded = player.body.detection.collisions.below ? true : false;

        player.CreateOrb();
    }
}
