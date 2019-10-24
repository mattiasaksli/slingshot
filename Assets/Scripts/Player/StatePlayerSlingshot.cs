﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerSlingshot : State
{
    public void Update(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        Vector2 towardsorb = new Vector2(player.orb.transform.position.x - player.transform.position.x, player.orb.transform.position.y - player.transform.position.y);
        var collisions = player.body.detection.collisionDirections;
        bool collisionHappened = (collisions[0] && player.body.Movement.y <= 0) || (collisions[1] && player.body.Movement.x >= 0) || (collisions[2] && player.body.Movement.y >= 0) || (collisions[3] && player.body.Movement.x <= 0);
        if (towardsorb.magnitude < (player.body.Movement * Time.deltaTime).magnitude || collisionHappened )
        {
            player.state = player.states[0];
            player.body.Movement = player.body.Movement.normalized * player.SlingShotMaxSpeed * 0.3f ;
            player.RecallOrb();
        }
    }
    public void FixedUpdate(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        Vector2 towardsorb = new Vector2(player.orb.transform.position.x - player.transform.position.x, player.orb.transform.position.y - player.transform.position.y);
        player.body.Acceleration = player.SlingShotAcceleration;
        player.body.TargetMovement = towardsorb.normalized * player.SlingShotMaxSpeed;
        player.body.Move(player.body.Movement * Time.deltaTime);
    }
}
