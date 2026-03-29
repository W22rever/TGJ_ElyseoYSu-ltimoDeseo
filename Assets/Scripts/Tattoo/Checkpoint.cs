using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [HideInInspector] public int index;
    private PathManager _manager;
    private TMP_Text _textMesh;
    private Collider2D _col; // Guardamos su colisionador

    public void Initialize()
    {
        _manager = GetComponentInParent<PathManager>();
        _textMesh = GetComponent<TMP_Text>();
        _col = GetComponent<Collider2D>(); // Lo buscamos al iniciar

        if (_textMesh != null)
        {
            _textMesh.text = (index + 1).ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pointer") && _manager != null)
        {
            _manager.CheckPointReached(index);
        }
    }

    public void SetVisible(bool isVisible)
    {
        if (_textMesh != null) _textMesh.enabled = isVisible;
    }

    public void SetColor(Color color)
    {
        if (_textMesh != null) _textMesh.color = color;
    }

    // El Manager usará esto para apagar/prender las físicas del punto
    public void SetColliderActive(bool isActive)
    {
        if (_col != null) _col.enabled = isActive;
    }
}