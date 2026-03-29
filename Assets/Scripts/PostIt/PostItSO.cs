using UnityEngine;

[CreateAssetMenu(fileName = "PostItSO", menuName = "Scriptable Objects/PostItSO")]
public class PostItSO : ScriptableObject
{
    public string minigameName;
    public string sceneName;
    public Sprite baseSprite;
    public Sprite selectedSprite;
}
