using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Condition : ScriptableObject
{
    public string ConditionName;
    [HideInInspector] public bool ConditionMet;

    public void CheckCondition(Item i)
    {
        switch (ConditionName)
        {
            case "Is Weapon":
            if (i.Damage > 0)
            {
                ConditionMet = true;
            }
            break;
            case "Is Artefact":
            if (i.Damage == 0)
            {
                ConditionMet = true;
            }
            break;
            case "Is Axe":
            if (i.DamageType.AttributeName == "Slashing")
            {
                ConditionMet = true;
            }
            break;
            case "Is Physical":
            if(i.DamageType.AttributeName == "Slashing" || i.DamageType.AttributeName == "Bashing" || i.DamageType.AttributeName == "Shadow")
            {
                ConditionMet = true;
            }
            break;
            case "Is Magical":
            if(i.DamageType.AttributeName == "Frost" || i.DamageType.AttributeName == "Fire" || i.DamageType.AttributeName == "Poison")
            {
                ConditionMet = true;
            }
            break;
            default:
            ConditionMet = false;
            break;
        }
    }
}
