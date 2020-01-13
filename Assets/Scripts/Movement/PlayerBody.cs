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

    public override bool CanHugWalls()
    {
        return controller.state == controller.states[2];
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!detection.collisions.below && controller.state != controller.states[2] && StoredMovement != Vector2.zero && Time.time > storedMovementRestTimer && !detection.MovedByPlatform)
        {
            ReleaseStoredEnergy();
        }
    }

    public void ReleaseStoredEnergy()
    {
        Movement += StoredMovement;
        StoredMovement = Vector2.zero;
        TargetStoredMovement = Vector2.zero;
        detection.MovedByPlatform = false;
    }
}
