using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMenu : BaseMainMenu
{
    public TMP_Text Subtitles;
    private UIObersver Subject;
    public Image RingUi;
    public LevelSubtitleDialogueObject LevelDialogue;
    private Queue<Dialogue> DiaologueClips = new Queue<Dialogue>();
    private void PlayText(string text) => Subtitles.text = text;
    private void ClearText() => Subtitles.text = "";
    private Dialogue currentDialogue;
    private bool RingUIGone = true;

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
            return;
        }

        for (int i = 0; i < LevelDialogue.AudioClip.Count; i++)
        {
            DiaologueClips.Enqueue(LevelDialogue.AudioClip[i]);
        }

        if(RingUi.gameObject.activeSelf) RingUi.gameObject.SetActive(false); 

       // Debug.Log("dialogue is done");
    }

    private void UpdateDialogueSequence()
    {
        if (DiaologueClips.Count == 0)
        {
            EndDialogue();
            return;
        }
        if(!RingUIGone && !RingUi.gameObject.activeSelf) RingUi.gameObject.SetActive(true);

        PlayDialogue();    
    }

    private void PlayDialogue()
    {
        if(!menu.activeSelf) Menu(true);
        
        ClearText();
        
        currentDialogue = DiaologueClips.Dequeue();

        PlayText(currentDialogue.DialogueText);
        SoundPlayer.PlayDialogueSound(currentDialogue.AudioClip);

        if(RingUIGone) RingUIGone = false;
    }

    public void EndDialogue()
    {
        ClearText();
        SoundPlayer.StopDialogueSound();

        if (DiaologueClips.Count != 0)
        {
          DiaologueClips.Clear();
        }

        Menu(false);

        if(Subject == null) throw new UnityException("Observer has not been instantied, add the component retard");

        Subject.TellObervers(new UIinformation{info = UITextInfo.Counter});

       Debug.Log("done");
    }
}
