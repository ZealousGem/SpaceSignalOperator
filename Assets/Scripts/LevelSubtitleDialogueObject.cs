using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Dialogue
{
    public Sound AudioClip;
    
    [TextArea(5,5)]
    public string DialogueText;
}

[CreateAssetMenu(fileName ="DialogueLevel", menuName = "ScriptableObjects/Dialogue")]
public class LevelSubtitleDialogueObject : ScriptableObject
{
   public List<Dialogue> AudioClip;
   
}
