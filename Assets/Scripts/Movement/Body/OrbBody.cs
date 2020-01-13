using UnityEngine;
using System.Collections;

public class OrbBody : KinematicBody
{

    public override bool CanHugWalls()
    {
        return false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (TargetStoredMovement != Vector2.zero && StoredMovement != Vector2.zero && Time.time > storedMovementRestTimer)
        {
            StoredMovement *= 1.2f;
            ReleaseStoredEnergy();
        }
    }
}
