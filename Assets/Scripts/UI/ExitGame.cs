using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void ExitGame()
    {
#if UNITY_EDITOR
        // If running in the editor, stop the play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running in a build, quit the game
        Application.Quit();
#endif
    }
}