using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPad : MonoBehaviour
{
    public AudioClipGroup AudioJump;
    private Animator animator;
    public float JumpPower;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        KinematicBody body = collision.gameObject.GetComponent<KinematicBody>();
        if (body)
        {
            Vector2 target = transform.position - body.transform.position;
            target.y = Mathf.Max(0, target.y);
            body.Move(Mathf.Min(target.magnitude)*target.normalized+(Vector2)transform.up*1.5f);
            Vector2 set = (Vector2)(JumpPower * transform.up);
            if (transform.up != Vector3.down)
            {
                set.y = Mathf.Max(set.y, 4);
            }
            body.Movement = set;
            Debug.Log(body +  ": " + body.Movement);
            animator.SetTrigger("Jump");
        }
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player)
        {
            if(player.state == player.states[1])
            {
                player.state = player.states[0];
            }
        }
    }
}
