using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TMP_Text Subtitles;
    public string LevelName;
    public UIObersver Subject;
    public LevelSubtitleDialogueObject LevelDialogue;
    private Queue<Dialogue> DiaologueClips = new Queue<Dialogue>();

    private void PlayText(string text) => Subtitles.text = text;
    private void ClearText() => Subtitles.text = "";

    protected override void Awake()
    {
        addDialogueToQueue();
       // UpdateDialogueSequence();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void retrieveData(endGameUI data)
    {
        if (data.gameState == GameState.Dialogue)
        {
            UpdateDialogueSequence();
        }
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

        if(Subject == null) throw new Exception("Observer has not been instantied, add the component retard");

        Subject.TellObervers(new UIinformation{info = UITextInfo.Counter});
    }
}
