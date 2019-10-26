using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public PlayerController Player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UI.Instance.Win = true;
            UI.Instance.ShowEndGamePanel();
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            Player.IsInputLocked = true;
        }
    }

}
