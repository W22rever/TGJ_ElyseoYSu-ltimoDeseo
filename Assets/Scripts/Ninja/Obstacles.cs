using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private float timeToWait;
    [SerializeField] private Vector3 targetPos;
    
    private Vector3 _initialPosition;
    private float _timer;
    private bool _isMovingToTarget = true;
    private bool _waiting;
    private bool _isClose;
    private void Start() => _initialPosition = transform.position;

    private void Update()
    {
        Vector3 target = _isMovingToTarget ? targetPos : _initialPosition;
        _isClose = Vector2.Distance(transform.position, target) <= Mathf.Epsilon;

        if (_waiting)
        {
            _timer += Time.deltaTime;
            if (_timer >= timeToWait)
            {
                _waiting = false;
                _timer = 0;
                _isMovingToTarget = !_isMovingToTarget;
            }
            
            return;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (_isClose) _waiting = true;


    } 
    
}
