using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerDead : State
{

    public void Update(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        if (Time.time > player.DeathTime && player.Sprite.enabled)
        {
            player.fade.animation.clip = player.fade.animation.GetClip("FadeOut");
            player.fade.animation.Play();
            Debug.Log("Animation");
        }

        if(Time.time > player.DeathTime-1.0f && player.Sprite.color != Color.clear)
        {
            player.DefeatParticle.Play();
            player.Sprite.color = Color.clear;
        }
    }
    public void FixedUpdate(MonoBehaviour controller)
    {
        PlayerController player = (PlayerController)controller;
        player.body.Movement.y -= player.GravityPower * Time.deltaTime;
        player.body.TargetMovement.y = player.body.Movement.y;
        player.body.Movement = Vector2.zero;
        player.transform.position += new Vector3(player.body.Movement.x * Time.deltaTime, player.body.Movement.y * Time.deltaTime);
        player.Sprite.transform.Rotate(new Vector3(0, 0, 180 * Time.deltaTime));
    }
}
