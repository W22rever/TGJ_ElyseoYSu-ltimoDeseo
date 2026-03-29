using UnityEngine;
using TMPro;

public class Note : MonoBehaviour
{
    public float speed = 3.9f;
    public KeyCode assignedKey;
    public TextMeshPro textDisplay;

    void Start()
    {
        // Muestra el nombre de la tecla en el texto
        textDisplay.text = assignedKey.ToString();
    }

    void Update()
    {
        // Mueve la nota hacia abajo constantemente
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}