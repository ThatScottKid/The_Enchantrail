using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Shopkeeper : ScriptableObject
{
    public string ShopkeeperName;
    public List<Item> CommonItems;
    public List<Item> RareItems;
}
