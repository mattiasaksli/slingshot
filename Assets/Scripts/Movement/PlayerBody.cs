using UnityEngine;
using System.Collections;

public class PlayerBody : KinematicBody
{
    private PlayerController controller;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
        //Physics2D.autoSyncTransforms = true;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!detection.collisions.below && controller.state != controller.states[2] && StoredMovement != Vector2.zero && Movement.y != -0.1f)
        {
            Movement += StoredMovement;
            StoredMovement = Vector2.zero;
            TargetStoredMovement = Vector2.zero;
        }
    }
}
