using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteClamp : MonoBehaviour
{
    private SpriteRenderer sprite;
    private static float pixelperUnity = 32;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 parentpos = transform.parent.transform.position;
        float aspect = (1 / pixelperUnity);
        parentpos.x = Mathf.Round(parentpos.x / aspect) * aspect;
        parentpos.y = Mathf.Round(parentpos.y / aspect) * aspect;
        transform.position = (Vector3)parentpos;
    }
}
