using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("Configuración de Zoom")]
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 5f;

    private Camera _cam;
    private Vector3 _dragOrigin;
    private Vector3 _startPosition;

    private void Start()
    {
        _cam = Camera.main;
        _startPosition = transform.position; // Guarda el centro (X:0, Y:0)
    }

    private void Update()
    {
        HandleZoom();
        HandlePan();
    }

    private void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0)
        {
            _cam.orthographicSize -= scroll * zoomSpeed;
            _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize, minZoom, maxZoom);

            // Si se aleja al máximo, obligamos a la cámara a centrarse
            if (_cam.orthographicSize >= maxZoom)
            {
                transform.position = _startPosition;
            }
        }
    }

    private void HandlePan()
    {
        // Si no hay zoom, no hay movimiento
        if (_cam.orthographicSize >= maxZoom) return;

        // Click derecho presionado
        if (Input.GetMouseButtonDown(1))
        {
            _dragOrigin = _cam.ScreenToWorldPoint(Input.mousePosition);
        }

        // Arrastrar el mouse
        if (Input.GetMouseButton(1))
        {
            Vector3 difference = _dragOrigin - _cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPosition = _cam.transform.position + difference;

            // Calculamos cuánto espacio "sobra" entre el zoom actual y el tamaño máximo de 5
            float verticalLimit = maxZoom - _cam.orthographicSize;
            float horizontalLimit = verticalLimit * _cam.aspect; // Ajusta el ancho al 16:9 de tu pantalla

            // Limitamos la posición para que NUNCA pase del borde original
            float clampedX = Mathf.Clamp(targetPosition.x, _startPosition.x - horizontalLimit, _startPosition.x + horizontalLimit);
            float clampedY = Mathf.Clamp(targetPosition.y, _startPosition.y - verticalLimit, _startPosition.y + verticalLimit);

            _cam.transform.position = new Vector3(clampedX, clampedY, -10f);
        }
    }
}