﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPad : MonoBehaviour
{
    public AudioClipGroup AudioJump;
    private Animator animator;
    public float JumpPower;
    private FallingPlatform parentplatform;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        parentplatform = gameObject.GetComponentInParent<FallingPlatform>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        KinematicBody body = collision.gameObject.GetComponent<KinematicBody>();
        OrbBody orb = body.GetComponent<OrbBody>();
        PlayerController player = body.GetComponent<PlayerController>();

        if (body)
        {
            if (transform.up.y != 1)
            {
                Vector2 target = transform.position - body.transform.position;
                target.y = Mathf.Max(0, target.y);
                body.Move(target);
            }
            if(transform.up.y == 1)
            {
                Vector2 target = new Vector2(0, transform.position.y - body.transform.position.y);
                body.Move(target);
            }
            Vector2 set = (Vector2)(JumpPower * transform.up);
            if (transform.up != Vector3.down && player)
            {
                set.y = Mathf.Max(set.y, 8);
            }
            if(orb && transform.up.y != 1)
            {
                set.x *= 1.2f;
            }
            if(parentplatform)
            {
                parentplatform.Triggered();
            }
            body.Movement = set;
            animator.SetTrigger("Jump");
            AudioJump?.Play();
        }
        if(player)
        {
            if (transform.up.y != 1)
            {
                player.JumpPadTimestamp = Time.time + 0.1f;
            }
            if (player.state == player.states[1])
            {
                player.state = player.states[0];
            }
            if(!player.IsOrbAvailable && player.orb == null)
            {
                player.IsOrbAvailable = true;
            }
        }
    }
}
