using UnityEngine;
using TMPro; // Necesario para TextMeshPro

public class PanelResultados : MonoBehaviour
{
    // Arrastra los textos de tu prefab a estos huecos en el Inspector
    public TextMeshProUGUI Puntaje;
    public TextMeshProUGUI Aciertos;
    public TextMeshProUGUI Fallos;

    // Método público para recibir los datos
    public void ConfigurarTextos(int fallos, int aciertos, int total)
    {
        Aciertos.text = aciertos.ToString();
        Fallos.text = fallos.ToString();
        Puntaje.text = total.ToString();
    }

    public void ConfigurarTextos(string fallos, string aciertos, int total)
    {
        Aciertos.text = aciertos;
        Fallos.text = fallos;
        Puntaje.text = total.ToString();
    }
}