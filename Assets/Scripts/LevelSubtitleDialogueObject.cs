using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Dialogue
{
    public Sound AudioClip;

    public string DialogueText;
}

[CreateAssetMenu(fileName ="DialogueLevel", menuName = "ScriptableObjects/Dialogue")]
public class LevelSubtitleDialogueObject : ScriptableObject
{
   public List<Dialogue> AudioClip;
   
}
