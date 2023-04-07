using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    public intVariable WorldLvl;
    public List<World> AllWorlds;
    public List<World> liveWorlds;
    public List<World> completedWorlds;
    public World FinalWorld;
    public WorldVariable WV;
    public int battleCount;
    public List<EnemyType> battles;


    public void BuildWorld() //Set current world & create list of battles
    {
        if (liveWorlds[0] == AllWorlds[0])
        {
            WV.CurrentWorld = liveWorlds[0]; //Select 'tutorial' world
        }
        else
        {
            WV.CurrentWorld = liveWorlds[Random.Range(0, liveWorlds.Count)]; //Select random world
        }

        
        if (WV.CurrentWorld.EnemyTypes.Count > 0)
        {
            for (int i = 0; i < battleCount; i++)
            {
                battles.Add(WV.CurrentWorld.EnemyTypes[Random.Range(0, WV.CurrentWorld.EnemyTypes.Count)]); //Add 5 random enemies
            }
        }
        
        battles.Add(WV.CurrentWorld.Bosses[Random.Range(0, WV.CurrentWorld.Bosses.Count)]); //Add 1 random boss

    }

    public void MissionUpdate() //After win, update battles
    {
        battles.RemoveAt(0);

        if (battles.Count == 0) //World completed
        {
            WorldLvl.value += 1;
            completedWorlds.Add(WV.CurrentWorld);
            liveWorlds.Remove(WV.CurrentWorld);
            if (completedWorlds.Count == 4)
            {
                liveWorlds.Clear();
                liveWorlds.Add(FinalWorld);
            }
             
            if (WorldLvl.value < 6)
            {
                BuildWorld();
            }
            else
            {
                ResetMissions();
            }
            
        }
    }

    public void ResetMissions() //After game loss / game start
    {
        battles.Clear();
        liveWorlds.Clear();
        completedWorlds.Clear();
        WorldLvl.value = 1;
        
        for (int i = 0; i < AllWorlds.Count; i++)
        {
            liveWorlds.Add(AllWorlds[i]);
        }
    }

    public void SaveMissions()
    {
        ES3.Save("WorldLevel", WorldLvl.value);
        ES3.Save("LiveWorlds", liveWorlds);
        ES3.Save("CompletedWorlds", completedWorlds);
        ES3.Save("WorldVariable", WV.CurrentWorld);
        ES3.Save("Battles", battles);
    }

    public void LoadMissions()
    {
        if (ES3.KeyExists("WorldLevel"))
        {
            WorldLvl.value = ES3.Load<int>("WorldLevel");
        }

        if (ES3.KeyExists("LiveWorlds"))
        {
            liveWorlds = ES3.Load<List<World>>("LiveWorlds");
        }

        if (ES3.KeyExists("CompletedWorlds"))
        {
            completedWorlds = ES3.Load<List<World>>("CompletedWorlds");
        }

        if (ES3.KeyExists("WorldVariable"))
        {
            WV.CurrentWorld = ES3.Load<World>("WorldVariable");
        }

        if (ES3.KeyExists("Battles"))
        {
            battles = ES3.Load<List<EnemyType>>("Battles");
        }
    }

    public void DeleteMissions()
    {
        ES3.DeleteKey("WorldLevel");
        ES3.DeleteKey("LiveWorlds");
        ES3.DeleteKey("CompletedWorlds");
        ES3.DeleteKey("WorldVariable");
        ES3.DeleteKey("Battles");
    }

    private void OnApplicationQuit()
    {
        SaveMissions();
    }
}
