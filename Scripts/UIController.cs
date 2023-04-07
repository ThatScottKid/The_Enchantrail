using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Inventory PlayerInv, ShopInv;
    public Button[] playerSlots;
    public Image[] playerIcons;
    public intVariable itemID;
    private int switchID;
    public bool switching;
    public Image itemIcon;
    public TextMeshProUGUI itemN, itemD, encD, goldLoot;
    public Button SwitchB;
    public GameObject playerSlider, enemySlider, invPanel, mainMenu, playMenu, quitMenu, actionButtons, contButton, shopMenu, itemPanel, WinPanel, LosePanel, VictoryPanel, ShiftPanel, artTag, sAlert;
    public Inventory Loot;
    public GameObject[] LootItems, WorldIcons;
    public Sprite goldIcon;
    public intVariable WorldLvl, PlayerHealth;

    
    private void Start() 
    {
        ResetUI();
        switching = false;
        WorldCounter();

        if (PlayerHealth.value > 0)
        {
            Show(contButton);
        }
        else
        {
            Hide(contButton);
        }
    }

    public void Show(GameObject go)
    {
        go.SetActive(true);
    }

    public void Hide(GameObject go)
    {
        go.SetActive(false);
    }

    public void ResetUI() //When game starts / player loses
    {
        Hide(playerSlider);
        Hide(enemySlider);
        Hide(invPanel);
        Hide(playMenu);
        Hide(actionButtons);
        Hide(shopMenu);
        Hide(itemPanel);
        Show(mainMenu);
    }

    public void MainMenuPlay()
    {
        WorldCounter();
        Hide(mainMenu);
        Show(playMenu);
        Show(invPanel);
        ShopAlert();
    }

    public void PlayMenuBattle()
    {
        Show(playerSlider);
        Show(enemySlider);
        Show(actionButtons);
        Hide(invPanel);
        Hide(playMenu);
        Hide(shopMenu);
        Hide(itemPanel);
    }

    public void PlayMenuShop()
    {
        Show(shopMenu);
    }

    public void OpenQuitMenu()
    {
        Show(quitMenu);
    }

    public void QuitGame()
    {
        Hide(quitMenu);
        Hide(invPanel);
        Hide(playMenu);
        Show(mainMenu);
        Show(contButton);
    }

    private void ShopAlert()
    {
        if (PlayerInv.Inv.Count == 1)
        {
            Show(sAlert);
        }
        else
        {
            Hide(sAlert);
        }
    }

    public void WonUI()
    {
        if (WorldLvl.value > 4)
        {
            Show(VictoryPanel);
            Hide(contButton);
        }
        else
        {
            Show(WinPanel);
        }
        
        Hide(enemySlider);
        Hide(actionButtons);
    }
    public void LoseUI()
    {
        Show(LosePanel);
        Hide(contButton);
        Hide(enemySlider);
        Hide(actionButtons);
    }

    public void LootUI()
    {
        goldLoot.text = "2";
        LootItems[0].GetComponent<Image>().sprite = goldIcon;

        if (Loot.Inv.Count > 0)
        {
            LootItems[1].GetComponent<Image>().sprite = Loot.Inv[0].ItemSprite;
            Show(LootItems[1]);
        }
        else
        {
            Hide(LootItems[1]);
        }
        
        Hide(LootItems[2]);
    }

    public void BattleWonCont()
    {  
        Show(invPanel);
        Show(playMenu);
        Hide(actionButtons);
        Hide(playerSlider);
        Hide(WinPanel);
        ShopAlert();
        WorldCounter();
    }

    public void BattleLostCont()
    {
        Show(mainMenu);
        Hide(actionButtons);
        Hide(playerSlider);
        Hide(LosePanel);
    }

    public void VictoryCont()
    {
        Show(mainMenu);
        Hide(actionButtons);
        Hide(playerSlider);
        Hide(VictoryPanel);
    }

    public void SelectPlayerItem(int id)
    {
        itemID.value = id;
        DisplayItem(PlayerInv, id);
        SwitchB.gameObject.SetActive(true);
    }

    public void SelectShopItem(int id)
    {
        itemID.value = id;
        DisplayItem(ShopInv, id);
        SwitchB.gameObject.SetActive(false);
    }

    private void DisplayItem(Inventory inv, int id)
    {
        itemIcon.GetComponent<Image>().sprite = inv.Inv[id].ItemSprite;
        itemN.GetComponent<TextMeshProUGUI>().text = inv.Inv[id].ItemName;

        if (inv.Inv[id].EncDesc == "")
        {
            if(inv.Inv[id].Enchants.Count > 0)
            {
                encD.GetComponent<TextMeshProUGUI>().text = "Enchantment:\n\n" + inv.Inv[id].Enchants[0].StatName;
            }
            else
            {
                if (inv.Inv[id].DamageType.AttributeName == "Sword")
                {
                    encD.GetComponent<TextMeshProUGUI>().text = "Enchantment:\n\n +1 Damage";
                }
                else
                {
                    encD.GetComponent<TextMeshProUGUI>().text = "";
                }
            }
        }
        else
        {
            encD.GetComponent<TextMeshProUGUI>().text = "Enchantment:\n\n" + inv.Inv[id].EncDesc;
        }
        

        if (inv.Inv[id].ItemDesc == "")
        {
            if (inv.Inv[id].Damage == 0)
            {
                if (inv.Inv[id].Enchants.Count > 0)
                {
                    itemD.GetComponent<TextMeshProUGUI>().text = "Apply " + inv.Inv[id].Enchants[0].StatName;
                }
                else
                {
                    itemD.GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            else
            {
                if (inv.Inv[id].isFood == true)
                {
                    itemD.GetComponent<TextMeshProUGUI>().text = "Restore " + inv.Inv[id].Damage + " health";
                }
                else
                {
                    if (inv.Inv[id].DamageType == null || inv.Inv[id].DamageType.AttributeName == "Sword")
                    {
                        itemD.GetComponent<TextMeshProUGUI>().text = "Deal " + inv.Inv[id].Damage + " damage";
                    }
                    else
                    {
                        itemD.GetComponent<TextMeshProUGUI>().text = "Deal " + inv.Inv[id].Damage +" "+ inv.Inv[id].DamageType.AttributeName + " damage";
                    }
                }
            }
        }
        else
        {
            itemD.GetComponent<TextMeshProUGUI>().text = inv.Inv[id].ItemDesc;
        }

        if (inv.Inv[id].Damage == 0)
        {
            Show(artTag);
        }
        else
        {
            Hide(artTag);
        }
        
        Show(itemPanel);
    }

    public void LoadInventory() //Refresh icons in player inventory
    {
        for (int i = 0; i < PlayerInv.MaxQty; i++)
        {
            playerIcons[i].gameObject.SetActive(false);
            playerSlots[i].interactable = false;
        }

        for (int i = 0; i < PlayerInv.Inv.Count; i++)
        {
            playerIcons[i].gameObject.SetActive(true);
            playerIcons[i].GetComponent<Image>().sprite = PlayerInv.Inv[i].ItemSprite;
            playerSlots[i].interactable = true;
        }
    }

    public void SelectSwitch() //Click 'shift' button
    {
        Show(ShiftPanel);
        switching = true;
        switchID = itemID.value;
        Hide(playMenu);
        Hide(shopMenu);
    }

    public void SwitchItems() //Shift the items
    {
        if(switching == true)
        {
            Hide(itemPanel);
            Hide(ShiftPanel);
            
            if (switchID > itemID.value)
            {
                PlayerInv.Inv.Insert(itemID.value, PlayerInv.Inv[switchID]);
                PlayerInv.Inv.RemoveAt(switchID + 1);
            }
            else if (switchID < itemID.value)
            {
                PlayerInv.Inv.Insert(itemID.value + 1, PlayerInv.Inv[switchID]);
                PlayerInv.Inv.RemoveAt(switchID);
            }
            
            Show(playMenu);
            LoadInventory();
            switching = false;
        }
    }

    public void CancelSwitch()
    {
        Hide(itemPanel);
        Hide(ShiftPanel);
        Show(playMenu);
        switching = false;
    }

    private void WorldCounter()
    {
        if (WorldLvl.value < 6)
        {
            for(int i = 0; i < WorldIcons.Length; i++)
            {
                Hide(WorldIcons[i]);
            }

            for(int j = 0; j < WorldLvl.value; j++)
            {
                Show(WorldIcons[j]);
            }
        }
    }

    public void GameComplete()
    {
        if (WorldLvl.value < 6)
        {
            ResetUI();
            Hide(contButton);
        }
    }
}
