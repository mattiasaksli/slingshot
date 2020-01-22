using System.Collections.Generic;
using UnityEngine;


//Credit to Sebastian Lague for their Unity 2D Platformer series: https://github.com/SebLague/2DPlatformer-Tutorial

public class CollisionDetection : RaycastController
{
    public LayerMask collisionMask;
    public LayerMask suffocationMask;

    public CollisionInfo collisions;
    public FreeRays freeRays;
    public List<Transform> InsideCollisions;
    private Dictionary<Transform, PlatformController> platformdictionary;

    public bool MovedByPlatform;
    public Vector2 collisionNormal;


    public override void Start()
    {
        base.Start();
        platformdictionary = new Dictionary<Transform, PlatformController>();
    }

    public void Move(Vector3 velocity, bool standingOnPlatform = false, bool leftCollision = false, bool rightCollision = false, bool belowCollision = false)
    {
        //MovedByPlatform = false;
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
        if (leftCollision)
        {
            collisions.right = true;
        }
        if (rightCollision)
        {
            collisions.left = true;
        }
        if (belowCollision)
        {
            collisions.above = true;
        }
    }

    public RaycastHit2D CastRay(Vector2 Direction, float PercentAlongSide, float rayLength)
    {
        float directionX = Mathf.Sign(Direction.x);
        float directionY = Mathf.Sign(Direction.y);

        if (directionX != 0) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * (int)(PercentAlongSide*horizontalRayCount));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            return hit;
        } else {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * (int)(PercentAlongSide*verticalRayCount));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            return hit;
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
        collisionNormal = Vector2.zero;

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
        int[] rayhits = { 0, 0 };

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                if(!platformdictionary.ContainsKey(hit.transform))
                {
                    var cont = hit.transform.GetComponent<PlatformController>();
                    if (cont)
                    {
                        platformdictionary.Add(hit.transform, cont);
                    }
                }
                if (platformdictionary.ContainsKey(hit.transform))
                {
                    platformdictionary[hit.transform].OnMovingTransform?.Invoke(transform);
                }
                rayhits[directionX == -1 ? 0 : 1]++;
                //var cont = hit.transform.GetComponent<PlatformController>();

                if (!hit.transform.GetComponent<OneWayPlatform>())
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;
                    collisionNormal = hit.normal;

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
        freeRays.left = (rayhits[0] / (float)horizontalRayCount);
        freeRays.right = rayhits[1] / (float)horizontalRayCount;
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        int[] rayhits = new int[2];

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                rayhits[directionY == -1 ? 0 : 1]++;
                if (!platformdictionary.ContainsKey(hit.transform))
                {
                    var cont = hit.transform.GetComponent<PlatformController>();
                    if (cont)
                    {
                        platformdictionary.Add(hit.transform, cont);
                    }
                }
                if (platformdictionary.ContainsKey(hit.transform))
                {
                    platformdictionary[hit.transform].OnMovingTransform?.Invoke(transform);
                }
                if (!hit.transform.GetComponent<OneWayPlatform>() || directionY != 1)
                {
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;
                    collisionNormal = hit.normal;

                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;
                }
            }
        }
        freeRays.below = rayhits[0] / (float)verticalRayCount;
        freeRays.above = rayhits[1] / (float)verticalRayCount;
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

    public struct FreeRays
    {
        public float above, below;
        public float left, right;

        public void Reset()
        {
            above = below = 0;
            left = right = 0;
        }
    }
}
