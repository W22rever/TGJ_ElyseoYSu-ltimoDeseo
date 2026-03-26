using UnityEngine;
using System.Collections.Generic;
public class EventScript : MonoBehaviour
{
    [SerializeField] GameObject buttonNext;
    private int _maxPostItSelected = 5;
    private bool _isButtonActive;
    
    private List<PostIt> _postIts = new List<PostIt>(); // Con solo la lista puedo obtener un contador con la propiedad .Count(). NO OLVIDAR INICIALIZAR LA LISTA
    
    private void OnEnable() => PostIt.OnStateChange += PostItsSelected;
    private void OnDisable() => PostIt.OnStateChange -= PostItsSelected;

    private void PostItsSelected(PostIt postItSelected, bool selected)
    {
        //_postItSelectedCounter += selected ? 1 : -1;

        if (selected)
        {
            // Verificamos si el postIt seleccionado NO está en la lista, lo agregamos.
            // Y también verificamos que la cantidad seleccionada no sobrepase el límite permitido
            if (!_postIts.Contains(postItSelected) && _postIts.Count < _maxPostItSelected) 
            {
                _postIts.Add(postItSelected);
                //Debug.Log(_postIts.Count + ".Se añadió el minijuego: "+ postItSelected.sceneName);
                UpdateLines();
            }
        }
        else
        {
            _postIts.Remove(postItSelected); // Lo quitamos
            //Debug.Log(_postIts.Count + ".Se quitó el minijuego: "+ postItSelected.sceneName);
            UpdateLines();
        }

        bool shouldBeActive = _postIts.Count >= _maxPostItSelected;

        if (shouldBeActive != _isButtonActive)
        {
            _isButtonActive = shouldBeActive;

            if (_isButtonActive)
                ActiveButton();
            else
                DesactiveButton();
        }
    }
    
    private void ActiveButton()
    {
        buttonNext.SetActive(true);
        Debug.Log("buttonNext active");
    }

    private void DesactiveButton()
    {
        buttonNext.SetActive(false);
        Debug.Log("buttonNext desactive");
    }
    
    [SerializeField] private GameObject linePrefab;

    private List<GameObject> _lines = new List<GameObject>();
    
    private void UpdateLines()
    {
        // 1. Eliminar líneas existentes
        foreach (var line in _lines)
        {
            Destroy(line);
        }
        _lines.Clear();

        // 2. Si hay menos de 2, no hacer nada
        if (_postIts.Count < 2) return;

        // 3. Crear nuevas líneas
        for (int i = 0; i < _postIts.Count - 1; i++)
        {
            CreateLine(_postIts[i], _postIts[i + 1]);
        }
    }
    
    private void CreateLine(PostIt a, PostIt b)
    {
        GameObject lineObj = Instantiate(linePrefab, transform);
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();

        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        RectTransform rectA = a.GetComponent<RectTransform>();
        RectTransform rectB = b.GetComponent<RectTransform>();

        Vector3 posA = rectA.position;
        Vector3 posB = rectB.position;

        lr.useWorldSpace = true;

        lr.positionCount = 2;
        lr.SetPosition(0, posA);
        lr.SetPosition(1, posB);

        _lines.Add(lineObj);
    }
    
    
}
