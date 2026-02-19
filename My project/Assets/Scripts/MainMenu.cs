using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Image spitImage;
    public AudioClip spitClip;
    public AudioSource spitSource;

    public float transitionDuration = 0.6f;

    private bool isTransitioning = false;

    public void PlayGame()
    {
        if (isTransitioning) return; // Prevent button spam
        isTransitioning = true;

        // Enable the image when player clicks
        spitImage.gameObject.SetActive(true);

        StartCoroutine(SpitTransition());
    }

    IEnumerator SpitTransition()
    {
        Vector3 startScale = Vector3.one * 10f;
        Vector3 endScale = Vector3.one * 2f;

        float time = 0f;

        spitImage.transform.localScale = startScale;

        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            float t = time / transitionDuration;

            spitImage.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        spitImage.transform.localScale = endScale;

        // Play sound AFTER reaching scale 2
        spitSource.PlayOneShot(spitClip);

        // Small delay so sound is not cut off immediately
        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
