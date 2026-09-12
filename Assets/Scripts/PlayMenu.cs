using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int ProgressionCounter = 0;

    [SerializeField] private List<Button> Buttons;

    private MainMenu mainMenu;

    private readonly string unLockedHexCode = "#1F4ABA";

    private readonly string textMeshColour = "#544E4E";

    protected override void Awake()
    {
        mainMenu = GetComponent<MainMenu>();
        base.Awake();
    }

    void Start()
    {
        ProgressionCounter = ProgressionManager.Instance.GetProgessionCounter();
        SetUpButtons();
    }

    public void GoBack()
    {
        DOTween.KillAll();
        Menu(false);
        mainMenu.Menu(true);
    }

    private void SetUpButtons()
    {
        if(ProgressionCounter > Buttons.Count)
        {
            Debug.Log("PorgressionSystem is bugged fix counter");
            return;
        }

        for (int i = 0; i < ProgressionCounter; i++)
        {
           Buttons[i].onClick.AddListener(() => LoadMapIndex(i));

           TMP_Text textMesh = Buttons[i].gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
           
          // if (ColorUtility.TryParseHtmlString(textMeshColour, out Color newTextColor)) textMesh.color = newTextColor;
           
           textMesh.text = "Level " + i;

           if (ColorUtility.TryParseHtmlString(unLockedHexCode, out Color newColor))Buttons[i].image.color = newColor;
        
        }
    }

    private void LoadMapIndex(int index)
    {
        Debug.Log(index);
        LoadingManager.Instance.LoadScene(index + 1);
    }

    public override void Menu(bool state)
    {
        base.Menu(state);

        if (state)
        {
            // will do it tomorrow 
        }
    }


}
