using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargeOrb : MonoBehaviour
{
    public float CooldownDuration = 5f;
    public AudioClipGroup AudioGet;
    private float cooldown;
    private bool available;
    private Animator animator;

    // Start is called before the first frame update
    private void Awake()
    {
        LevelEvents.OnPlayerRespawn += OnPlayerRespawn;
    }

    private void OnDestroy()
    {
        LevelEvents.OnPlayerRespawn -= OnPlayerRespawn;
    }
    void Start()
    {
        cooldown = 0;
        animator = gameObject.GetComponent<Animator>();
        available = true;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = Mathf.Max(cooldown - Time.deltaTime, 0);
        if (cooldown == 0)
        {
            available = true;
        }
        animator.SetBool("Available", available);
    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player && available)
        {
            if (!player.orb && !player.IsOrbAvailable)
            {
                player.IsOrbAvailable = true;
                available = false;
                cooldown = CooldownDuration;
                AudioGet?.Play();
            }
        }
    }

    void OnPlayerRespawn()
    {
        cooldown = 0;
        available = true;
    }
}
