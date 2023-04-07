using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class BattleController : MonoBehaviour
{
    public PlayerEntity player;
    public EnemyEntity enemy;
    private EnemyType enemyType;
    private MissionController mc;
    private bool isPlayerTurn;
    public Button[] Actions;
    public Image[] ActionIcons;
    public GameObject enemyImage;

    public TextMeshProUGUI enemyTitle;
    public intVariable WorldLevel;
    
    public Inventory Loot;
    public Material EncMat, NonMat;

    public AudioSource PBattleAudioSource, EBattleAudioSource;

    public UnityEvent YouWin;
    public UnityEvent YouLose;
    public UnityEvent YouDraw;

    private void Awake()
    {
        LoadPlayer();
    }

    private void Start()
    {
        mc = GetComponent<MissionController>();
    }

    public void BeginBattle()
    {
        RemoveEnchantments(player);
        RemoveEnchantments(enemy);
        Loot.Inv.Clear();
        enemyType = mc.battles[0];
        isPlayerTurn = true;
        SetupTurn();
        LoadEnemy();
        player.RefreshStatIcons();
        enemy.RefreshStatIcons();
    }

    private void SetupButtons()
    {
        for (int i = 0; i < Actions.Length; i++) //Disable all action buttons
        {
            ActionIcons[i].gameObject.SetActive(false);
            Actions[i].interactable = false;
            Actions[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            Actions[i].GetComponent<Image>().material = NonMat;
        }

        for (int i = 0; i < player.inventory.Inv.Count; i++)    //Re-enable them as long as there is an item
        {
            ActionIcons[i].gameObject.SetActive(true);
            ActionIcons[i].GetComponent<Image>().sprite = player.inventory.Inv[i].ItemSprite;
            Actions[i].interactable = true;
            Actions[i].GetComponentInChildren<TextMeshProUGUI>().text = player.inventory.Inv[i].Damage.ToString();
            player.LoadItemTypes(i);

            if (player.Enchantments.Count > 0 && player.Enchantments[0] == i)
            {
                Actions[i].GetComponent<Image>().material = EncMat;
            }

            if (i == 2)
            {
                break;
            }
        }
    }

    private void LoadEnemy() //Assign EnemyType stats to EnemyEntity
    {
        enemy.EntityName = enemyType.EnemyName;
        enemy.MaxHealth = (enemyType.MaxHealth + Random.Range(0,4) + (2 * (WorldLevel.value - 1)));
        enemy.CurrentHealth.value = enemyType.MaxHealth;
        enemy.Weakness = enemyType.Weakness;
        enemyImage.GetComponent<SpriteRenderer>().sprite = enemyType.EnemySprite;
        enemyTitle.text = enemyType.EnemyName;

        enemy.inventory.Inv.Clear();
        for (int i = 0; i < enemyType.EnemyItems.Count; i++)
        {
            enemy.inventory.Inv.Add(enemyType.EnemyItems[i]);
        }
        
        for(int j = 0; j < enemyType.StartingStats.Count; j++)
        {
            enemy.AddStatus(enemyType.StartingStats[j]);
        }

        if (enemyType.isBoss)
        {
            Loot.Inv.AddRange(enemyType.Loot);
        }
    }

    private void SetupTurn()
    {
        for (int i = 0; i < Actions.Length; i++)
        {
            if(isPlayerTurn)
            {
                SetupButtons();
            }
            else
            {
                Actions[i].interactable = false;
                StartCoroutine(EnemyMove());
            }
        }
    }
    private IEnumerator EnemyMove()
    {
        yield return new WaitForSeconds(1.5f);
        int r = Random.Range(0, enemy.inventory.Inv.Count);
        SelectMove(r); //Enemy AI
    }

    public void SelectMove(int selection)
    {
        if(isPlayerTurn) //OnClick
        {
            if(player.Enchantments.Count > 0 && player.Enchantments[0] == selection && player.inventory.Inv[selection].DamageType.AttributeName == "Sword") //Sword Enchantment
            {
                player.ManageModifier(1);
            }
            PerformMove(player, enemy, player.inventory.Inv[selection]);
            AddStat(player, enemy, selection);
            EnchantItem(player, selection);
            player.inventory.Inv[selection].AttackAudio.Play(PBattleAudioSource);
            StartCoroutine(enemy.SpriteBlink());
        }
        else
        {
            if(enemy.Enchantments.Count > 0 && enemy.Enchantments[0] == selection && enemy.inventory.Inv[selection].DamageType.AttributeName == "Sword") //Sword Enchantment
            {
                enemy.ManageModifier(1);
            }
            PerformMove(enemy, player, enemy.inventory.Inv[selection]);
            AddStat(enemy, player, selection);
            EnchantItem(enemy, selection);
            enemy.inventory.Inv[selection].AttackAudio.Play(EBattleAudioSource);
        }
        IsGameOver();
    }

    private void PerformMove(GameEntity attacker, GameEntity defender, Item move) //DamageCalc
    {
        int d = move.Damage;

        defender.DefendCheck(move);
        attacker.AttackCheck(move);

        d += (attacker.BattleModifier - defender.BattleModifier);

        if (d < 0)
        {
            d = 0;
        }

        if (move.DamageType == defender.Weakness)
        {
            d += 2;
        }

        defender.ManageHealth(-d);
        defender.HitAudioEvent.Play(defender.HitAudioSource);

        attacker.ResetBattleMod();
        defender.ResetBattleMod();
        StopAllCoroutines();
    }

    private void IsGameOver()
    {
        if(player.CurrentHealth.value <= 0 && enemy.CurrentHealth.value <= 0)
        {
            GameIsOver();
            YouDraw.Invoke();
        }
        else if(player.CurrentHealth.value <= 0)
        {
            GameIsOver();
            YouLose.Invoke();
        }
        else if(enemy.CurrentHealth.value <= 0)
        {
            AddLoot();
            GameIsOver();
            YouWin.Invoke();
        }
        else
        {
            ChangeTurn();
        }
    }

    private void GameIsOver()
    {
        RemoveEnchantments(player);
        RemoveEnchantments(enemy);
        player.Stats.Clear();
        player.HideItemTypes();
        player.HideStatIcons();
        enemy.Stats.Clear();
        enemy.HideStatIcons();
    }

    private void ChangeTurn()
    {
        isPlayerTurn =! isPlayerTurn;
        SetupTurn();
    }

    private void AddStat(GameEntity a, GameEntity d, int s)
    {
        if(a.inventory.Inv[s].isSuperItem)
        {
            if (a.inventory.Inv[s].EnchantSelf)
            {
                a.AddStatus(a.inventory.Inv[s].Enchants[0]);
                a.RefreshStatIcons();
            }
            else
            {
                d.AddStatus(a.inventory.Inv[s].Enchants[0]);
                d.RefreshStatIcons();
            }  
        }
        
        if (a.Enchantments.Count > 0 && a.Enchantments[0] == s && a.inventory.Inv[s].Enchants.Count > 0) //If attacker is enchanted AND matches the selected item AND item has enchantments
        {
            if (a.inventory.Inv[s].EnchantSelf)
            {
                for(int i = 0; i < a.inventory.Inv[s].Enchants.Count; i++)
                {
                    a.AddStatus(a.inventory.Inv[s].Enchants[i]);
                }
                a.RefreshStatIcons();
            }
            else
            {
                for(int i = 0; i < a.inventory.Inv[s].Enchants.Count; i++)
                {
                    d.AddStatus(a.inventory.Inv[s].Enchants[i]);
                }
                d.RefreshStatIcons();
            }
        }
    }

    private void EnchantItem(GameEntity ge, int s)
    {
        if (ge.inventory.Inv.Count > 2)
        {
            ge.Enchantments.Clear();
            int r = Random.Range(0, 3);

            if (r == s)
            {
                EnchantItem(ge, s);
            }

            ge.Enchantments.Add(r);
        }
    }

    private void RemoveEnchantments(GameEntity ge)
    {
        ge.Enchantments.Clear();
    }

    private void AddLoot()
    {
        if (player.inventory.Inv.Count >= player.inventory.MaxQty)
        {
            player.PlayerGold.value += Loot.Inv[0].Value / 2;
        }
        else
        {
            player.inventory.Inv.AddRange(Loot.Inv);
        } 
    }

    public void SavePlayer()
    {
        ES3.Save("PlayerInventory", player.inventory.Inv);
        ES3.Save("PlayerGold", player.PlayerGold.value);
        ES3.Save("PlayerHealth", player.CurrentHealth.value);
        ES3.Save("PlayerStats", player.Stats);
    }

    public void LoadPlayer()
    {
        if (ES3.KeyExists("PlayerInventory"))
        {
            player.inventory.Inv = ES3.Load<List<Item>>("PlayerInventory");
        }

        if (ES3.KeyExists("PlayerGold"))
        {
            player.PlayerGold.value = ES3.Load<int>("PlayerGold");
        }

        if (ES3.KeyExists("PlayerHealth"))
        {
            player.CurrentHealth.value = ES3.Load<int>("PlayerHealth");
        }

        if (ES3.KeyExists("PlayerStats"))
        {
            player.Stats = ES3.Load<List<Status>>("PlayerStats");
        }
    }

    public void DeletePlayer()
    {
        ES3.DeleteKey("PlayerInventory");
        ES3.DeleteKey("PlayerGold");
        ES3.DeleteKey("PlayerHealth");
        ES3.DeleteKey("PlayerStats");
    }

    private void OnApplicationQuit()
    {
        SavePlayer();
    }
}
