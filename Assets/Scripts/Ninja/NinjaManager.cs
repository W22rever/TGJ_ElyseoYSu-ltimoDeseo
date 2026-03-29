using UnityEngine;
using TMPro;

public class NinjaManager : MonoBehaviour
{
    //Reloj
    public float timeLimit = 21f;
    public TMP_Text timerText;


    //Pantallas Finales
    public GameObject pantallaVictoria;
    public GameObject pantallaDerrota;

    //Reglas
    public int totalTargets = 3;
    public int totalShurikens = 5;

    private float _currentTime;
    private bool _isGameOver;
    private int _targetsHit = 0;
    private int _shurikensUsed = 0;

    private void Start()
    {
        _currentTime = timeLimit;
    }

    private void Update()
    {
        if (_isGameOver) return;

        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0)
        {
            _currentTime = 0;
            EndGame();
        }
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(_currentTime);
            timerText.text = seconds.ToString();

            if (seconds <= 5) timerText.color = Color.red;
        }
    }

    public void RegisterShurikenUsed()
    {
        if (_isGameOver) return;
        _shurikensUsed++;

        if (_shurikensUsed >= totalShurikens && _targetsHit < totalTargets)
        {
            Invoke("EndGame", 1.5f);
        }
    }

    public void RegisterHit()
    {
        if (_isGameOver) return;
        _targetsHit++;

        if (_targetsHit >= totalTargets)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        int fallas = _shurikensUsed - _targetsHit;
        if (fallas < 0) fallas = 0;
        int puntaje = _targetsHit * 50;

        string textoAciertos = $"{_targetsHit} / {totalTargets}";
        string textoFallas = $"{fallas} / {totalShurikens}";

        // Ganas si le diste a todos los objetivos
        bool esVictoria = _targetsHit >= totalTargets;

        // Elegimos la pantalla correspondiente
        GameObject pantallaAMostrar = esVictoria ? pantallaVictoria : pantallaDerrota;

        if (pantallaAMostrar != null)
        {
            pantallaAMostrar.SetActive(true);
            PanelResultados panel = pantallaAMostrar.GetComponent<PanelResultados>();
            if (panel != null)
            {
                panel.ConfigurarTextos(textoFallas, textoAciertos, puntaje);
            }
        }
        else
        {
            Debug.LogError("¡Falta asignar las pantallas en el NinjaManager!");
        }
    }
}