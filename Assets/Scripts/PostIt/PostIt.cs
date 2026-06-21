using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PostIt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum States
    {
        None,
        Selected,
        Unselected,
    }
    public static event Action<PostIt,bool> OnStateChange;
    
    [Header("Modifications")]
    [SerializeField] private float growthValue;
    [SerializeField] private States state;
    
    [Header("Data")]
    [SerializeField] private PostItSO postItData;
    
    private Vector3 _originalScale;
    private bool _isSelected;
    private Image _imageComponent;
    
    public PostItSO GetData() => postItData;
    
    //private PathManager _pathManager;

    void Awake()
    {
        state = States.Unselected;
        _imageComponent  = GetComponent<Image>();

        //_pathManager = GameObject.Find("PathManager").GetComponent<PathManager>();
    }

    private void Start()
    {
        _originalScale = transform.localScale;
        _imageComponent.sprite = postItData.baseSprite;
        Debug.Log(postItData.baseSprite.name);
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
        state = _isSelected ? States.Selected : States.Unselected;
        
        
        switch (state)
        {
            case States.Selected:
                IncreaseScale();
                OnChageState();
                _imageComponent.sprite = postItData.selectedSprite;
                break;
            case States.Unselected:
                DecreaseScale();
                OnChageState();  
                _imageComponent.sprite = postItData.baseSprite;
                break;
        }
    }

    private void IncreaseScale() => transform.localScale = _originalScale * growthValue;
    private void DecreaseScale() => transform.localScale = _originalScale;
    
    private void OnChageState()
    {
        OnStateChange?.Invoke(this, _isSelected);

        if (_isSelected)
        {
            MinigameSequenceManager.Instance.AddMinigame(postItData);
        }
        else
        {
            MinigameSequenceManager.Instance.RemoveMinigame(postItData);
        }
    }
}