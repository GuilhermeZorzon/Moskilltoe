using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptedAction", menuName = "Scripted Actions/New ResistentCheckLetterAction")]
public class ResistentCheckLetterAction : ScriptableAction
{
  public override void CheckLetters(Mosquitoe mosquitoe, string letterToCheck)
  {
    if (mosquitoe.assignedText.Contains(letterToCheck) && !mosquitoe.isDestoyed)
    {
      mosquitoe.timesHit += 1;
      if (mosquitoe.timesHit == 3)
      {
        mosquitoe.isDestoyed = true;
        DestroyMosquitoe(mosquitoe);
        MosquitoeSpawner.instance.mosquitoesToRemove.Add(mosquitoe);
        mosquitoe.RemoveLetter(letterToCheck);
      }
    }
  }
}