using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    public static CollisionDetection coll;
    
    // Start is called before the first frame update
    void Start()
    {
        coll = this;
    }

    // Update is called once per frame
    public void Move(KinematicBody body, Vector2 add)
    {
        RaycastHit2D[] results = new RaycastHit2D[4];

        //Horizontal
        Vector2 xcomp = new Vector2(add.x, 0);
        int n = body.boxCollider.Cast(xcomp, results);
        if (n > 0)
        {
            for (var i = 0; i < n; i++)
            {
                float d = Mathf.Max(results[i].distance - 0.005f);
                if (d < xcomp.magnitude)
                {
                    xcomp = xcomp.normalized * d;
                    body.TargetMovement.x = 0;
                    body.Movement.x = 0;
                }
            }
        }
        body.transform.position += (Vector3)xcomp;

        //Vertical
        Vector2 ycomp = new Vector2(0, add.y);
        n = body.boxCollider.Cast(ycomp, results);
        if(n > 0)
        {
            for (var i = 0; i < n; i++)
            {
                float d = Mathf.Max(results[i].distance - 0.005f);
                if (d < ycomp.magnitude)
                {
                    ycomp = ycomp.normalized * d;
                    body.TargetMovement.y = 0;
                    body.Movement.y = 0;
                }
            }
        }
        body.transform.position += (Vector3)ycomp;
    }
}
