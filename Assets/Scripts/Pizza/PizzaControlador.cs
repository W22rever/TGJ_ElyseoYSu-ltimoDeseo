using System.Collections;
using UnityEngine;

public class ControladorPizza : MonoBehaviour
{
    // Sprites de estados de la pizza
    public Sprite[] spritesPizza;

    // Movimiento de la pizza
    public Transform puntoPlato; // Posicion de la Pizza
    public Transform puntoInicioDerecha; // Desde dónde aparece la pizza nueva
    public float velocidadDeslizamiento = 15f;

    // Personaje
    public Animator animPersonaje;

    private SpriteRenderer spriteRenderer;
    private int trozoActual = 0;
    public bool esperandoPizza = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = puntoPlato.position;
        trozoActual = 0;
        if (spritesPizza.Length > 0)
        {
            spriteRenderer.sprite = spritesPizza[0]; 
        }
    }

    public void QuitarPrimerPedazo()
    {
        if (esperandoPizza) return;

        trozoActual = 1; // Pasamos lógicamente al pedazo 1
        if (spritesPizza.Length > 1)
        {
            spriteRenderer.sprite = spritesPizza[1]; // Mostramos la pizza incompleta
        }
    }

    // Pizza Manager lo referencia cada que acierta
    public void ComerPedazo()
    {
        if (esperandoPizza) return; // No hacer nada si la pizza está viajando

        trozoActual++;

        // Si aún quedan pedazos:
        if (trozoActual < spritesPizza.Length)
        {
            spriteRenderer.sprite = spritesPizza[trozoActual];
        }
        if (trozoActual >= spritesPizza.Length - 1)
        {
            StartCoroutine(TraerNuevaPizza());
        }
    }

    private IEnumerator TraerNuevaPizza()
    {
        esperandoPizza = true;
        yield return new WaitForSeconds(0.3f);

        // Personaje vuelve a su estado de preparación
        animPersonaje.SetBool("Esperando", true);

        // Escondemos la pizza y la movemos fuera de la pantalla a la derecha
        spriteRenderer.sprite = null;
        transform.position = puntoInicioDerecha.position;

        // Reseteamos la pizza a entera
        trozoActual = 0;
        spriteRenderer.sprite = spritesPizza[0];

        while (Vector3.Distance(transform.position, puntoPlato.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoPlato.position, velocidadDeslizamiento * Time.deltaTime);
            yield return null;
        }

        transform.position = puntoPlato.position;

        animPersonaje.SetBool("Esperando", false);

        esperandoPizza = false;
    }
}