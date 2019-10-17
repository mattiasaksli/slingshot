using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    public static CollisionDetection coll;
    private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(10);

    public float WalljumpPower = 10f;

    // Start is called before the first frame update
    void Start()
    {
        coll = this;
    }

    // Update is called once per frame
    public void Move(KinematicBody body, Vector2 add)
    {
        RaycastHit2D[] results = new RaycastHit2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(body.gameObject.layer));
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

                    if (!body.IsGrounded)
                    {
                        float walljumpHorizontal = resultsList[i].normal.x;
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            body.Movement.x = walljumpHorizontal * WalljumpPower;
                            body.Movement.y = WalljumpPower;
                        }
                    }
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
        body.IsGrounded = false;
        if (n > 0)
        {
            for (var i = 0; i < resultsList.Count; i++)
            {
                float d = Mathf.Max(resultsList[i].distance - padding);
                if (d <= ycomp.magnitude)
                {
                    if (resultsList[i].normal == Vector2.up) { body.IsGrounded = true; }
                    ycomp = ycomp.normalized * d;
                    body.TargetMovement.y = 0;
                    body.Movement.y = 0;
                }
            }
        }
        body.transform.position += (Vector3)ycomp;
    }
}
