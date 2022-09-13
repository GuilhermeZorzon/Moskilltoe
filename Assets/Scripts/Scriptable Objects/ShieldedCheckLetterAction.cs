using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptedAction", menuName = "Scripted Actions/New ShieldedCheckLetterAction")]
public class ShieldedCheckLetterAction : ScriptableAction
{
    public override void CheckLetters(Mosquitoe mosquitoe, string letterToCheck)
    {
      foreach (string letter in mosquitoe.assignedText)
      {
        Debug.Log(System.DateTime.Now.ToLongTimeString() + " Letter for mosquitoe " + letter);
      }

      if (mosquitoe.assignedText.Count > 0 && mosquitoe.assignedText[0] == letterToCheck && !mosquitoe.isDestoyed)
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