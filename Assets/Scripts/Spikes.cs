using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(collision.gameObject);
            UI.Instance.Win = false;
            UI.Instance.ShowEndGamePanel();
        }
    }
}
