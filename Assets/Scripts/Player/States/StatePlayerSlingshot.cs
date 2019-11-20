using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerSlingshot : State
{
    public void Update(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        Vector2 towardsorb = new Vector2(player.orb.transform.position.x - player.transform.position.x, player.orb.transform.position.y - player.transform.position.y);
        var collisions = player.body.detection.collisions;
        bool collisionHappened = (collisions.above || collisions.below || collisions.left ||collisions.right);
        if (towardsorb.magnitude < (player.body.Movement * Time.deltaTime).magnitude || collisionHappened )
        {
            player.state = player.states[0];
            player.body.Movement = player.body.Movement.normalized * Mathf.Min(player.SlingShotMaxSpeed * 0.3f, player.body.Movement.magnitude * 0.7f);
            player.RecallOrb();
            player.AudioSlingShot?.Play();
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
