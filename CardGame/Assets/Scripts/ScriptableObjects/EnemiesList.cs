using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "New Enemies List", menuName = "Enemies List")]
public class EnemiesList : ScriptableObject
{
    // Start is called before the first frame update
    public List<CharacterData> normalEnemies = new List<CharacterData>();
    public List<CharacterData> eliteEnemies = new List<CharacterData>();
    public List<CharacterData> bossEnemies = new List<CharacterData>();
}
