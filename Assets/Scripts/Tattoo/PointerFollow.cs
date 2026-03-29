using UnityEngine;

public class PointerFollow : MonoBehaviour
{
    private Camera cam;
    void Start() => cam = Camera.main;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;

        transform.position = cam.ScreenToWorldPoint(mousePos);
    }
}