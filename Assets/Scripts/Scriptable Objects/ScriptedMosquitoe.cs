using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptedMosquitoe", menuName = "Scripted Objects/New ScriptedMosquitoe")]
public class ScriptedMosquitoe : ScriptableObject
{
    public int assignedTextLength;
    public float speed;
    public Sprite _sprite;

    public List<ScriptableAction> checkLetterActions;
}
