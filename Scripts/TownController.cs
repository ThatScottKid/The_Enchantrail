using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TownController : MonoBehaviour
{
    public PlayerEntity p;
    public Inventory player;
    public intVariable playerGold;
    public List<Shopkeeper> LevelShops, Tier1, Tier2, Tier3;
    public Shopkeeper shopType;
    public Inventory shop;
    public Button[] shopActions;
    public Image[] shopIcons;
    public TextMeshProUGUI shopN, buyText, sellText, eatText;
    public intVariable itemID;
    public Button BuyB, SellB, EatB;
    [SerializeField] private bool shopOpen;


    public void Start()
    {
        shopOpen = false;
    }

    public void SetShops()
    {
        LevelShops.Clear();

        if(playerGold.value < 10)
        {
            LevelShops.AddRange(Tier1);
        }
        else if(playerGold.value < 15)
        {
            LevelShops.AddRange(Tier2);
        }
        else
        {
            LevelShops.AddRange(Tier3);
        }
    }

    public void CreateShopkepperInventory()
    {
        shop.Inv.Clear();
        SetShops();
        shopType = LevelShops[Random.Range(0, LevelShops.Count)];

        for (int i = 0; i < 2; i++)
        {
            shop.Inv.Add(shopType.CommonItems[Random.Range(0, shopType.CommonItems.Count)]);
            shopActions[i].interactable = true;
            // Adds two common items

            if (i == 1)
            {
                if (shop.Inv[0] == shop.Inv[1])
                {
                    CreateShopkepperInventory();
                }
            }
        }
        shop.Inv.Add(shopType.RareItems[Random.Range(0, shopType.RareItems.Count)]);;
        shopActions[2].interactable = true;
        // Adds one rare item

        DisplayShopkeeperInventory();
        
    }

    public void DisplayShopkeeperInventory()
    {
        for (int i = 0; i < shopActions.Length; i++)
        {
            shopActions[i].GetComponentInChildren<TextMeshProUGUI>().text = shop.Inv[i].Value.ToString();
            shopIcons[i].GetComponent<Image>().sprite = shop.Inv[i].ItemSprite;
            shopN.GetComponent<TextMeshProUGUI>().text = shopType.ShopkeeperName;
        }
    }

    public void OpenShop()
    {
        shopOpen = true;
    }

    public void CloseShop()
    {
        if (shopOpen)
        {
            shopOpen = false;
        }
    }

    public void CanBuy()
    {
        if (player.Inv.Count < player.MaxQty && playerGold.value >= shop.Inv[itemID.value].Value && shopOpen)
        {
            if (shop.Inv[itemID.value].isFood)
            {
                EatButton();
            }
            else
            {
                BuyButton();
            }
        }
        else
        {
            return;
        }
    }

    public void CanSell()
    {
        if (player.Inv.Count > 1 && shopOpen)
        {
            SellButton();
        }
        else
        {
            return;
        }
    }

    public void BuyItem() //OnClick Buy Button
    {    
        p.ManageGold(- shop.Inv[itemID.value].Value);
        player.Inv.Add(shop.Inv[itemID.value]);
        shopActions[itemID.value].interactable = false;
        SoldText();
    }

    public void SellItem() //OnClick Sell Button
    {
        p.ManageGold(player.Inv[itemID.value].Value / 2);
        player.Inv.Remove(player.Inv[itemID.value]);
    }

    public void EatFood() //OnClick Eat Button
    {
        p.ManageGold(-shop.Inv[itemID.value].Value);
        p.ManageHealth(shop.Inv[itemID.value].Damage);
        p.ManageHealthText();
        if (shop.Inv[itemID.value].Enchants.Count > 0)
        {
            p.AddStatus(shop.Inv[itemID.value].Enchants[0]);
        }
        shopActions[itemID.value].interactable = false;
        SoldText();
    }

    private void SoldText()
    {
        shopActions[itemID.value].GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
    }

    public void BuyButton()
    {
        BuyB.gameObject.SetActive(true);
        buyText.text = shop.Inv[itemID.value].Value.ToString();
    }
    public void SellButton()
    {
        SellB.gameObject.SetActive(true);
        sellText.text = (player.Inv[itemID.value].Value / 2).ToString();
    }
    public void EatButton()
    {
        EatB.gameObject.SetActive(true);
        eatText.text = shop.Inv[itemID.value].Value.ToString();
    }

    public void SaveTown()
    {
        ES3.Save("ShopType", shopType);
        ES3.Save("ShopInventory", shop.Inv);
    }

    public void LoadTown()
    {
        if (ES3.KeyExists("ShopType"))
        {
            shopType = ES3.Load<Shopkeeper>("ShopType");
        }

        if (ES3.KeyExists("ShopInventory"))
        {
            shop.Inv = ES3.Load<List<Item>>("ShopInventory");
        }
    }

    public void DeleteTown()
    {
        ES3.DeleteKey("ShopType");
        ES3.DeleteKey("ShopInventory");
    }

    private void OnApplicationQuit()
    {
        SaveTown();
    }
}
