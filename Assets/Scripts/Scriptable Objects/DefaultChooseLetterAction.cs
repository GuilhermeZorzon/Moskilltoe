using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptedAction", menuName = "Scripted Actions/New DefaultChooseLetterAction")]
public class DefaultChooseLetterAction : ScriptableChooseLetterAction
{
    public override void ChooseLetter(string letter)
    {
      KeyboardManager.instance.chooseLetter(letter);
    }
}