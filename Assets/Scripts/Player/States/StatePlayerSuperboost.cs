using UnityEngine;

public class StatePlayerSuperboost : State
{
    private float disconnectTimestamp = 0;
    public float stateStartTime;
    public Vector2 initialDirection;
    private Vector2 towardsorb;


    public void Update(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
    }
    public void FixedUpdate(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;

        player.IsGrounded = false;
        player.body.Acceleration = 0;
        player.body.TargetMovement = player.body.Movement;
        player.body.StoredMovement = Vector2.zero;
        player.body.Move(player.body.Movement*Time.deltaTime);
        var detection = player.body.detection;
        if (detection.collisions.left || detection.collisions.right || detection.collisions.above || detection.collisions.below)
        {
            Slingshot(player);
        }
    }

    public void Slingshot(PlayerController player)
    {
            player.state = player.states[0];
            player.body.Movement = player.body.Movement.normalized * Mathf.Min(player.SlingShotMaxSpeed * 0.3f, player.body.Movement.magnitude * 0.7f);
            /*if (towardsorb.magnitude <= 0.1f)
            {
                Transform p = GameObject.Instantiate<ParticleSystem>(player.SlingshotParticle).transform;
                p.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, player.body.Movement));
                p.transform.position = player.transform.position;
            }*/
            GameObject.FindGameObjectWithTag("Player").GetComponent<TrailRenderer>().emitting = false;
            player.RecallOrb();
    }
}
