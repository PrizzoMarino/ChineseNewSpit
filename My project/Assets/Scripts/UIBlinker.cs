using UnityEngine;
using TMPro;
using System.Collections;

public class BlinkText : MonoBehaviour
{
    public TMP_Text uiText;
    public float blinkInterval = 0.1f;
    public float totalDuration = 5f;

    void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        float timer = 0f;

        while (timer < totalDuration)
        {
            uiText.enabled = !uiText.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // Ensure it stays hidden after 5 seconds
        uiText.enabled = false;
    }
}
