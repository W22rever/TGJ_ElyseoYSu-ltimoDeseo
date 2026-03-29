using System.Collections;
using UnityEngine;
using TMPro; 

public class EfectoPuntaje : MonoBehaviour
{
    // Animacion
    public float distanciaSubida = 50f;
    public float duracion = 0.8f;

    private TextMeshProUGUI textoUI;
    private RectTransform rectTransform;
    private Vector2 posicionOriginal;
    private Coroutine animacionActual;

    void Start()
    {
        textoUI = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        
        posicionOriginal = rectTransform.anchoredPosition;

        //Color colorInicio = textoUI.color;
        //colorInicio.a = 0f;
        //textoUI.color = colorInicio;
    }

    public void Mostrar(int puntos)
    {
        textoUI.text = "+" + puntos;

        // Si el jugador acierta muy rápido, reiniciamos la animación sin que se rompa
        if (animacionActual != null) StopCoroutine(animacionActual);

        animacionActual = StartCoroutine(Animar());
    }

    private IEnumerator Animar()
    {
        // Lo devolvemos a su lugar original y lo hacemos 100% visible
        rectTransform.anchoredPosition = posicionOriginal;
        Color colorActual = textoUI.color;
        colorActual.a = 1f;
        textoUI.color = colorActual;

        float tiempo = 0f;

        // Animamos la subida y la transparencia al mismo tiempo
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float porcentaje = tiempo / duracion;

            // Mueve el texto hacia arriba poco a poco
            rectTransform.anchoredPosition = Vector2.Lerp(posicionOriginal, posicionOriginal + (Vector2.up * distanciaSubida), porcentaje);

            // Baja el Alpha (transparencia) de 1 a 0 poco a poco
            colorActual.a = Mathf.Lerp(1f, 0f, porcentaje);
            textoUI.color = colorActual;

            yield return null;
        }
    }
}