using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RoomFollower : MonoBehaviour
{
    public float TargetZoom;
    private float zoom;

    private Transform player;
    private GameObject room;
    private Vector3 TargetPosition;

    private Vector2 CameraSize;
    private Camera camera;
    private PixelPerfectCamera pixelperfect;

    private void Start()
    {
        camera = gameObject.GetComponent<Camera>();
        TargetZoom = 1;
        zoom = 1;
        pixelperfect = gameObject.GetComponent<PixelPerfectCamera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float lerpAmount = 0.2f;
        TargetZoom = Mathf.Max(1, TargetZoom);
        zoom = Mathf.Lerp(zoom, TargetZoom, lerpAmount);
        camera.orthographicSize = 8.4375f * 1/zoom;
        UpdateCameraSize();
        room = GameObject.Find("RoomBounds");
        player = GameObject.Find("Player").transform;
        if(player && room)
        {
            Bounds bounds = room.GetComponent<BoxCollider2D>().bounds;
            TargetPosition.x = Mathf.Clamp(player.position.x, bounds.min.x + CameraSize.x, bounds.max.x - CameraSize.x);
            TargetPosition.y = Mathf.Clamp(player.position.y, bounds.min.y + CameraSize.y, bounds.max.y - CameraSize.y);
            TargetPosition.z = transform.position.z;
        }
        if(!room)
        {
            player.GetComponent<PlayerController>().Defeat();
            LevelController.Instance.SpawnPoint.GetComponentInParent<RoomBoundsManager>().RoomCollider.gameObject.SetActive(true);
        }

        transform.position = Vector3.Lerp(transform.position, TargetPosition, lerpAmount);
    }

    void UpdateCameraSize()
    {
        CameraSize.x = camera.orthographicSize * camera.aspect;
        CameraSize.y = camera.orthographicSize;
    }
}
