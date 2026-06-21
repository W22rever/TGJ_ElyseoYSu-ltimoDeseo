using UnityEngine;
using TMPro; 

public class PanelResultados : MonoBehaviour
{
    public TextMeshProUGUI Puntaje;
    public TextMeshProUGUI Aciertos;
    public TextMeshProUGUI Fallos;
    
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