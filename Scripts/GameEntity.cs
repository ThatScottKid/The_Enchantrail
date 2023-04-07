using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEntity : MonoBehaviour
{
    public string EntityName;
    public Inventory inventory;
    public int MaxHealth;
    public intVariable CurrentHealth;
    public Attribute Weakness;
    public Slider HealthBar;
    [SerializeField] private TextMeshProUGUI hpText;
    public AudioEvent HitAudioEvent;
    public AudioSource HitAudioSource;
    public List<Status> Stats;
    public List<GameObject> StatIcons;
    public List<int> Enchantments;
    [HideInInspector] public int BattleModifier;


    public void ResetHealth()
    {
        CurrentHealth.value = MaxHealth;
        HealthBar.maxValue = MaxHealth;
        HealthBar.value = CurrentHealth.value;
        hpText.text = CurrentHealth.value.ToString() + " / " + MaxHealth.ToString();
    }

    public void ManageHealth(int amount)
    {
        CurrentHealth.value += amount;
        HealthBar.value = CurrentHealth.value;

        if (CurrentHealth.value > MaxHealth)
        {
            CurrentHealth.value = MaxHealth;
        }
        
        hpText.text = CurrentHealth.value.ToString() + " / " + MaxHealth.ToString();
    }

    public void SetHealth(int h)
    {
        CurrentHealth.value = h;
    }

    public void AddStatus(Status stat)
    {
        Stats.Add(stat);
    }

    private void RemoveStatus(int i)
    {
        StatIcons[i].SetActive(false);
        Stats.RemoveAt(i);
    }

    public void RefreshStatIcons()
    {
        HideStatIcons();
        
        for (int j = 0; j < Stats.Count; j++)
        {
            StatIcons[j].GetComponent<Image>().sprite = Stats[j].StatSprite;
            StatIcons[j].SetActive(true);
        }
    }

    public void HideStatIcons()
    {
        for (int i = 0; i < StatIcons.Count; i++)
        {
            StatIcons[i].SetActive(false);
        }
    }

    public void AttackCheck(Item item)
    {
        if (Stats.Count > 0)
        {
            for (int i = Stats.Count - 1; i >= 0; i--)
            {
                if (Stats[i].defenceCheck == false)
                {
                    Stats[i].ActivateStatus(this, item);

                    if (Stats[i].isPersistent == false)
                    {
                        RemoveStatus(i);
                    }
                }
            }
        }
    }

    public void DefendCheck(Item item)
    {
        for (int i = Stats.Count - 1; i >= 0; i--)
        {
            if (Stats[i].defenceCheck)
            {
                Stats[i].ActivateStatus(this, item);

                if (Stats[i].isPersistent == false)
                {
                    RemoveStatus(i);
                }
            }
        }
    }

    public void ManageModifier(int amount)
    {
        BattleModifier += amount;
    }

    public void ResetBattleMod()
    {
        BattleModifier = 0;
    }
}
