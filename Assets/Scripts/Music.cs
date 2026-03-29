using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic; 

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (backgroundMusic)
        {
            _audioSource.clip = backgroundMusic;
            _audioSource.loop = true;  
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se ha asignado música para esta escena: " + gameObject.name);
        }
    }
}