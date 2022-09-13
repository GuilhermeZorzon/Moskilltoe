using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptedAction", menuName = "Scripted Actions/New DefaultCheckLetterAction")]
public class DefaultCheckLetterAction : ScriptableAction
{
    public override void CheckLetters(Mosquitoe mosquitoe, string letterToCheck)
    {
      if (mosquitoe.assignedText.Contains(letterToCheck) && !mosquitoe.isDestoyed)
      {
        if (mosquitoe.currentAssignedText.Count == 1)
        {
          mosquitoe.isDestoyed = true;
          DestroyMosquitoe(mosquitoe);
          MosquitoeSpawner.instance.mosquitoesToRemove.Add(mosquitoe);
        }
        mosquitoe.RemoveLetter(letterToCheck);
      }
    }
}