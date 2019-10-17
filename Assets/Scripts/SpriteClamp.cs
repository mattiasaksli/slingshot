using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteClamp : MonoBehaviour
{
    private SpriteRenderer sprite;
    public float pixelperUnit = 1;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 parentpos = transform.parent.transform.position;
        parentpos.x = Mathf.Round(parentpos.x / pixelperUnit) * pixelperUnit;
        parentpos.y = Mathf.Round(parentpos.y / pixelperUnit) * pixelperUnit;
        transform.position = parentpos;
    }
}
