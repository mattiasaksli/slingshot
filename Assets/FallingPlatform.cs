using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : PlatformController
{
    public bool triggered = false;
    public bool StartTime;
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
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
            Movement.y = -FallSpeed;
        }
    }

    public override Vector3 GetStoredMovement()
    {
        return Movement.normalized * FallSpeed;
    }
}
