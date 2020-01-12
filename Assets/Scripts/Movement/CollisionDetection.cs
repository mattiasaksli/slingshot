using System.Collections.Generic;
using UnityEngine;


//Credit to Sebastian Lague for their Unity 2D Platformer series: https://github.com/SebLague/2DPlatformer-Tutorial

public class CollisionDetection : RaycastController
{
    public LayerMask collisionMask;
    public LayerMask suffocationMask;

    public CollisionInfo collisions;
    public List<Transform> InsideCollisions;


    public override void Start()
    {
        base.Start();
    }

    public void Move(Vector3 velocity, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
        InsideCollisions = CheckInside();
        
        if (standingOnPlatform)
        {
            collisions.below = true;
        }
    }

    public List<Transform> CheckInside()
    {
        float rayLength = collider.bounds.size.x - skinWidth * 2;
        List<Transform> _InsideCollisions = new List<Transform>();
        Physics2D.SyncTransforms();

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.down * (inwardRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, collisionMask);

            if(hit) {
                if (!_InsideCollisions.Contains(hit.transform))
                {
                    _InsideCollisions.Add(hit.transform);
                }
            }
            
        }
        return _InsideCollisions;
    }

    public int Cast(Vector3 velocity)
    {
        Vector3 originalVelocity = new Vector3(velocity.x,velocity.y,velocity.z);

        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        return originalVelocity.Equals(velocity) ? 0 : 1;
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                if (!hit.transform.GetComponent<OneWayPlatform>())
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                if (!hit.transform.GetComponent<OneWayPlatform>() || directionY != 1)
                {
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;
                }
            }
        }
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}
