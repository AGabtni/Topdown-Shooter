
using UnityEngine;
[CreateAssetMenu(fileName = "new Character Profile", menuName = "Character Profile")]
public class Character: ScriptableObject
{
    public CharacterName characterName;
    public Sprite profileSprite;
    public GameObject spritePrefab;
}


public enum CharacterName{
    Character1,
    Character2,
    Character3

}