using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int index;
    private PathManager _manager;

    private void Start() => _manager = FindFirstObjectByType<PathManager>();


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pointer")) _manager.CheckPointReached(index);
    }
}