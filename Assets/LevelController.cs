using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    SpawnPoint SpawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    private void Awake()
    {
        LevelEvents.OnRoomChange += OnRoomChange;
    }

    private void OnDestroy()
    {
        LevelEvents.OnRoomChange -= OnRoomChange;
    }

    void OnRoomChange(RoomBoundsManager roomCollider)
    {
        Debug.Log("Room changed to: " + roomCollider.ToString());
    }
}
