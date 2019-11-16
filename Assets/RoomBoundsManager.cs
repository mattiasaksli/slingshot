using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoomBoundsSize))]
public class RoomBoundsManager : MonoBehaviour
{
    public BoxCollider2D RoomCollider;
    private Transform player;
    private BoxCollider2D collider;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        collider = gameObject.GetComponentInChildren<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Bounds bounds = collider.bounds;
        if(player.position.x < bounds.min.x || player.position.y < bounds.min.y || player.position.x > bounds.max.x || player.position.y > bounds.max.y)
        {
            RoomCollider.gameObject.SetActive(false);
        } else
        {
            if (!RoomCollider.gameObject.activeSelf)
            {
                RoomCollider.gameObject.SetActive(true);
                LevelEvents.ChangeRoom(this);
            }
        }
    }
}
