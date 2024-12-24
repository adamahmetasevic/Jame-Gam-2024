using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;  // Reference to the AudioSource component
    public float delayTime = 2f;     // Time delay before starting the music

    void Start()
    {
        // Start the coroutine to wait and then play the music
        StartCoroutine(PlayMusicAfterDelay());
    }

    IEnumerator PlayMusicAfterDelay()
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delayTime);

        // Start the music (play it)
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
