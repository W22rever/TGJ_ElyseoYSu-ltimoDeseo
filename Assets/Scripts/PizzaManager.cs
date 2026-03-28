using UnityEngine;

public class PizzaManager : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform[] spawnPoints;

    public ControladorPizza controladorPizza;
    public Animator characterAnimator;

    private bool turnoDerechaActual = true;
    private int correctNotesPressed = 0;

    public GameObject provechoTextObject;

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
        correctNotesPressed++;

        // Ahora el personaje come por cada acierto.
        PersonajeComer();
    }

    void SpawnNote()
    {
        if (isGameOver || isSuccess) return;
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawn = spawnPoints[randomSpawnIndex];
        GameObject newNote = Instantiate(notePrefab, selectedSpawn.position, Quaternion.identity);

        Note noteScript = newNote.GetComponent<Note>();
        int randomLetterValue = Random.Range((int)KeyCode.A, (int)KeyCode.Z + 1);
        noteScript.assignedKey = (KeyCode)randomLetterValue;
    }

    [System.Obsolete]
    public void EndGameBySuccess()
    {
        if (isSuccess || isGameOver) return;
        isSuccess = true;
        Debug.Log("¡Timer terminado, ganaste!");
        MostrarTextoFinish();
        LimpiarPantalla();
    }

    [System.Obsolete]
    public void EndGameByFailure()
    {
        if (isGameOver || isSuccess) return;
        isGameOver = true;
        Debug.Log("¡Game Over, perdiste!");
        MostrarTextoFinish();
        LimpiarPantalla();
    }

    public GameObject finishTextObject;
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

        // Llamamos a la función de la pizza
        if (controladorPizza != null)
        {
            controladorPizza.ComerPedazo();
        }
    }

    [System.Obsolete]
    private void LimpiarPantalla()
    {
        // Apagamos el reloj generador por completo por seguridad
        CancelInvoke(nameof(SpawnNote));

        // Buscamos todas las notas que existan en la escena en este momento
        Note[] notasActivas = FindObjectsOfType<Note>();

        // Las destruimos una por una
        foreach (Note nota in notasActivas)
        {
            Destroy(nota.gameObject);
        }
    }
}