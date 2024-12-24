using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlackFadeIn : MonoBehaviour
{
    public Image fadeImage;  // Reference to the Image component
    public float fadeDuration = 2f;  // Duration for the fade-in effect (2 seconds)

    void Start()
    {
        // Start the fade-in when the scene starts
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // Make sure the image is fully black at the start
        fadeImage.color = new Color(0, 0, 0, 1);  // Fully black (opaque)

        // Fade out the image by reducing the alpha over time
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);  // Interpolate alpha from 1 to 0
            fadeImage.color = new Color(0, 0, 0, alpha);  // Update the alpha of the black color
            elapsedTime += Time.deltaTime;  // Increment the elapsed time
            yield return null;  // Wait for the next frame
        }

        // Ensure it's fully transparent at the end
        fadeImage.color = new Color(0, 0, 0, 0);  // Fully transparent

        fadeImage.gameObject.SetActive(false);

    }
}
