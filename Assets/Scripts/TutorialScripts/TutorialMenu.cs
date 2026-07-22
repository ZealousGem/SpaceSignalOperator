using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text TutorialText;
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture; // Output target for the VideoPlayer
    public Button NextButton;
    private Queue<TutorialList> currentobject = new Queue<TutorialList>();

    protected override void OnEnable()=> EventBus.Subscribe<TutorialEvent>(RetrieveData); 

    protected override void OnDisable()=> EventBus.Unsubscribe<TutorialEvent>(RetrieveData);
    
    private void RetrieveData(TutorialEvent data)
    {
        if(data.gameState == GameState.Tutorial) ActivateTutorial(data.obj);
    }

    protected override void Awake()
    {
        if (NextButton != null)
        {
            NextButton.onClick.AddListener(PlayTutorial);
        }

        base.Awake();
    }

    private void Start()
    {
        videoPlayer.targetTexture = renderTexture;
        rawImage.texture = renderTexture; 
        videoPlayer.isLooping = true; // GIFs usually loop, set this if needed
    }

    private void ActivateTutorial(TutorialObject obj)
    {
        if(obj == null) return;

        currentobject.Clear();

        for (int i = 0; i < obj.tutorial.Count; i++)
        {
          currentobject.Enqueue(obj.tutorial[i]);  
        }

        Menu(true);

        Time.timeScale = 0f;
        rawImage.gameObject.SetActive(true); // Show the raw image
        
        PlayTutorial();

    }

    private void PlayTutorial()
    {
         
        if (currentobject.Count == 0 && Time.timeScale == 0f)
        {
           Time.timeScale = 1f;
           videoPlayer.Stop();

           Menu(false);
           rawImage.gameObject.SetActive(false);

           EventBus.Act(new endGameUI(GameState.TutorialDone));
        }

        else
        {
           TutorialList tutorialPage = currentobject.Dequeue();
           NextSequence(tutorialPage.TutorialText, tutorialPage.clip);
        }
    }

    private void NextSequence(string text, VideoClip videoClip)
    {
        if(currentobject.Count == 0 || videoPlayer == null || rawImage == null || TutorialText == null) throw new UnityException("one of these compooents are not instantied");
        
        TutorialText.text = text;

        videoPlayer.clip = videoClip;
        videoPlayer.Prepare();
        videoPlayer.Play();
    }


}
