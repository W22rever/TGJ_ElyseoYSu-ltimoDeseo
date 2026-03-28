using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private Transform linesParent;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private float minDistance = 0.1f;

    private LineRenderer _currentLine;
    private List<Vector3> _points = new List<Vector3>();
    private Camera _camera;

    private void Start() => _camera = Camera.main; 

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartNewLine();
        }

        
        if (Input.GetMouseButton(0))
        {
            if (!_currentLine) return;
            
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;

            Vector3 worldPos = _camera.ScreenToWorldPoint(mousePos);

            if (_points.Count == 0 || Vector3.Distance(_points[^1], worldPos) > minDistance)
            {
                _points.Add(worldPos);
                _currentLine.positionCount = _points.Count;
                _currentLine.SetPositions(_points.ToArray());
            }
        }
    }
    
    private void StartNewLine()
    {
        GameObject newLine = Instantiate(linePrefab, linesParent);
        _currentLine = newLine.GetComponent<LineRenderer>();
        
        _points.Clear();
    }
}