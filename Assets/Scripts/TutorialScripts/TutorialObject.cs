using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public struct TutorialList
{
    public VideoClip clip;
    public string TutorialText;
}

[CreateAssetMenu(fileName ="TutorialObject", menuName = "ScriptableObjects/Tutorial")]
public class TutorialObject : ScriptableObject
{
    public List<TutorialList> tutorial;
}
