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

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        StoredMovement = Vector2.Lerp(StoredMovement, TargetStoredMovement, 1f);
        if (!detection.collisions.below && controller.state != controller.states[2] && StoredMovement != Vector2.zero && Movement.y != -0.1f)
        {
            Debug.Log("Stored Energy Released");
            Movement += StoredMovement;
            StoredMovement = Vector2.zero;
            TargetStoredMovement = Vector2.zero;
        }
    }
}
