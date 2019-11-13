using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public static CameraScroll Instance;

    public Transform PlayerTransform;
    public Transform BorderLeftDown;
    public Transform BorderRightUp;
    public RoomBounds ActiveRoom;

    void Start()
    {
        Instance = this;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ChangeRoom(ActiveRoom);
    }

    void Update()
    {
        if (PlayerTransform)
        {
            float horizontalPadding = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0)).x - Camera.main.ViewportToWorldPoint(new Vector3(0, 0)).x;
            float verticalPadding = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 5f)).y - Camera.main.ViewportToWorldPoint(new Vector3(0, 0)).y;

            float targetX = Mathf.Clamp(PlayerTransform.position.x, BorderLeftDown.position.x + horizontalPadding, BorderRightUp.position.x - horizontalPadding);
            float targetY = Mathf.Clamp(PlayerTransform.position.y, BorderLeftDown.position.y + verticalPadding, BorderRightUp.position.y - verticalPadding);

            transform.position = new Vector3(targetX, targetY, -10);
        }
    }

    public void ChangeRoom(RoomBounds newRoom)
    {
        // Sets camera bounds in the new room.
        ActiveRoom = newRoom;
        BorderLeftDown = ActiveRoom.LeftDown.transform;
        BorderRightUp = ActiveRoom.RightUp.transform;
    }
}
