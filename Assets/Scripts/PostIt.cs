using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PostIt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum States
    {
        None,
        Selected,
        Unselected,
    }
    
    public static event Action<PostIt,bool> OnStateChange;
    
    [Header("Scene Name")] public string sceneName;
    
    
    [Header("Modifications")]
    [SerializeField] private float growthValue;
    [SerializeField] private States state;
    
    private Vector3 _originalScale;
    private bool _isSelected;

    void Awake()
    {
        _originalScale = transform.localScale;
        state = States.Unselected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (state == States.Unselected) IncreaseScale();   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (state == States.Unselected) DecreaseScale();
    }

    public void OnClick()
    {
        // Interruptor de activación
        _isSelected = !_isSelected;
        
        // Cambio de estado dependiendo del interruptor
        if (_isSelected) state = States.Selected;
        else state = States.Unselected;
        
        // 
        switch (state)
        {
            case States.Selected:
                IncreaseScale();
                OnChageState();
                break;
            case States.Unselected:
                DecreaseScale();
                OnChageState();  
                break;
        }
    }

    private void IncreaseScale() => transform.localScale = _originalScale * growthValue;
    private void DecreaseScale() => transform.localScale = _originalScale;
    
    private void OnChageState() => OnStateChange?.Invoke(this, _isSelected);
}