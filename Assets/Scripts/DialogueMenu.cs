using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueMenu : BaseMainMenu
{
    public TMP_Text Subtitles;
    private UIObersver Subject;
    public LevelSubtitleDialogueObject LevelDialogue;
    private Queue<Dialogue> DiaologueClips = new Queue<Dialogue>();
    private void PlayText(string text) => Subtitles.text = text;
    private void ClearText() => Subtitles.text = "";

    protected override void Awake() => Subject = GameObject.FindWithTag("Manager").GetComponent<UIObersver>();
    private void Start() => addDialogueToQueue();
    protected override void retrieveData(endGameUI data)
    {
        if (data.gameState == GameState.Dialogue)
        {
            UpdateDialogueSequence();
        }
    }

    private void addDialogueToQueue()
    {
        if(LevelDialogue == null)
        {
            EndDialogue();
           // Debug.Log("dialogue is done");
            return;
        }

        for (int i = 0; i < LevelDialogue.AudioClip.Count;)
        {
            DiaologueClips.Enqueue(LevelDialogue.AudioClip[i]);
        }

        if(!menu.activeSelf && DiaologueClips.Count != 0) Menu(true); 
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

       // Debug.Log("donr");
    }
}
