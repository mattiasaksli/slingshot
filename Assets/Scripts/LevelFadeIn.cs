using UnityEngine;

public class LevelFadeIn : MonoBehaviour
{
    public UIFade FadePanel;

    private void Start()
    {
        PlayerController Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Player.IsInputLocked = true;
        FadeIn();
        Player.IsInputLocked = false;
        Destroy(this);
    }

    private void FadeIn()
    {
        UIFade panel = Instantiate(FadePanel, transform);
        panel.AddCallback(() => Destroy(panel.gameObject));
        panel.FadeIn(true);
    }

}
