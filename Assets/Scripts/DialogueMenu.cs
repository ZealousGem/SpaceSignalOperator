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

        if(DiaologueClips.Count != 0 && RingUi.gameObject.activeSelf) PlayDialogue(); 

        else if (!RingUi.gameObject.activeSelf && DiaologueClips.Count != 0) StartCoroutine(RingingSequence());

        else EndDialogue();     
    }

    private IEnumerator RingingSequence()
    {
        if(RingUi == null) yield break;

       // Debug.Log("playing ringing");
        Menu(true);
        ClearText();
        
        currentDialogue = DiaologueClips.Dequeue();

        PlayText(currentDialogue.DialogueText);
        SoundPlayer.PlayDialogueSound(currentDialogue.AudioClip);


        if(SoundManager.Instance == null) yield break;
        AudioSource source = SoundManager.Instance.GetDialogueSoundProperty().getSource();

        if(source == null) throw new UnityException("source is null in DialogueMenu");
    //    yield return new WaitForSeconds(source.clip.length);
        float timer = 0f;

        while (timer < source.clip.length)
        {
            if(DiaologueClips.Count == 0 && !source.isPlaying) yield break;

            timer += Time.deltaTime;
            yield return null;
        }

        RingUi.gameObject.SetActive(true);
    }

    private void PlayDialogue()
    {
        ClearText();
        
        currentDialogue = DiaologueClips.Dequeue();

        PlayText(currentDialogue.DialogueText);
        SoundPlayer.PlayDialogueSound(currentDialogue.AudioClip);
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

       // Debug.Log("donr");
    }
}
