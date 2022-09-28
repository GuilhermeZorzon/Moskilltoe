using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableChooseLetterAction : ScriptableObject
{
    public abstract void ChooseLetter(string letterToCheck);
}