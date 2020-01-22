using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public Sprite[] SpikeSprites;
    // Start is called before the first frame update
    void Start()
    {
        var s = GetComponent<SpriteRenderer>();
        if(s)
        {
            s.sprite = SpikeSprites[Random.Range(0,SpikeSprites.Length)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player)
        {
            player.Defeat();
        }
    }
}
