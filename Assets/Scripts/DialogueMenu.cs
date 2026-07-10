using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TMP_Text Subtitles;
    public string LevelName;
    public LevelSubtitleDialogueObject LevelDialogue;
    private Queue<Dialogue> DiaologueClips = new Queue<Dialogue>();

    private void PlayText(string text) => Subtitles.text = text;
    private void ClearText() => Subtitles.text = "";

    protected override void Awake()
    {
        addDialogueToQueue();
        UpdateDialogueSequence();
    }

    private void addDialogueToQueue()
    {
        for (int i = 0; i < LevelDialogue.AudioClip.Count;)
        {
            DiaologueClips.Enqueue(LevelDialogue.AudioClip[i]);
        }
    }

    private void UpdateDialogueSequence()
    {
        if(DiaologueClips.Count != 0)
        {
            PlayDialogue();
        }

        else
        {
            EndDialogue();
        }
    }

    private void PlayDialogue()
    {
        ClearText();
        
        Dialogue currentDialogue = DiaologueClips.Dequeue();

        PlayText(currentDialogue.DialogueText);
        SoundPlayer.PlayDialogueSound(currentDialogue.AudioClip);
    }

    public void EndDialogue()
    {
        ClearText();
        if (DiaologueClips.Count != 0)
        {
          DiaologueClips.Clear();
          SoundPlayer.StopDialogueSound();

        }

        Menu(false);
    }
}
