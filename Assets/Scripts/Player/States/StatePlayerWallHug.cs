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
            player.body.detection.collisions.Reset();
            player.state = player.states[0];
            return;
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
        //if (player.state == player.states[2])
        //{
        player.body.Movement.y -= player.GravityPower * Time.deltaTime * (player.body.Movement.y < 0 ? 0.5f : 1f);
        player.body.TargetMovement.y = player.body.Movement.y;
        bool aboveC = player.body.detection.collisions.above;
        player.body.Move(player.body.Movement * Time.deltaTime);
        player.IsGrounded = player.body.detection.collisions.below ? true : false;

        if (player.body.detection.collisions.below || player.body.detection.collisions.above)
        {
            player.body.Movement.y = 0;
            player.body.TargetMovement.y = player.body.Movement.y;
        }

        if (player.body.detection.collisions.right || player.body.detection.collisions.left)
        {
            player.body.Movement.x = 0;
            player.body.TargetMovement.x = player.body.Movement.x;
        }
        if (!aboveC && player.body.detection.collisions.above)
        {
            player.Sprite.GetComponent<SquashStrech>().ApplyMorph(1.2f, 2.2f, 0, 1);
        }
        float walldist = 0.05f;
        Vector2 wallcheck = new Vector2(walldist, 0);
        int n = player.body.detection.Cast(player.IsHuggingRight ? wallcheck : -wallcheck);
        if (n == 0 || player.IsGrounded || player.WalljumpHoldCounter > player.WalljumpUnHugTime)
        {
            if (player.state == player.states[2])
            {
                player.state = player.states[0];
                player.WalljumpHoldCounter = 0;
                player.body.detection.collisions.Reset();
                return;
            }
        }

        player.CreateOrb();
        //}
    }
}
