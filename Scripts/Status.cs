using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Status : ScriptableObject
{
    public string StatName;
    public Sprite StatSprite;
    public Color StatCol;
    public Effect effect;
    public Condition condition;
    public int Effector;
    public bool defenceCheck;
    public bool isPersistent;
    

    public void ActivateStatus(GameEntity ge, Item item)
    {
        if (condition == null)
        {
            effect.ActivateEffect(ge, Effector);
        } 
        else
        {
            condition.CheckCondition(item);

            if (condition.ConditionMet)
            {
                effect.ActivateEffect(ge, Effector);
                condition.ConditionMet = false;
            }        
        }
    }
}
