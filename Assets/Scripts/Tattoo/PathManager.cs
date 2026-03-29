using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private int totalPoints;

    
    private int _startIndex;
    private int _currentIndex = -1;
    private bool _failed;

    public void CheckPointReached(int index)
    {
        if (_failed) return;

        if (_currentIndex == -1)
        {
            _currentIndex = index;
            Debug.Log("Inicio en: " + index);
            return;
        }

        int nextIndex = (_currentIndex + 1) % totalPoints;

        if (index == nextIndex)
        {
            _currentIndex = index;

            if (_currentIndex == _startIndex)
            {
                Debug.Log("COMPLETADO LOOP ✔");
            }
        }
        else
        {
                Debug.Log("ERROR ❌");
            _failed = true;
        }
    }
}