using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerVictory : State
{
    // Start is called before the first frame update
    public void Update(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        player.body.Acceleration = player.AccelerationGround;
        if (Mathf.Round(Input.GetAxis("Horizontal")) == 0)
        {
            player.body.Acceleration = player.IsGrounded ? player.AccelerationGround : player.AccelerationAir;
        }
        if (!player.IsGrounded && Mathf.Sign(player.body.TargetMovement.x) == Mathf.Sign(player.body.Movement.x) && Mathf.Abs(player.body.TargetMovement.x) < Mathf.Abs(player.body.Movement.x) || player.WalljumpHoldCounter > 0 || player.JumpPadTimestamp > Time.time)
        {
            player.body.Acceleration = player.AccelerationAir;
        }
    }

    // Update is called once per frame
    public void FixedUpdate(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        player.body.TargetMovement.x = 0;
        player.body.Movement.y -= player.GravityPower * Time.deltaTime;
        player.body.TargetMovement.y = player.body.Movement.y;
        bool aboveC = player.body.detection.collisions.above;
        var referenceY = player.body.Movement.y; //Used to determine whether to play the landing animation or not
        player.body.Move(player.body.Movement * Time.deltaTime);

        if(player.body.detection.collisions.below || player.body.detection.collisions.above)
        {
            player.body.Movement.y = 0;
            player.body.TargetMovement.y = player.body.Movement.y;
        }

        if (player.body.detection.collisions.right || player.body.detection.collisions.left)
        {
            player.body.Movement.x = 0;
            player.body.TargetMovement.x = player.body.Movement.x;
        }
        bool g = player.IsGrounded;
        player.IsGrounded = player.body.detection.collisions.below;
        if(player.IsGrounded && !g/* && referenceY < -1.7*/)
        {
            player.Sprite.GetComponent<SquashStrech>().ApplyMorph(1.2f, 2.2f,0,-1);
            player.AudioLand?.Play();
        }
        if(!aboveC && player.body.detection.collisions.above)
        {
            player.Sprite.GetComponent<SquashStrech>().ApplyMorph(1.2f, 2.2f, 0, 1);
        }

        if(player.IsGrounded)
        {
            player.IsOrbAvailable = true;
        }


        player.CreateOrb();
    }
}
