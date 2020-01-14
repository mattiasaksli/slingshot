using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbFollower : MonoBehaviour
{
    public Transform Orb;
    public Vector3 TargetPosition;
    public bool Available;
    public float LerpSpeed;
    private float currentLerpSpeed;

    private PlayerController player;
    private SpriteRenderer sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<PlayerController>();
        sprite = Orb.gameObject.GetComponent<SpriteRenderer>();
        Orb.transform.parent = null;
        currentLerpSpeed = LerpSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Orb.position = Vector3.Lerp(Orb.position, TargetPosition, currentLerpSpeed * Time.deltaTime);
        if(player.orb)
        {
            TargetPosition = player.orb.transform.position;
            currentLerpSpeed += 30.0f * Time.deltaTime;
        } else
        {
            TargetPosition = player.transform.position + new Vector3((player.IsFacingRight ? -1 : 1),0,0);
            currentLerpSpeed = LerpSpeed;
        }
        sprite.enabled = player.IsOrbAvailable;
    }
}
