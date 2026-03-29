using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Transition : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Scene Cambio")]
    public string sceneName;

    [Header("Settings Cortina")]
    public Image Cortina;      
    public float fadeDuration = 1f; 

    [Header("Settings Sonido Latido")]
    public AudioSource fadeAudioSource; 
    public AudioClip fadeSound;

    [Header("Settings Music Background")]
    public AudioSource musicSource; 
    public float targetMusicVolume = 0.2f; 
    private float originalVolume;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            if (Cortina != null)
                StartCoroutine(FadeAndLoadScene(sceneName));
            else
                SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No se ha asignado ninguna escena en " + gameObject.name);
        }
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
     
        if (musicSource != null)
            originalVolume = musicSource.volume;

        
        if (fadeAudioSource != null && fadeSound != null)
        {
            fadeAudioSource.PlayOneShot(fadeSound);
        }

      
        Color color = Cortina.color;
        color.a = 0f;
        Cortina.color = color;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = t / fadeDuration;

            
            color.a = Mathf.Lerp(0f, 1f, progress);
            Cortina.color = color;

            if (musicSource != null)
                musicSource.volume = Mathf.Lerp(originalVolume, targetMusicVolume, progress);

            yield return null;
        }

       
        color.a = 1f;
        Cortina.color = color;

        if (musicSource != null)
            musicSource.volume = targetMusicVolume;

        
        SceneManager.LoadScene(sceneName);
    }
}