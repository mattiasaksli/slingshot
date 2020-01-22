using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : PlatformController
{
    [HideInInspector]
    public bool triggered = false;
    [HideInInspector]
    private bool isFalling;
    [HideInInspector]
    private bool hasBudged;
    public float StartTime;
    private float StartTimestamp;
    public float FallSpeed;

    protected override void Awake()
    {
        base.Awake();
        OnMovingTransform += OnContact;
    }

    public override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnMovingTransform -= OnContact;
    }

    public override void OnPlayerRespawn()
    {
        base.OnPlayerRespawn();
        triggered = false;
        Movement = Vector2.zero;
        StartTimestamp = Time.time;
        isFalling = false;
        hasBudged = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(Time.time > StartTimestamp && !isFalling && triggered)
        {
            isFalling = true;
            Movement.y = -FallSpeed;
        }
        if(!hasBudged && triggered)
        {
            Move(Vector2.down * 0.15f);
            hasBudged = true;
        }
        Move(Movement*Time.deltaTime);
    }

    public void OnContact(Transform othertransform)
    {
        if (othertransform.GetComponent<PlayerController>())
        {
            Triggered();
        }
    }

    public void Triggered()
    {
        if(!triggered)
        {
            triggered = true;
            audioSource.loop = false;
            audioSource.volume = 1 * Volume * StopVolume;
            audioSource.clip = AudioMove.AudioClips[Random.Range(0, AudioStop.AudioClips.Count)];
            audioSource.Play();
            StartTimestamp = Time.time + StartTime;
        }
    }

    public override Vector3 GetStoredMovement()
    {
        return Movement.normalized * FallSpeed;
    }

    protected override void Update()
    {
        
    }
}
