using UnityEngine;

public class HitZone : MonoBehaviour
{
    private Note activeNote;
    private PizzaManager gameManager;

    [System.Obsolete]
    void Start()
    {
        // Buscamos automáticamente
        gameManager = FindObjectOfType<PizzaManager>();
    }

    void Update()
    {
        if (activeNote != null && Input.anyKeyDown)
        {
            if (Input.GetKeyDown(activeNote.assignedKey))
            {
                if (gameManager != null)
                {
                    gameManager.RegistrarAcierto();
                }

                Destroy(activeNote.gameObject);
                activeNote = null;
            }
            else
            {
                Debug.Log("¡Tecla equivocada!");
            }
        }
    }

    // Cuando la nota entra a la línea
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            activeNote = collision.GetComponent<Note>();
        }
    }
}