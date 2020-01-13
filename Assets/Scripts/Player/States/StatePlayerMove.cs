using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerMove : State
{
    // Start is called before the first frame update
    public void Update(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        float input = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        player.body.TargetMovement.x = input * player.MovementSpeed;
        if (Input.GetKeyDown("space") && player.IsGrounded)
        {
            player.body.TargetMovement.y = player.JumpPower;
            player.body.Movement.y = player.JumpPower;
            player.IsJumping = true;
            player.Sprite.GetComponent<SquashStrech>().ApplyMorph(0.5f, 3.2f,0,-1);
            float walldist = 0.05f;
            Vector2 wallcheck = new Vector2(walldist, 0);
            int wall = player.body.detection.Cast(wallcheck);
            if (player.body.detection.collisions.right) { RightHug(); }
            wall = player.body.detection.Cast(-wallcheck);
            if (player.body.detection.collisions.left) { LeftHug(); }
            player.AudioJump?.Play();
            player.body.ReleaseStoredEnergy();
        }

        player.WalljumpHoldCounter = Mathf.Max(0,player.WalljumpHoldCounter - Time.deltaTime);

        if (player.body.Movement.y <= 0)
        {
            player.IsJumping = false;
        }
        if (Mathf.Abs(player.body.Movement.x) <= 0.1)
        {
            player.IsWallJumping = false;
        }

        if (!player.IsGrounded)
        {
            if(player.body.detection.collisions.right && input > 0)
            {
                RightHug();
                player.Sprite.GetComponent<SquashStrech>().ApplyMorph(0.7f, 3.2f, -1, 0);
            }
            if(player.body.detection.collisions.left && input < 0)
            {
                LeftHug();
                player.Sprite.GetComponent<SquashStrech>().ApplyMorph(0.7f, 3.2f, -1, 0);
            }
        }

        player.body.Acceleration = player.AccelerationGround;
        if(Mathf.Round(Input.GetAxis("Horizontal")) == 0)
        {
            player.body.Acceleration = player.IsGrounded ? player.AccelerationGround : player.AccelerationAir;
        }
        if(!player.IsGrounded && Mathf.Sign(player.body.TargetMovement.x) == Mathf.Sign(player.body.Movement.x) && Mathf.Abs(player.body.TargetMovement.x) < Mathf.Abs(player.body.Movement.x) || player.WalljumpHoldCounter > 0)
        {
            player.body.Acceleration = player.AccelerationAir;
        }

        if (!Input.GetKey("space") && player.IsJumping)
        {
            if (player.IsJumping)
            {
                player.body.TargetMovement.y *= 0.6f;
                player.body.Movement.y *= 0.6f;
                player.IsJumping = false;
            }
            if (player.IsWallJumping && player.WalljumpHoldCounter > player.WalljumpMinHoldTime)
            {
                player.body.Movement.x *= 0.6f;
                player.IsWallJumping = false;
                player.WalljumpHoldCounter = 0;
            }
        }

        if(player.WalljumpHoldCounter == 0 && player.IsWallJumping)
        {
            player.body.Movement.x *= 0.6f;
            player.IsWallJumping = false;
        }

        player.OrbBehaviour();

        void RightHug()
        {
            player.state = player.states[2];
            player.body.Movement.y = Mathf.Max(0, player.body.Movement.y);
            player.IsHuggingRight = true;
            player.WalljumpHoldCounter = 0;
            player.IsGrounded = false;
            if (player.body.Movement.x > 0)
            {
                //player.Sprite.GetComponent<SquashStrech>().ApplyMorph(0.7f, 3.2f, 1, 0);
            }
        }
        void LeftHug()
        {
            player.state = player.states[2];
            player.body.Movement.y = Mathf.Max(0, player.body.Movement.y);
            player.IsHuggingRight = false;
            player.WalljumpHoldCounter = 0;
            player.IsGrounded = false;
            if (player.body.Movement.x < 0)
            {
                //player.Sprite.GetComponent<SquashStrech>().ApplyMorph(0.7f, 3.2f, 1, 0);
            }
        }
    }

    // Update is called once per frame
    public void FixedUpdate(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
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
