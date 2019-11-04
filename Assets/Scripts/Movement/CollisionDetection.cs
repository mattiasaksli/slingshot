using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public static float padding = 0.01f;

    
    public bool[] collisionDirections = new bool[4];
    private Collider2D boxCollider2D;
    private List<RaycastHit2D> hitBufferList;
    public RaycastHit2D[] results;
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
        hitBufferList = new List<RaycastHit2D>(10);
    }

    public int Cast(Vector2 ray)
    {
        return body.collider2d.Cast(ray, filter, results, ray.magnitude);
    }

    // Update is called once per frame
    public void Move(Vector2 add)
    {
        int n;

        collisionDirections = new bool[4] { false, false, false, false };

        //Horizontal
        Vector2 xcomp = new Vector2(add.x, 0);
        n = body.collider2d.Cast(xcomp, filter, results, xcomp.magnitude);
        hitBufferList.Clear();
        for (var i = 0; i < n; i++)
        {
            hitBufferList.Add(results[i]);
        }
        if (n > 0)
        {
            for (var i = 0; i < n; i++)
            {
                float d = Mathf.Max(0, hitBufferList[i].distance - padding);
                if (gameObject.GetComponent<OrbController>() != null) { Debug.Log("X: " + d); }
                if (d < xcomp.magnitude)
                {
                    if (hitBufferList[i].normal == Vector2.left)
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
                }
            }
        }
        body.collider2d.transform.position += (Vector3)xcomp;

        //Vertical
        Vector2 ycomp = new Vector2(0, add.y);
        n = body.collider2d.Cast(ycomp, filter, results, ycomp.magnitude);
        hitBufferList.Clear();
        for (var i = 0; i < n; i++)
        {
            hitBufferList.Add(results[i]);
        }
        if (n > 0)
        {
            for (var i = 0; i < n; i++)
            {
                float d = Mathf.Max(0, hitBufferList[i].distance - padding);
                if (gameObject.GetComponent<OrbController>() != null) { Debug.Log("Y: " + d); }
                if (d < ycomp.magnitude)
                {
                    if (hitBufferList[i].normal == Vector2.up)
                    {
                        collisionDirections[0] = true;
                    }
                    else
                    {
                        collisionDirections[2] = true;
                    }
                    ycomp = ycomp.normalized * d;
                    body.TargetMovement.y = 0;
                    body.Movement.y = 0;
                }
            }
        }
        body.collider2d.transform.position += (Vector3)ycomp;
        /*add.x = Mathf.Abs(add.x) > padding ? add.x : 0;
        add.y = Mathf.Abs(add.y) > padding ? add.y : 0;
        {
            Vector2 ycomp = new Vector2(add.x, add.y);
            n = body.collider2d.Cast(ycomp, filter, results, ycomp.magnitude);
            hitBufferList.Clear();
            for (var i = 0; i < n; i++)
            {
                hitBufferList.Add(results[i]);
            }
            Debug.Log(n);
            if (n > 0)
            {
                for (var i = 0; i < n; i++)
                {
                    float d = hitBufferList[i].distance;
                    Vector2 temp = (ycomp.normalized * d);
                    temp.x -= Mathf.Min(padding,Mathf.Abs(temp.x)) * Mathf.Sign(temp.x);
                    temp.y -= Mathf.Min(padding, Mathf.Abs(temp.y)) * Mathf.Sign(temp.y);
                    d = Mathf.Max(0,temp.magnitude);
                    if (d < ycomp.magnitude)
                    {
                        int xmodif = 1;
                        int ymodif = 1;
                        if (hitBufferList[i].normal == Vector2.up && add.y < 0)
                        {
                            collisionDirections[0] = true;
                            body.TargetMovement.y = 0;
                            body.Movement.y = 0;
                            if (d == 0) { ymodif = 0; }

                        }
                        if (hitBufferList[i].normal == Vector2.down && add.y > 0)
                        {
                            collisionDirections[2] = true;
                            body.TargetMovement.y = 0;
                            body.Movement.y = 0;
                            if (d == 0) { ymodif = 0; }
                        }
                        if (hitBufferList[i].normal == Vector2.left && add.x > 0)
                        {
                            collisionDirections[1] = true;
                            body.TargetMovement.x = 0;
                            body.Movement.x = 0;
                            if (d == 0) { xmodif = 0; }
                        }
                        if (hitBufferList[i].normal == Vector2.right && add.x < 0)
                        {
                            collisionDirections[3] = true;
                            body.TargetMovement.x = 0;
                            body.Movement.x = 0;
                            if (d == 0) { xmodif = 0; }
                        }
                        if (d > 0) { ycomp = ycomp.normalized * d; }
                        else
                        {
                            ycomp.x *= xmodif;
                            ycomp.y *= ymodif;
                        }
                    }
                }
            }
            body.collider2d.transform.position += (Vector3)ycomp;
        }*/

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
