using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicBody : MonoBehaviour
{
    public Vector2 TargetMovement = new Vector2(0,0);
    public Vector2 Movement = new Vector2(0, 0);
    public float Acceleration;

    public Collider2D collider2d;
    [HideInInspector]
    public CollisionDetection detection { get; private set; }

    private void Start()
    {
        detection = gameObject.GetComponent<CollisionDetection>();
        collider2d = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 dist = (TargetMovement - Movement);
        dist = dist.normalized * Mathf.Min(Acceleration*Time.deltaTime, dist.magnitude);
        Movement += dist;
    }

    public void Move(Vector2 Movement)
    {
        detection.Move(Movement);
    }
}
