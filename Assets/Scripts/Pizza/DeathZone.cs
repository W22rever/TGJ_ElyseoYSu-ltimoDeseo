using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Este método se activa en cuanto una nota toca la zona de muerte
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si lo que entró tiene el Tag "Note"
        if (collision.CompareTag("Note"))
        {
            Debug.Log("¡Fallaste!");

            // Destruimos la nota para liberar memoria
            Destroy(collision.gameObject);

            // Más adelante, aquí le avisaremos al PizzaManager que reste una vida
        }
    }
}