using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicBody : MonoBehaviour
{
    public Vector2 TargetMovement = new Vector2(0,0);
    public Vector2 Movement = new Vector2(0, 0);
    private float Acceleration;
    public float GroundAcceleration = 40;
    public float MidairAcceleration = 0;
    public bool IsGrounded = false;

    public BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsGrounded && TargetMovement.x == 0)
        {
            Acceleration = MidairAcceleration;
        }
        else
        {
            Acceleration = GroundAcceleration;
        }

        Vector2 dist = (TargetMovement - Movement);
        dist = dist.normalized * Mathf.Min(Acceleration*Time.deltaTime, dist.magnitude);
        Movement += dist;

        CollisionDetection.coll.Move(this,Movement * Time.deltaTime);
    }
}
