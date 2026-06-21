using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class PostItConexions : MonoBehaviour
{
    [SerializeField] GameObject buttonNext;
    
    public int maxPostItSelected = 5;
    [HideInInspector] public List<PostIt> postIts = new List<PostIt>(); // Con solo la lista puedo obtener un contador con la propiedad .Count(). NO OLVIDAR INICIALIZAR LA LISTA
    
    private bool _isButtonActive;
    
    private void OnEnable() => PostIt.OnStateChange += PostItsSelected;
    private void OnDisable() => PostIt.OnStateChange -= PostItsSelected;

    private void PostItsSelected(PostIt postItSelected, bool selected)
    {
        //_postItSelectedCounter += selected ? 1 : -1;

        if (selected)
        {
            // Verificamos si el postIt seleccionado NO está en la lista, lo agregamos.
            // Y también verificamos que la cantidad seleccionada no sobrepase el límite permitido
            if (!postIts.Contains(postItSelected) && postIts.Count < maxPostItSelected) 
            {
                postIts.Add(postItSelected);
                //Debug.Log(_postIts.Count + ".Se añadió el minijuego: "+ postItSelected.sceneName);
                UpdateLines();
            }
        }
        else
        {
            postIts.Remove(postItSelected); // Lo quitamos
            //Debug.Log(_postIts.Count + ".Se quitó el minijuego: "+ postItSelected.sceneName);
            UpdateLines();
        }

        bool shouldBeActive = postIts.Count >= maxPostItSelected;

        if (shouldBeActive != _isButtonActive)
        {
            _isButtonActive = shouldBeActive;

            if (_isButtonActive)
                ActiveButton();
            else
                DesactiveButton();
        }
    }
    
    private void ActiveButton() => buttonNext.SetActive(true);
    private void DesactiveButton() => buttonNext.SetActive(false);
    
    
    [SerializeField] private GameObject linePrefab;
    private List<GameObject> _lines = new List<GameObject>();
    
    private void UpdateLines()
    {
        // Eliminar líneas existentes
        foreach (var line in _lines)
        {
            Destroy(line);
        }
        _lines.Clear();

        // Si hay menos de 2, no hacer nada
        if (postIts.Count < 2) return;

        // Crear nuevas líneas
        for (int i = 0; i < postIts.Count - 1; i++)
        {
            CreateLine(postIts[i], postIts[i + 1]);
        }
    }
    
    private void CreateLine(PostIt origin, PostIt target)
    {
        GameObject lineObj = Instantiate(linePrefab, transform);
        LineRenderer path = lineObj.GetComponent<LineRenderer>();

        //RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        RectTransform rectA = origin.GetComponent<RectTransform>();
        RectTransform rectB = target.GetComponent<RectTransform>();

        Vector3 posA = rectA.position;
        Vector3 posB = rectB.position;

        path.useWorldSpace = true;

        path.positionCount = 2;
        path.SetPosition(0, posA);
        path.SetPosition(1, posB);

        _lines.Add(lineObj);
    }
    
    
}
