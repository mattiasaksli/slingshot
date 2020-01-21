using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomManager : MonoBehaviour
{
    public event Action OnRoomStart;
    public  void StartRoom() => OnRoomStart?.Invoke();
}
