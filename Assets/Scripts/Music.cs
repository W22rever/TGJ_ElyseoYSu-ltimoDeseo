using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    public AudioClip backgroundMusic; 

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;  
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se ha asignado música para esta escena: " + gameObject.name);
        }
    }
}