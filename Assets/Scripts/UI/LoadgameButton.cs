using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameButton : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void LoadMainGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
