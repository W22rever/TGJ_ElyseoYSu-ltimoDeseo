using UnityEngine;

public class PointerFollow : MonoBehaviour
{
    private Camera cam;
    private Collider2D col; // Referencia al collider

    void Start()
    {
        cam = Camera.main;
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        transform.position = cam.ScreenToWorldPoint(mousePos);

        // Solo activamos las colisiones si el botón está presionado
        if (col != null)
        {
            col.enabled = Input.GetMouseButton(0);
        }
    }
}