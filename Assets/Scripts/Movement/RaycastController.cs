using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Credit to Sebastian Lague for their Unity 2D Platformer series: https://github.com/SebLague/2DPlatformer-Tutorial
public class RaycastController : MonoBehaviour
{
    [HideInInspector]
    public const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public bool autoRay = true;
    public float autoRaySpacing = 0.2f;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;
    [HideInInspector]
    public float inwardRaySpacing;

    [HideInInspector]
    public BoxCollider2D collider;
    [HideInInspector]
    public RaycastOrigins raycastOrigins;

    public virtual void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);
        if (autoRay)
        {
            horizontalRayCount = Mathf.Clamp((int)(bounds.size.y / autoRaySpacing) + 2, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp((int)(bounds.size.x/autoRaySpacing)+2, 2, int.MaxValue);
        }
        else
        {
            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
        }

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        inwardRaySpacing = (bounds.size.y - skinWidth * 2) / (horizontalRayCount - 1);
    }
    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
