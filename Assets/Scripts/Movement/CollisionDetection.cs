using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public float padding = 0.002f;

    [HideInInspector]
    public bool[] collisionDirections { get; private set; } = new bool[4];
    private BoxCollider2D boxCollider2D;
    private List<RaycastHit2D> hitBufferList;
    private RaycastHit2D[] results;
    private ContactFilter2D filter;
    private KinematicBody body;
    private List<RaycastHit2D> resultsList;

    // Start is called before the first frame update
    void Start()
    {
        results = new RaycastHit2D[10];
        body = gameObject.GetComponent<KinematicBody>();
        filter = new ContactFilter2D();
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(body.gameObject.layer));
        resultsList = new List<RaycastHit2D>(10);
    }

    // Update is called once per frame
    public void Move(Vector2 add)
    {
        int n;

        //Horizontal
        collisionDirections = new bool[4] { false, false, false, false };
        Vector2 xcomp = new Vector2(add.x, 0);
        n = body.collider2d.Cast(xcomp, results);
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
                    if (resultsList[i].normal == Vector2.left)
                    {
                        collisionDirections[1] = true;
                    }
                    else
                    {
                        collisionDirections[3] = true;
                    }
                    xcomp = xcomp.normalized * d;
                    body.TargetMovement.x = 0;
                    body.Movement.x = 0;
                    /*if (!body.IsGrounded)
                    {
                        float walljumpHorizontal = resultsList[i].normal.x;
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            body.Movement.x = walljumpHorizontal * WalljumpPower;
                            body.Movement.y = WalljumpPower;
                            isWalljumping = true;
                            if (walljumpHorizontal > 0)
                            {
                                walljumpingRight = true;
                            }
                            else
                            {
                                walljumpingRight = false;
                            }
                        }
                    }*/
                }
            }
        }
        body.transform.position += (Vector3)xcomp;

        //Vertical
        Vector2 ycomp = new Vector2(0, add.y);
        n = body.collider2d.Cast(ycomp, results);
        resultsList.Clear();
        for (var i = 0; i < n; i++)
        {
            resultsList.Add(results[i]);
        }
        //body.IsGrounded = false;
        if (n > 0)
        {
            for (var i = 0; i < resultsList.Count; i++)
            {
                float d = Mathf.Max(resultsList[i].distance - padding);
                if (d <= ycomp.magnitude)
                {
                    if (resultsList[i].normal == Vector2.up) {
                        collisionDirections[0] = true;
                    } else
                    {
                        collisionDirections[2] = true;
                    }
                    ycomp = ycomp.normalized * d;
                    body.TargetMovement.y = 0;
                    body.Movement.y = 0;
                }
            }
        }
        body.transform.position += (Vector3)ycomp;
    }
}
