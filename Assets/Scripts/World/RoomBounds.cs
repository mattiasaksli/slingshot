using UnityEngine;

public class RoomBounds : MonoBehaviour
{
    public RoomBounds PreviousRoom;
    public RoomBounds NextRoom;
    public GameObject LeftDown;
    public GameObject RightUp;

    private Transform PlayerTransform;
    private KinematicBody PlayerBody;
    private PlayerController PlayerController;

    public NextRoomTrigger NextTrigger;
    public PreviousRoomTrigger PreviousTrigger;

    private void Start()
    {
        LeftDown.GetComponent<SpriteRenderer>().enabled = false;
        RightUp.GetComponent<SpriteRenderer>().enabled = false;

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = Player.transform;
        PlayerBody = Player.GetComponent<KinematicBody>();
        PlayerController = Player.GetComponent<PlayerController>();

        NextTrigger = GetComponentInChildren<NextRoomTrigger>();
        PreviousTrigger = GetComponentInChildren<PreviousRoomTrigger>();
    }

    public void NextRoomTrigger()
    {
        NextRoom.PreviousTrigger.gameObject.SetActive(false);
        PlayerController.IsInputLocked = true;

        while (PlayerTransform.position.x < NextRoom.LeftDown.transform.position.x + 1.5f)
        {
            PlayerBody.Move((new Vector2(1, 0)) * Time.deltaTime);
        }
        CameraScroll.Instance.ChangeRoom(NextRoom);

        PlayerController.IsInputLocked = false;
        NextRoom.PreviousTrigger.gameObject.SetActive(true);
    }

    public void PreviousRoomTrigger()
    {
        PreviousRoom.NextTrigger.gameObject.SetActive(false);
        PlayerController.IsInputLocked = true;

        while (PlayerTransform.position.x > PreviousRoom.RightUp.transform.position.x - 1.5f)
        {
            PlayerBody.Move((new Vector2(-1, 0)) * Time.deltaTime);
        }
        CameraScroll.Instance.ChangeRoom(PreviousRoom);

        PlayerController.IsInputLocked = false;
        PreviousRoom.NextTrigger.gameObject.SetActive(true);
    }
}
