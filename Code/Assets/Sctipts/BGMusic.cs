using UnityEngine;

public class BGMusic : MonoBehaviour
{
    private static BGMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern to make sure only one exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Makes it persist across scenes
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }
}
