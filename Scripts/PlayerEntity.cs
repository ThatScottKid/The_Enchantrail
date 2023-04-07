using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerEntity : GameEntity
{
    public intVariable PlayerGold;
    public intVariable DefaultGold;
    public List<Item> DefaultInventory;
    public Sprite AxeType, MaceType, FireType, FrostType, PoisonType, ShadowType, SwordType;
    public Image[] PlayerItemTypes;
    public TextMeshProUGUI goldText, healthText;

    public void ResetPlayer()
    {
        inventory.Inv.Clear();

        for (int i = 0; i < DefaultInventory.Count; i++)
        {
            inventory.Inv.Add(DefaultInventory[i]);
        }

        PlayerGold.value = DefaultGold.value;
        ResetHealth();
        ContinuePlayer();
    }

    public void ContinuePlayer()
    {
        ManageGold(0);
        ManageHealthText();
    }

    public void ManageGold(int amount)
    {
        PlayerGold.value += amount;

        if (PlayerGold.value <= 0)
        {
            PlayerGold.value = 0;
        }

        goldText.GetComponent<TextMeshProUGUI>().text = PlayerGold.value.ToString();
    }

     public void ManageHealthText()
    {
        healthText.text = CurrentHealth.value.ToString() + "/" + MaxHealth.ToString();
    }

    public void LoadItemTypes(int i)
    {
        PlayerItemTypes[i].enabled = true;

        if (inventory.Inv[i].DamageType == null)
        {
            PlayerItemTypes[i].sprite = SwordType;
        }
        else
        {
            switch (inventory.Inv[i].DamageType.AttributeName)
            {
                case "Bashing":
                PlayerItemTypes[i].sprite = MaceType;
                break;
                case "Slashing":
                PlayerItemTypes[i].sprite = AxeType;
                break;
                case "Fire":
                PlayerItemTypes[i].sprite = FireType;
                break;
                case "Frost":
                PlayerItemTypes[i].sprite = FrostType;
                break;
                case "Poison":
                PlayerItemTypes[i].sprite = PoisonType;
                break;
                case "Shadow":
                PlayerItemTypes[i].sprite = ShadowType;
                break;
                default:
                PlayerItemTypes[i].sprite = SwordType;
                break;
            }
        }
    }

    public void HideItemTypes()
    {
        for (int i = 0; i < PlayerItemTypes.Length; i++)
        {
            PlayerItemTypes[i].enabled = false;
        }
    }
}
