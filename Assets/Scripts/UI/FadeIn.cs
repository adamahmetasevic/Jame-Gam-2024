using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlackFadeIn : MonoBehaviour
{
    public Image fadeImage;  
    public float fadeDuration = 2f;  

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        fadeImage.color = new Color(0, 0, 0, 1);  

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);  
            fadeImage.color = new Color(0, 0, 0, alpha);  
            elapsedTime += Time.deltaTime; 
            yield return null;  
        }

        fadeImage.color = new Color(0, 0, 0, 0);  

        fadeImage.gameObject.SetActive(false);

    }
}
