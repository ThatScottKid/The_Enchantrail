using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyType : ScriptableObject
{
    public string EnemyName;
    public Sprite EnemySprite;
    public int MaxHealth;

    public List<Item> EnemyItems;
    public Attribute Weakness;
    public List<Status> StartingStats;
    public bool isBoss;
    public AudioEvent SummonAudio;
    public List<Item> Loot;

}
