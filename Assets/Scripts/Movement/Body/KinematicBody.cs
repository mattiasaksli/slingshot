using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicBody : MonoBehaviour
{
    public Vector2 TargetMovement = new Vector2(0,0);
    public Vector2 Movement = new Vector2(0, 0);

    public Vector2 TargetStoredMovement = new Vector2(0, 0);
    public Vector2 StoredMovement = new Vector2(0, 0);
    public float StoredMovementRest = 0.2f;

    [HideInInspector]
    public float storedMovementRestTimer = 0f;

    private float storedRetention = 0.3f;
    private float retentionTime = 0;

    public float Acceleration;

    [HideInInspector]
    public Collider2D collider2d;
    [HideInInspector]
    public CollisionDetection detection { get; private set; }

    public virtual void Start()
    {
        detection = gameObject.GetComponent<CollisionDetection>();
        collider2d = gameObject.GetComponent<BoxCollider2D>();
    }

    public virtual bool CanHugWalls()
    {
        return false;
    }

    //Delays stored movement
    public void SetStoredMovementRestTime()
    {
        if (TargetStoredMovement == Vector2.zero)
        {
            storedMovementRestTimer = Time.time + StoredMovementRest;
        }
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        Movement = AccelerateVector(Movement, TargetMovement, Acceleration);
        if(!detection.MovedByPlatform)
        {
            if(Time.time > retentionTime)
            {
                StoredMovement = Vector2.zero;
            }
            TargetStoredMovement = Vector2.zero;
        } else
        {
            if (TargetStoredMovement == Vector2.zero)
            {
                if (Time.time > retentionTime)
                {
                    StoredMovement = TargetStoredMovement;
                }
            } else
            {
                retentionTime = Time.time + storedRetention;
                StoredMovement = TargetStoredMovement;
            }
        }
    }

    public Vector2 AccelerateVector(Vector2 _Start, Vector2 _Target, float _Acceleration)
    {
        Vector2 dist = (_Target - _Start);
        dist = dist.normalized * Mathf.Min(_Acceleration * Time.deltaTime, dist.magnitude);
        _Start += dist;
        return _Start;
    }

    public void Move(Vector2 Movement)
    {
        detection.Move(Movement);
    }

    public void ReleaseStoredEnergy()
    {
        Movement += StoredMovement;
        StoredMovement = Vector2.zero;
        TargetStoredMovement = Vector2.zero;
        detection.MovedByPlatform = false;
    }
}
