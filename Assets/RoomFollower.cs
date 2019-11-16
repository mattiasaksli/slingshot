using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFollower : MonoBehaviour
{
    private Transform player;
    private GameObject room;
    private Vector3 TargetPosition;

    private Vector2 CameraSize;
    private Camera camera;

    private void Start()
    {
        camera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraSize();
        room = GameObject.Find("RoomBounds");
        player = GameObject.Find("Player").transform;
        float lerpAmount = 0.2f;
        if(player && room)
        {
            Bounds bounds = room.GetComponent<BoxCollider2D>().bounds;
            TargetPosition.x = Mathf.Clamp(player.position.x, bounds.min.x + CameraSize.x, bounds.max.x - CameraSize.x);
            TargetPosition.y = Mathf.Clamp(player.position.y, bounds.min.y + CameraSize.y, bounds.max.y - CameraSize.y);
            TargetPosition.z = transform.position.z;
        }

        transform.position = Vector3.Lerp(transform.position, TargetPosition, lerpAmount);
    }

    void UpdateCameraSize()
    {
        CameraSize.x = camera.orthographicSize * camera.aspect;
        CameraSize.y = camera.orthographicSize;
    }
}
