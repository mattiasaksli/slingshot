using UnityEngine;
using System.Collections;

public class PingPongPlatform : PlatformController
{
    public float MaxSpeed;
    public float Speed;
    public float Acceleration;
    public float Rest;

    private float restTimer;
    int fromWaypointIndex;
    float percentBetweenWaypoints;
    public override void Start()
    {
        base.Start();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        var dist = MaxSpeed - Speed;
        if (Time.time > restTimer)
        {
            Speed += Mathf.Min(dist, Mathf.Sign(dist) * Acceleration * Time.deltaTime);
        }
        Movement = CalculatePlatformMovement();

        Move(Movement);
    }
    Vector3 CalculatePlatformMovement()
    {
        int toWaypointIndex = fromWaypointIndex + 1;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.fixedDeltaTime * Speed / distanceBetweenWaypoints;

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], percentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;
            restTimer = Time.time + Rest;
            Speed = 0;
            if (fromWaypointIndex >= globalWaypoints.Length - 1)
            {
                fromWaypointIndex = 0;
                System.Array.Reverse(globalWaypoints);
            }
        }

        return newPos - transform.position;
    }

    public override Vector3 GetStoredMovement()
    {
        return Movement.normalized*Speed;
    }
}
