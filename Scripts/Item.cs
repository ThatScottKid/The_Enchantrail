using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string ItemName;
    public int Value;
    public Sprite ItemSprite;
    public int Damage;
    public Attribute DamageType;
    public bool isFood;
    public bool isSuperItem;
    public AudioEvent AttackAudio;

    public List<Status> Enchants;
    public bool EnchantSelf;

    public string ItemDesc;
    public string EncDesc;
    
}
