using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    public WorldVariable WV;
    public SpriteRenderer Background;
    public Sprite[] AllBGs;

    private void Awake()
    {
        DefaultBG();
    }

    public void DefaultBG()
    {
        int i = Random.Range(0, AllBGs.Length);
        Background.sprite = AllBGs[i];
    }

    public void SetBG()
    {
        Background.sprite = WV.CurrentWorld.BG;
    }
}
