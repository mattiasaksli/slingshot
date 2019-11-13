using UnityEngine;

public class NextRoomTrigger : MonoBehaviour
{
    public RoomBounds RoomBounds;

    private void Start()
    {
        RoomBounds = GetComponentInParent<RoomBounds>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RoomBounds.NextRoomTrigger();
        }
    }
}
