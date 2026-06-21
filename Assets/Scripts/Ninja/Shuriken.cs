using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField] private int speed;
    
    private Camera _camera;
    private Vector3 _direction;
    private bool _isMoving;
    private bool _used;
    private Animator _animator;

    private ShurikenSpawner _spawnerManager;
    private NinjaManager _ninjaManager;

    void Awake()
    {
        _camera = Camera.main;
        _spawnerManager = GetComponentInParent<ShurikenSpawner>();
        _ninjaManager = FindFirstObjectByType<NinjaManager>();
        _animator = GetComponentInParent<Animator>();
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
        

            if (_ninjaManager) _ninjaManager.RegisterShurikenUsed();
            if (_animator) _animator.SetTrigger("Throw");
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
            if (_ninjaManager != null) _ninjaManager.RegisterHit();
            Destroy(other.gameObject); 
            NotifyAndDestroy();        // Apaga el shuriken y llama al siguiente
        }
        else if (other.CompareTag("Obstacle"))
        {
            // Si choca con un obst�culo, el obst�culo se queda, pero el shuriken se apaga
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

        _spawnerManager.ActivateNext();

        gameObject.SetActive(false);
    }
}
