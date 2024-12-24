using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadGameButton : MonoBehaviour
{
    public Image blackground; // Reference to the fade-to-black background image
    public TextMeshProUGUI fadeText; // Reference to the text that fades in
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ParticleSystem particle3;
    public float fadeDuration = 2f; // Duration of the background fade effect
    public float textDelay = 2f; // Delay before showing the text
    public float textFadeDuration = 1f; // Duration of the text fade-in effect
    public Button skipbutton; 

    // This method will be called when the button is clicked
    public void LoadMainGame()
    {
        StartCoroutine(FadeBackgroundAndText("MainScene"));
    }

    private IEnumerator FadeBackgroundAndText(string sceneName)
    {
        // Ensure the background starts fully transparent
        blackground.color = new Color(0, 0, 0, 0); // Fully transparent
        blackground.gameObject.SetActive(true);

        // Fade the background to fully black
        float elapsedTime = 0f;
        Color startColor = blackground.color;
        Color targetColor = Color.black; // Fully black
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            blackground.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure the background is fully black
        blackground.color = targetColor;

        // Disable particles
        particle1.gameObject.SetActive(false);
        particle2.gameObject.SetActive(false);
        particle3.gameObject.SetActive(false);

        // Wait before transitioning to light gray
        yield return new WaitForSeconds(2f); // Optional pause before changing to gray

        // Fade the background from black to light gray
        elapsedTime = 0f;
        targetColor = new Color(0.5f, 0.5f, 0.5f, 1); // Light gray
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            blackground.color = Color.Lerp(Color.black, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure the background is light gray
        blackground.color = targetColor;



        // Wait for the specified delay before showing the text
        yield return new WaitForSeconds(textDelay);

        // Gradually fade in the text
        if (fadeText != null)
        {
            fadeText.gameObject.SetActive(true);
            Color textColor = fadeText.color;
            textColor.a = 0; // Start text as fully transparent
            fadeText.color = textColor;

            elapsedTime = 0f;
            while (elapsedTime < textFadeDuration)
            {
                elapsedTime += Time.deltaTime;
                textColor.a = Mathf.Clamp01(elapsedTime / textFadeDuration);
                fadeText.color = textColor;
                yield return null;
            }

            // Ensure the text is fully visible
            textColor.a = 1;
            fadeText.color = textColor;
        }

        skipbutton.gameObject.SetActive(true);

        // Optional: Add a delay before loading the new scene
        yield return new WaitForSeconds(30f);

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }
}
