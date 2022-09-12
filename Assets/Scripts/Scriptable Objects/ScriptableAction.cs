using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableAction : ScriptableObject
{
    public abstract void CheckLetters(Mosquitoe mosquitoe, string letterToCheck);

    protected void DestroyMosquitoe(Mosquitoe mosquitoe) 
    {
      Debug.Log("destoyed this one: " + mosquitoe.id + " with letter " + mosquitoe.assignedText);
      MosquitoeSpawner.instance.DecreaseMosquitoeCounter();
      Destroy(mosquitoe.gameObject);
    }
}