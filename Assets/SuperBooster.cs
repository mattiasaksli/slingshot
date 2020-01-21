using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBooster : MonoBehaviour
{
    public CircleCollider2D DetectRadius;
    public CircleCollider2D LatchRadius;
    [HideInInspector]
    public OrbController orb;
    [HideInInspector]
    public ParticleSystem particle;
    public OrbController detectorb;
    public float LatchStrength = 20f;

    private void Start()
    {
        particle = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var o = collision.gameObject.GetComponent<OrbController>();
        if (LatchRadius.IsTouching(collision))
        {
            if (o)
            {
                orb = o;
                orb.isSuperBoosting = this;
            }
        }
        if(o)
        {
            detectorb = o;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var o = collision.gameObject.GetComponent<OrbController>();
        if(o == detectorb)
        {
            detectorb = null;
        }
    }

    private void FixedUpdate()
    {
        if(orb)
        {
            orb.body.Movement = Vector2.zero;
            orb.body.TargetMovement = Vector2.zero;
            orb.body.Move((transform.position - orb.transform.position)*LatchStrength*Time.deltaTime);
            if(orb.isSuperBoosting != this)
            {
                orb = null;
            }
        } else
        {
            orb = null;
        }
        if(detectorb && !orb)
        {
            var t = (detectorb.transform.position- transform.position);
            particle.transform.localPosition = t;//(1-(t.magnitude/LatchRadius.radius)) * t.normalized* 2f;
        } else
        {
            particle.transform.localPosition = Vector3.zero;
        }
    }
}
