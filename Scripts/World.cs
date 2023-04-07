using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class World : ScriptableObject
{
    public string WorldName;

    public List<EnemyType> EnemyTypes;
    public List<EnemyType> Bosses;

    public Sprite BG;
}
