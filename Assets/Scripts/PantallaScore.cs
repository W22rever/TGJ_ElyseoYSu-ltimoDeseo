using UnityEngine;
using TMPro;

public class PanelResultados : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI points;
    [SerializeField] private TextMeshProUGUI success;
    [SerializeField] private TextMeshProUGUI fails;
    
    public void ConfigurarTextos(int fallos, int aciertos, int total)
    {
        success.text = aciertos.ToString();
        fails.text = fallos.ToString();
        points.text = total.ToString();
    }
}