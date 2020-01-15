using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    Transform player;
    SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(1,1,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        Color col = renderer.color;
        float target = 0;
        if ((player.position - transform.position).magnitude < 8)
        {
            target = 1;
        }
        col[3] = Mathf.Lerp(renderer.color.a, target, 3.0f * Time.deltaTime);
        renderer.color = col;
    }
}
