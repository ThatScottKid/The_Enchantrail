using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : GameEntity
{
    public SpriteRenderer sr;
    public Color NoneCol, DefaultCol;
    
    public IEnumerator SpriteBlink()
    {
        sr.color = NoneCol;
        yield return new WaitForSeconds(0.15f);
        sr.color = DefaultCol;
    }
}
