using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    public static CollisionDetection coll;
    private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(10);

    // Start is called before the first frame update
    void Start()
    {
        coll = this;
    }

    // Update is called once per frame
    public void Move(KinematicBody body, Vector2 add)
    {
        RaycastHit2D[] results = new RaycastHit2D[10];
        List<RaycastHit2D> resultsList = new List<RaycastHit2D>(10);
        int n;
        float padding = 0.002f;

        //Horizontal
        Vector2 xcomp = new Vector2(add.x, 0);
        n = body.boxCollider.Cast(xcomp, results);
        resultsList.Clear();
        for (var i = 0; i < n; i++)
        {
            resultsList.Add(results[i]);
        }
        if (n > 0)
        {
            for (var i = 0; i < resultsList.Count; i++)
            {
                float d = Mathf.Max(resultsList[i].distance - padding);
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
        resultsList.Clear();
        for (var i = 0; i < n; i++)
        {
            resultsList.Add(results[i]);
        }
        if (n > 0)
        {
            for (var i = 0; i < resultsList.Count; i++)
            {
                float d = Mathf.Max(resultsList[i].distance - padding);
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
