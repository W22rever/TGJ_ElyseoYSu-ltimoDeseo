using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameSequenceManager : MonoBehaviour
{
    public static MinigameSequenceManager Instance;

    private List<PostItSO> minigameOrder = new List<PostItSO>();
    private int currentIndex = 0;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 👉 Agregar
    public void AddMinigame(PostItSO data)
    {
        if (!minigameOrder.Contains(data))
        {
            minigameOrder.Add(data);
            Debug.Log("Añadido: " + data.sceneName);
        }
    }

    // 👉 Remover (cuando deseleccionas)
    public void RemoveMinigame(PostItSO data)
    {
        if (minigameOrder.Contains(data))
        {
            minigameOrder.Remove(data);
            Debug.Log("Removido: " + data.sceneName);
        }
    }

    // 👉 Iniciar
    public void StartSequence()
    {
        if (minigameOrder.Count == 0)
        {
            Debug.LogWarning("No hay minijuegos seleccionados");
            return;
        }

        currentIndex = 0;
        LoadCurrentMinigame();
    }

    private void LoadCurrentMinigame()
    {
        SceneManager.LoadScene(minigameOrder[currentIndex].sceneName);
    }

    // 👉 Llamar al terminar un minijuego
    public void CompleteMinigame()
    {
        currentIndex++;

        if (currentIndex < minigameOrder.Count)
        {
            LoadCurrentMinigame();
        }
        else
        {
            Debug.Log("SECUENCIA COMPLETADA 🎉");
            // Aquí puedes volver al menú
        }
    }

    public void ClearSequence()
    {
        minigameOrder.Clear();
        currentIndex = 0;
    }
}