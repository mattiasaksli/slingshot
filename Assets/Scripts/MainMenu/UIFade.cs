using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    private Image screen;
    private Action callback;

    public void FadeIn(bool b)
    {
        screen = GetComponent<Image>();
        StartCoroutine(FadeImage(b));
    }

    public void AddCallback(Action action)
    {
        callback = action;
    }

    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                screen.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                screen.color = new Color(1, 1, 1, i);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            callback?.Invoke();
            yield return null;
        }
    }
}
