using UnityEngine;
using UnityEngine.SceneManagement;

public class skipButton : MonoBehaviour
{
    // This method is called when the button is clicked
    public void SkipToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
