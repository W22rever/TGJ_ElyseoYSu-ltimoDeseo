using TMPro;
using UnityEngine;

public class PizzaTimer : MonoBehaviour
{
    public float prepTime = 4f;    // 4 segundos de preparación
    public float gameTime = 21f;   // 21 segundos de juego
    public TextMeshProUGUI timerText;
    public PizzaManager manager;
    public Animator characterAnimator;

    private bool isPrepTime = true; // Controla la fase
    private float timeRemaining;
    private bool timerIsRunning = true;

    void Start()
    {
        // Al iniciar la escena, el tiempo restante es el de preparación
        timeRemaining = prepTime;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                if (isPrepTime)
                {
                    // Durante la preparación, mostramos solo los segundos
                    timerText.text = Mathf.Ceil(timeRemaining).ToString();
                }
                else
                {
                    // Durante el juego, mostramos el formato 0:00
                    UpdateTimerDisplay(timeRemaining);
                }
            }
            else
            {
                // El contador llegó a 0, chequear fase uauauauo
                if (isPrepTime)
                {
                    // Terminó la preparación
                    isPrepTime = false;
                    timeRemaining = gameTime; // Reiniciamos el reloj a 21 segundos
                    manager.IniciarJuego();   // Le avisamos al manager que suelte las notas
                    if (characterAnimator != null)
                    {
                        characterAnimator.SetBool("isGameplay", true); // Cambia el estado del personaje
                    }
                }
                else
                {
                    // Terminó el juego normal
                    timeRemaining = 0;
                    timerIsRunning = false;
                    UpdateTimerDisplay(timeRemaining);
                    manager.EndGameBySuccess();
                }
            }
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
