using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField] private int speed;
    
    private Camera _camera;
    private Vector3 _direction;
    private bool _isMoving;
    private bool _used;
    
    private ShurikenSpawner _manager;
    
    void Awake()
    {
        _camera = Camera.main;
        _manager = GetComponentInParent<ShurikenSpawner>();
    }

    void OnEnable()
    {
        _isMoving = false;
        _used = false;
    }
    
    void Update()
    {
        if(!_isMoving && Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            
            _direction = (mouseWorldPos - transform.position).normalized;
            
            _isMoving = true;
        }
        
        if (_isMoving)
        {
            transform.position += _direction * (speed * Time.deltaTime);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            NotifyAndDestroy();
        }
    }

    void OnBecameInvisible()
    {
        NotifyAndDestroy();
    }

    void NotifyAndDestroy()
    {
        if (_used) return;

        _used = true;

        _manager.ActivateNext();

        gameObject.SetActive(false);
    }
}
