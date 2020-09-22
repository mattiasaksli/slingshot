using UnityEngine;

public class PingPongPlatform : PlatformController
{
    public float MaxSpeed;
    public float Speed;
    public float Acceleration;
    public float Rest;

    private float restTimer;
    int fromWaypointIndex;
    float percentBetweenWaypoints;
    private bool rightway = true;
    public override void Start()
    {
        base.Start();
    }

    public override void OnPlayerRespawn()
    {
        base.OnPlayerRespawn();
        fromWaypointIndex = 0;
        percentBetweenWaypoints = 0;
        Speed = 0;
        if (!rightway)
        {
            System.Array.Reverse(globalWaypoints);
        }
        restTimer = Time.time;
        rightway = true;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        float minMovementAudioThreshold = 0.01f;
        if (Movement.magnitude > minMovementAudioThreshold && !audioSource.loop)
        {
            audioSource.loop = true;
            audioSource.volume = 0.5f * Volume;
            audioSource.clip = AudioMove.AudioClips[Random.Range(0, AudioMove.AudioClips.Count)];
            audioSource.Play();
        }
        if (Movement.magnitude < minMovementAudioThreshold && audioSource.loop)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.volume = 0.5f * Volume * StopVolume;
            audioSource.clip = AudioStop.AudioClips[Random.Range(0, AudioStop.AudioClips.Count)];
            audioSource.Play();
        }
        if (roomActive)
        {
            var dist = MaxSpeed - Speed;
            if (Time.time > restTimer)
            {
                Speed += Mathf.Min(dist, Mathf.Sign(dist) * Acceleration * Time.deltaTime);
            }
            Movement = CalculatePlatformMovement();

            Move(Movement);
        }
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
                rightway = !rightway;
            }
        }

        return newPos - transform.position;
    }

    public override Vector3 GetStoredMovement()
    {
        return Movement.normalized * Speed;
    }

}
