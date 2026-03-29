using UnityEngine;

public class ShurikenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shurikenPrefab;
    [SerializeField] private int numberOfShurikens;
    
    private int _currentIndex;
    
    private void Start()
    {
        for (int i = 0; i < numberOfShurikens; i++)
        {
            GameObject shurikenCreate = Instantiate(shurikenPrefab, transform.position, Quaternion.identity, transform);
            shurikenCreate.SetActive(false);
        }
        
        ActivateNext();
    }
    
    public void ActivateNext()
    {
        if (_currentIndex < transform.childCount)
        {
            Transform next = transform.GetChild(_currentIndex);
            next.gameObject.SetActive(true);
            _currentIndex++;
            Debug.Log(transform.childCount);
        }
    }
}
