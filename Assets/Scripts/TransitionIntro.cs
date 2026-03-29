using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionIntro : MonoBehaviour
{
    [Header("Referencia al diálogo")]
    public DialogoScript dialogoScript;

    [Header("Escena destino")]
    public string sceneName;

    [Header("Fade")]
    public Image curtain; 
    public float fadeDuration = 1f;

    [Header("Audio")]
    public AudioSource musicSource;

    private bool isTransitioning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dialogoScript.dialogueFinished && !isTransitioning)
        {
            StartCoroutine(FadeAndChangeScene());
        }
    }

    IEnumerator FadeAndChangeScene()
    {
        isTransitioning = true;

        float time = 0f;
        Color color = curtain.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(0, 1, time / fadeDuration);
            curtain.color = new Color(color.r, color.g, color.b, alpha);

            
            if (musicSource != null)
            {
                musicSource.volume = Mathf.Lerp(1, 0, time / fadeDuration);
            }

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}