using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoomBoundsSize))]
public class RoomBoundsManager : MonoBehaviour
{
    public BoxCollider2D RoomCollider;
    private Transform player;
    [HideInInspector]
    public BoxCollider2D collider;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        collider = gameObject.GetComponentInChildren<BoxCollider2D>();
        RoomCollider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Bounds bounds = collider.bounds;
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller.state != controller.states[3])
        {
            if (player.position.x < bounds.min.x || player.position.y < bounds.min.y || player.position.x > bounds.max.x || player.position.y > bounds.max.y)
            {
                if (RoomCollider.gameObject.activeSelf)
                {
                    RoomCollider.gameObject.SetActive(false);
                }
            }
            else
            {
                if (!RoomCollider.gameObject.activeSelf)
                {
                    RoomCollider.gameObject.SetActive(true);
                    LevelEvents.ChangeRoom(this);
                }
            }
        }

    }

    public SpawnPoint GetClosestSpawnPoint()
    {
        SpawnPoint[] spawnPoints = RoomCollider.gameObject.GetComponentsInChildren<SpawnPoint>();
        SpawnPoint closest = null;
        if (spawnPoints.Length > 0)
        {
            closest = spawnPoints[0];
            float c = Vector3.Distance(closest.transform.position, player.position);
            foreach (SpawnPoint spawn in spawnPoints)
            {
                float s = Vector3.Distance(spawn.transform.position, player.position);
                if (s < c)
                {
                    closest = spawn;
                    c = s;
                }
            }
        }
        return closest;
    }

    private void OnDrawGizmos()
    {
        Bounds bounds = gameObject.GetComponent<BoxCollider2D>().bounds;
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, 0), new Vector3(bounds.max.x, bounds.min.y, 0));
        Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y, 0), new Vector3(bounds.max.x, bounds.max.y, 0));
        Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.max.y, 0), new Vector3(bounds.min.x, bounds.max.y, 0));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y, 0), new Vector3(bounds.min.x, bounds.min.y, 0));
    }
}
