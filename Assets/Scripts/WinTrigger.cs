using Doozy.Engine;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameEventMessage.SendEvent("GoToMainMenu");
        }
    }
}
