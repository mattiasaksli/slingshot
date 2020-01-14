using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerSlingshot : State
{
    private float disconnectTimestamp = 0;
    public float stateStartTime;
    public Vector2 initialDirection;


    public void Update(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        Vector2 towardsorb = player.orb.transform.position - player.transform.position;
        var detection = player.body.detection;
    }
    public void FixedUpdate(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        Vector2 lastNormal = player.body.detection.collisionNormal;
        var detection = player.body.detection;
        Vector2 towardsorb = player.orb.transform.position - player.transform.position;
        player.body.Acceleration = player.SlingShotAcceleration;
        player.body.TargetMovement = towardsorb.normalized * player.SlingShotMaxSpeed;
        player.body.StoredMovement = Vector2.zero;
        if (towardsorb.normalized != Vector2.zero)
        {
            player.body.Movement = player.body.Movement.magnitude * towardsorb.normalized;
        }
        player.body.Move(Mathf.Min((player.body.Movement * Time.deltaTime).magnitude, towardsorb.magnitude)*player.body.Movement.normalized);
        if(lastNormal == Vector2.zero && detection.collisionNormal != Vector2.zero)
        {
            float s = disconnectTime(player, detection, towardsorb);
            disconnectTimestamp = Time.time + Mathf.Max(0.05f,s*3);
        }
        var lowD = LowestDot(player, detection, towardsorb);
        Debug.Log(towardsorb.magnitude);
        if (towardsorb.magnitude <= 0.1f || (lowD < 0.2 && Time.time > disconnectTimestamp) || lowD < -0.9f)
        {
            player.state = player.states[0];
            player.body.Movement = player.body.Movement.normalized * Mathf.Min(player.SlingShotMaxSpeed * 0.3f, player.body.Movement.magnitude * 0.7f);
            player.RecallOrb();
            player.AudioSlingShot?.Play();
        }
    }

    private float LowestDot(PlayerController player, CollisionDetection detection, Vector2 towardsorb) {
        float[] dots = { detection.collisions.above? Vector2.Dot(Vector2.down, towardsorb.normalized) : 1,
            detection.collisions.below? Vector2.Dot(Vector2.up, towardsorb.normalized) : 1,
            detection.collisions.right? Vector2.Dot(Vector2.left, towardsorb.normalized) : 1,
            detection.collisions.left? Vector2.Dot(Vector2.right, towardsorb.normalized) : 1};

        float lowestDot = dots[0];

        for (int i = 1; i < 4; i++)
        {
            if (dots[i] < lowestDot)
            {
                lowestDot = dots[i];
            }
        }

        return lowestDot;
    }

    private float disconnectTime(PlayerController player, CollisionDetection detection, Vector2 towardsorb)
    {
        float[] dots = { detection.collisions.above? Vector2.Dot(Vector2.down, towardsorb.normalized) : 1,
            detection.collisions.below? Vector2.Dot(Vector2.up, towardsorb.normalized) : 1,
            detection.collisions.right? Vector2.Dot(Vector2.left, towardsorb.normalized) : 1,
            detection.collisions.left? Vector2.Dot(Vector2.right, towardsorb.normalized) : 1};
        float[] percents = { detection.freeRays.below, detection.freeRays.above, detection.freeRays.left, detection.freeRays.right };

        float lowestDot = dots[0];
        float lowestPercent = percents[0];

        for (int i = 1; i < 4; i++)
        {
            if (dots[i] < lowestDot)
            {
                lowestDot = dots[i];
                lowestPercent = percents[i];
            }
        }

        return (lowestDot * 0.5f+0.5f)*(1f- lowestPercent);
    }
}
