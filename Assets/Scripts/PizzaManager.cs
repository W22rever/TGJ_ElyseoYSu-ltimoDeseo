using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class PizzaManager : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform[] spawnPoints;

    public GameObject prefabVictoria;
    public GameObject prefabDerrota;
    public Transform canvasTransform;

    public int puntajeTotal = 0;
    public EfectoPuntaje popupPuntaje;

    public ControladorPizza controladorPizza;
    public Animator characterAnimator;

    private int totalNotasGeneradas = 0;
    private int correctNotesPressed = 0;
    private bool turnoDerechaActual = true;

    public GameObject provechoTextObject;
    public GameObject finishTextObject;

    private bool isGameOver = false;
    private bool isSuccess = false;

    void Start()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetFloat("TurnoDerecha", 1f);
        }
    }

    public void IniciarJuego()
    {
        if (provechoTextObject != null)
        {
            provechoTextObject.SetActive(true);
            Invoke(nameof(OcultarTextoProvecho), 1f);
        }

        InvokeRepeating(nameof(SpawnNote), 1f, 1f);

        if (controladorPizza != null)
        {
            controladorPizza.QuitarPrimerPedazo();
        }
    }

    public void RegistrarAcierto()
    {
        if (isGameOver || isSuccess) return;

        correctNotesPressed++; // Sumamos un acierto
        puntajeTotal += 10;    // Sumamos al puntaje

        if (popupPuntaje != null)
        {
            popupPuntaje.Mostrar(10);
        }

        if (controladorPizza != null && !controladorPizza.esperandoPizza)
        {
            PersonajeComer();
        }
    }

    void SpawnNote()
    {
        if (isGameOver || isSuccess) return;
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawn = spawnPoints[randomSpawnIndex];
        GameObject newNote = Instantiate(notePrefab, selectedSpawn.position, Quaternion.identity);

        totalNotasGeneradas++; // Contamos cada nota que aparece

        Note noteScript = newNote.GetComponent<Note>();
        int randomLetterValue = Random.Range((int)KeyCode.A, (int)KeyCode.Z + 1);
        noteScript.assignedKey = (KeyCode)randomLetterValue;
    }

    public void EndGameBySuccess()
    {
        if (isSuccess || isGameOver) return;
        isSuccess = true;
        Debug.Log("¡Timer terminado, ganaste!");
        MostrarTextoFinish();
        LimpiarPantalla();
        EvaluarYMostrarResultado();
    }

    public void EndGameByFailure()
    {
        if (isGameOver || isSuccess) return;
        isGameOver = true;
        Debug.Log("¡Game Over, perdiste!");
        MostrarTextoFinish();
        LimpiarPantalla();
        EvaluarYMostrarResultado(); 
    }

    private void MostrarTextoFinish()
    {
        if (finishTextObject != null)
        {
            finishTextObject.SetActive(true);
        }
    }

    private void OcultarTextoProvecho()
    {
        if (provechoTextObject != null)
        {
            provechoTextObject.SetActive(false);
        }
    }

    private void PersonajeComer()
    {
        if (characterAnimator != null)
        {
            float valorLado = turnoDerechaActual ? 1f : 0f;
            characterAnimator.SetFloat("TurnoDerecha", valorLado);
            characterAnimator.SetTrigger("ComerTrigger");

            turnoDerechaActual = !turnoDerechaActual;

            Invoke(nameof(ActualizarLadoIdle), 0.5f);
        }
    }

    private void ActualizarLadoIdle()
    {
        if (characterAnimator != null)
        {
            float nuevoLado = turnoDerechaActual ? 1f : 0f;
            characterAnimator.SetFloat("TurnoDerecha", nuevoLado);
        }

        if (controladorPizza != null)
        {
            controladorPizza.ComerPedazo();
        }
    }

    private void LimpiarPantalla()
    {
        CancelInvoke(nameof(SpawnNote));

        Note[] notasActivas = FindObjectsOfType<Note>();
        foreach (Note nota in notasActivas)
        {
            Destroy(nota.gameObject);
        }
    }

    public void EvaluarYMostrarResultado()
    {
        GameObject prefabSeleccionado;
        int fallos = totalNotasGeneradas - correctNotesPressed;

        // Evaluar la condición 50% aciertos
        float porcentaje = (float)correctNotesPressed / totalNotasGeneradas;

        if (porcentaje >= 0.5f)
        {
            prefabSeleccionado = prefabVictoria;
        }
        else
        {
            prefabSeleccionado = prefabDerrota;
        }

        // Instanciar el prefab seleccionado dentro del Canvas
        Debug.Log("Intentando instanciar pantalla...");
        GameObject panelInstanciado = Instantiate(prefabSeleccionado, canvasTransform);
        panelInstanciado.SetActive(true);

        // Buscar el script en el objeto recién creado y poblar los textos
        PanelResultados scriptPanel = panelInstanciado.GetComponent<PanelResultados>();

        if (scriptPanel != null)
        {
            scriptPanel.ConfigurarTextos(fallos, correctNotesPressed, puntajeTotal);
        }
        else
        {
            Debug.LogWarning("El prefab instanciado no tiene el script PanelResultados adjunto.");
        }
    }
}