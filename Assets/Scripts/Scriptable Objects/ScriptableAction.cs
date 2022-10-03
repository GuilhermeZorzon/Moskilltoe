using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableAction : ScriptableObject
{
    public abstract void CheckLetters(Mosquitoe mosquitoe, string letterToCheck);

    protected void DestroyMosquitoe(Mosquitoe mosquitoe) 
    {
      MosquitoeSpawner.instance.DecreaseMosquitoeCounter();
      Destroy(mosquitoe.gameObject);
    }
}