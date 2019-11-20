using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    private static PlayerController player;
    public SpawnPoint SpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Awake()
    {
        LevelEvents.OnRoomChange += OnRoomChange;
        LevelEvents.OnPlayerRespawn += OnPlayerRespawn;
    }

    private void OnDestroy()
    {
        LevelEvents.OnRoomChange -= OnRoomChange;
        LevelEvents.OnPlayerRespawn -= OnPlayerRespawn;
    }

    void OnRoomChange(RoomBoundsManager Room)
    {
        if (player.state != player.states[3])
        {
            SpawnPoint = Room.GetClosestSpawnPoint();
        }
    }

    void OnPlayerRespawn()
    {
        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.transform.position = SpawnPoint.transform.position;
        player.Sprite.enabled = true;
        player.Sprite.transform.localRotation = Quaternion.Euler(0,0,0);
        player.body.Movement = new Vector2(0, 0);
        player.body.TargetMovement = new Vector2(0, 0);
        player.state = player.states[0];
        player.RecallOrb();
        player.IsOrbAvailable = true;
    }

    private void Update()
    {
        if (SpawnPoint)
        {
            Debug.DrawLine(SpawnPoint.transform.position, SpawnPoint.transform.position + new Vector3(0, 1, 0));
        }
    }
}
