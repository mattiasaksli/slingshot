using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerDead : State
{
    public void Update(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
    }
    public void FixedUpdate(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        player.body.Movement.y -= player.GravityPower * Time.deltaTime;
        player.body.TargetMovement.y = player.body.Movement.y;
        player.transform.position += new Vector3(player.body.Movement.x * Time.deltaTime, player.body.Movement.y * Time.deltaTime);
        player.Sprite.transform.Rotate(new Vector3(0, 0, 180 * Time.deltaTime));
    }
}
