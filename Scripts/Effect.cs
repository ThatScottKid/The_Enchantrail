using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Effect : ScriptableObject
{
    public string EffectName;

    public void ActivateEffect(GameEntity ge, int effector)
    {
        switch (EffectName)
        {
            case "Take Damage":
            ge.ManageHealth(-effector);
            break;
            case "Restore Health":
            ge.ManageHealth(effector);
            break;
            case "Increase Modifier":
            ge.ManageModifier(effector);
            break;
            case "Decrease Modifier":
            ge.ManageModifier(-effector);
            break;
            default:
            break;
        }
        
    }
}
