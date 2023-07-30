using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "New Enemies List", menuName = "Enemies List")]
public class EnemiesList : ScriptableObject
{
    // Start is called before the first frame update
    public List<Character> normalEnemies = new List<Character>();
    public List<Character> eliteEnemies = new List<Character>();
    public List<Character> bossEnemies = new List<Character>();
}
