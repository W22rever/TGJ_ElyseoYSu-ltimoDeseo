using UnityEngine;

public class SharkMinigame : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private RectTransform targetArea;
    [SerializeField] private RectTransform playerMarker;

    [Header("Movimiento jugador")]
    [SerializeField] private float speedUp = 5f;
    [SerializeField] private float gravity = 4f;

    [Header("Zona objetivo")]
    [SerializeField] private float targetSpeed = 2f;
    [SerializeField] private float targetSize = 0.2f;

    [Header("Progreso")]
    [SerializeField] private float maxBalanceTime = 3f;

    private float _currentY;
    private float _balanceTimer;
    private float _targetCenter;
    private bool _isPlaying = true;

    private float _targetMinY;
    private float _targetMaxY;
    
    private float _velocity;
    private float _changeTimer;

    void Start()
    {
        _currentY = 0.5f;         // empieza en el centro
        _balanceTimer = maxBalanceTime / 2f; // margen inicial
    }
    
    void Update()
    {
        if (!_isPlaying) return;

        MoveTarget();
        HandleMovement();
        CheckBalance();
        UpdateUI();
    }

    void MoveTarget()
    {
        _changeTimer -= Time.deltaTime;

        if (_changeTimer <= 0)
        {
            _velocity = Random.Range(-1f, 1f);
            _changeTimer = Random.Range(0.5f, 1.5f);
        }

        _targetCenter += _velocity * targetSpeed * Time.deltaTime;

        float halfSize = targetSize / 2f;
        _targetCenter = Mathf.Clamp(_targetCenter, halfSize, 1f - halfSize);

        _targetMinY = _targetCenter - halfSize;
        _targetMaxY = _targetCenter + halfSize;
    }

    // 🎮 Input del jugador
    void HandleMovement()
    {
        if (Input.GetMouseButton(0))
            _currentY += speedUp * Time.deltaTime;
        else
            _currentY -= gravity * Time.deltaTime;

        _currentY = Mathf.Clamp01(_currentY);
    }

    // 🧠 Lógica de ganar/perder
    void CheckBalance()
    {
        if (_currentY >= _targetMinY && _currentY <= _targetMaxY)
        {
            _balanceTimer += Time.deltaTime;

            if (_balanceTimer >= maxBalanceTime)
            {
                Debug.Log("GANASTE");
                _isPlaying = false;
            }
        }
        else
        {
            _balanceTimer -= Time.deltaTime * 2f;

            if (_balanceTimer <= 0)
            {
                Debug.Log("PERDISTE");
                _isPlaying = false;
            }
        }

        _balanceTimer = Mathf.Clamp(_balanceTimer, 0, maxBalanceTime);
    }

    // 🎨 UI
    void UpdateUI()
    {
        // Límites de la barra del player
        float parentHeight = ((RectTransform)playerMarker.parent).rect.height;
        float markerHeight = playerMarker.rect.height;

        float halfSize = (markerHeight / parentHeight) / 2f;

        float clampedY = Mathf.Clamp(_currentY, halfSize, 1f - halfSize);

        playerMarker.anchorMin = new Vector2(0.5f, clampedY);
        playerMarker.anchorMax = new Vector2(0.5f, clampedY);

        // Límites de la barra del target
        targetArea.anchorMin = new Vector2(0.5f, _targetMinY);
        targetArea.anchorMax = new Vector2(0.5f, _targetMaxY);
    }
}