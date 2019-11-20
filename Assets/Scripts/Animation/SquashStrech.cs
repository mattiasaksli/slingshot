using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashStrech : MonoBehaviour
{
    private Bounds defaultBounds;
    private Bounds currentBounds;
    private SpriteRenderer sprite;

    private float reformSpeed;

    public int StickHorizontal = 0;
    public int StickVertical = 0;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        defaultBounds = sprite.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        float morphAmount = 1 - transform.localScale.x;
        transform.localScale += new Vector3(Mathf.Min(reformSpeed*Time.deltaTime, Mathf.Abs(morphAmount))* Mathf.Sign(morphAmount), 0,0);
        transform.localScale = new Vector3(transform.localScale.x,2 - transform.localScale.x, 1);
        currentBounds = sprite.bounds;
        transform.localPosition = new Vector3((defaultBounds.size.x - currentBounds.size.x) / 2* StickHorizontal, (defaultBounds.size.y - currentBounds.size.y)/2 * StickVertical, 0);
    }

    public void ApplyMorph(float ScaleX, float reformSpeed, int StickHorizontal, int StickVertical)
    {
        this.reformSpeed = reformSpeed;
        transform.localScale = new Vector3(Mathf.Clamp(ScaleX,0,2), 2 - transform.localScale.x, 1);
        this.StickHorizontal = StickHorizontal;
        this.StickVertical = StickVertical;
    }
}
