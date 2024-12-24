using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadGameButton : MonoBehaviour
{
    public Image blackground; 
    public TextMeshProUGUI fadeText; 
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ParticleSystem particle3;
    public float fadeDuration = 2f; 
    public float textDelay = 2f; 
    public float textFadeDuration = 1f; 
    public Button skipbutton; 

    public void LoadMainGame()
    {
        StartCoroutine(FadeBackgroundAndText("MainScene"));
    }

    private IEnumerator FadeBackgroundAndText(string sceneName)
    {
        blackground.color = new Color(0, 0, 0, 0); 
        blackground.gameObject.SetActive(true);

        float elapsedTime = 0f;
        Color startColor = blackground.color;
        Color targetColor = Color.black; 
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            blackground.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        blackground.color = targetColor;

        particle1.gameObject.SetActive(false);
        particle2.gameObject.SetActive(false);
        particle3.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f); 

        elapsedTime = 0f;
        targetColor = new Color(0.5f, 0.5f, 0.5f, 1);
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            blackground.color = Color.Lerp(Color.black, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        blackground.color = targetColor;



        yield return new WaitForSeconds(textDelay);

        if (fadeText != null)
        {
            fadeText.gameObject.SetActive(true);
            Color textColor = fadeText.color;
            textColor.a = 0; 
            fadeText.color = textColor;

            elapsedTime = 0f;
            while (elapsedTime < textFadeDuration)
            {
                elapsedTime += Time.deltaTime;
                textColor.a = Mathf.Clamp01(elapsedTime / textFadeDuration);
                fadeText.color = textColor;
                yield return null;
            }

            textColor.a = 1;
            fadeText.color = textColor;
        }

        skipbutton.gameObject.SetActive(true);

        yield return new WaitForSeconds(30f);

        SceneManager.LoadScene(sceneName);
    }
}
