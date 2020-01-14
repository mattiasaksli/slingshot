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
    }

    public override bool CanHugWalls()
    {
        return controller.state == controller.states[2];
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (StoredMovement != Vector2.zero && Time.time > storedMovementRestTimer)
        {
            if (!detection.collisions.below && controller.state != controller.states[2] && TargetStoredMovement != Vector2.zero)
            {
                ReleaseStoredEnergy();
            }
        }
    }

    public override bool CanBeGrounded()
    {
        return true;
    }
}
