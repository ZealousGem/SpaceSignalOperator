using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public struct TutorialList
{
    public VideoClip clip;

    [TextArea(12,12)]
    public string TutorialText;
}

[CreateAssetMenu(fileName ="TutorialObject", menuName = "ScriptableObjects/Tutorial")]
public class TutorialObject : ScriptableObject
{
    public List<TutorialList> tutorial;
}
