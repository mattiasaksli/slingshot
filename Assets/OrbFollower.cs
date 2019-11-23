using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbFollower : MonoBehaviour
{
    public Transform Orb;
    public Vector3 TargetPosition;
    public bool Available;
    public float LerpSpeed;

    private PlayerController player;
    private SpriteRenderer sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<PlayerController>();
        sprite = Orb.gameObject.GetComponent<SpriteRenderer>();
        Orb.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        Orb.position = Vector3.Lerp(Orb.position, TargetPosition, LerpSpeed*Time.deltaTime);
        if(player.orb)
        {
            TargetPosition = player.orb.transform.position;
        } else
        {
            TargetPosition = player.transform.position + new Vector3((player.IsFacingRight ? -1 : 1),0,0);
        }
        sprite.enabled = player.IsOrbAvailable;
    }
}
