using System.Collections; // ¡Crucial para usar Corrutinas (tiempos de espera)!
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathManager : MonoBehaviour
{
    [SerializeField] private GameObject tattooTemplate;
    [Range(0, 100)] public float accuracy = 100f;

    //Reloj
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timeLimit = 60f;

    private float _currentTime;

    public bool isGameOver = false;

    //Pantallas finales
    [SerializeField] private GameObject pantallaGanadora;
    [SerializeField] private GameObject pantallaPerdedora;

    // --- SECUENCIA ---
    [SerializeField] private GameObject finishText;
    [SerializeField] private GameObject drawArea;
    [SerializeField] private LineDrawer miLapiz;

    private int _currentIndex = -1;
    private HashSet<int> _visitedPoints = new HashSet<int>();
    private List<Checkpoint> _allCheckpoints = new List<Checkpoint>();

    private void Start()
    {
        _currentTime = timeLimit;
        UpdateTimerUI();

        int autoIndex = 0;
        foreach (Transform pathFolder in transform)
        {
            foreach (Transform pointTransform in pathFolder)
            {
                Checkpoint cp = pointTransform.GetComponent<Checkpoint>();
                if (cp != null)
                {
                    cp.index = autoIndex;
                    cp.Initialize();
                    _allCheckpoints.Add(cp);
                    autoIndex++;
                }
            }
        }
        UpdateVisuals();
    }

    private void Update()
    {
        if (isGameOver) return;

        if (_currentIndex != -1)
        {
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0)
            {
                _currentTime = 0;
                EndGame();
            }

            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(_currentTime / 60F);
            int seconds = Mathf.FloorToInt(_currentTime - minutes * 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (_currentTime <= 10f) timerText.color = Color.red;
        }
    }

    public bool CheckPointReached(int index)
    {
        if (isGameOver) return false;

        int nextIndex = (_currentIndex == -1) ? index : (_currentIndex + 1);
        if (_visitedPoints.Contains(index) && index != nextIndex) return false;

        if (_currentIndex == -1)
        {
            if (index != 0) return false;

            _currentIndex = index;
            _visitedPoints.Add(index);
            UpdateVisuals();
            return true;
        }

        if (index == nextIndex)
        {
            _currentIndex = index;
            _visitedPoints.Add(index);
            UpdateVisuals();

            if (_visitedPoints.Count == _allCheckpoints.Count)
            {
                EndGame();
            }
            return true;
        }

        ReduceAccuracy(5f);
        return false;
    }

    public void ReduceAccuracy(float amount)
    {
        if (isGameOver) return;
        accuracy -= amount;
        accuracy = Mathf.Clamp(accuracy, 0f, 100f);
    }

    private void UpdateVisuals()
    {
        int targetIndex = _currentIndex + 1;
        foreach (Checkpoint cp in _allCheckpoints)
        {
            if (_visitedPoints.Contains(cp.index))
            {
                cp.SetVisible(true);
                cp.SetColor(Color.green);
                cp.SetColliderActive(false);
            }
            else if (cp.index == targetIndex)
            {
                cp.SetVisible(true);
                cp.SetColor(new Color(0.5f, 0.5f, 0.5f, 0.8f));
                cp.SetColliderActive(true);
            }
            else if (cp.index == targetIndex + 1)
            {
                cp.SetVisible(true);
                cp.SetColor(new Color(0.5f, 0.5f, 0.5f, 0.8f));
                cp.SetColliderActive(false);
            }
            else
            {
                cp.SetVisible(false);
                cp.SetColliderActive(false);
            }
        }
    }

    // --- EndGame ahora llama a la Corrutina ---
    private void EndGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        // StartCoroutine le dice a Unity que ejecute esta función especial que tiene pausas
        StartCoroutine(SecuenciaDeFinalizacion());
    }

    private IEnumerator SecuenciaDeFinalizacion()
    {
        if (tattooTemplate != null) tattooTemplate.SetActive(false);
        if (finishText != null) finishText.SetActive(true);

        yield return new WaitForSeconds(3.5f);

        if (finishText != null) finishText.SetActive(false);
        if (drawArea != null) drawArea.SetActive(false);

        if (miLapiz != null) miLapiz.ClearAllLines();

        int aciertos = _visitedPoints.Count;
        int fallos = _allCheckpoints.Count - aciertos;
        int puntaje = Mathf.RoundToInt(accuracy);

        float porcentajeAciertos = (float)aciertos / _allCheckpoints.Count;
        bool esVictoria = (porcentajeAciertos >= 0.75f) && (accuracy > 65f);

        GameObject pantallaAMostrar = esVictoria ? pantallaGanadora : pantallaPerdedora;

        if (pantallaAMostrar != null)
        {
            pantallaAMostrar.SetActive(true);
            PanelResultados scriptResultados = pantallaAMostrar.GetComponent<PanelResultados>();
            if (scriptResultados != null)
            {
                scriptResultados.ConfigurarTextos(fallos, aciertos, puntaje);
            }
        }
    }
}