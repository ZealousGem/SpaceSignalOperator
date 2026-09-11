using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int ProgressionCounter = 0;

    [SerializeField] private List<Button> Buttons;


    protected override void Awake()
    {
        ProgressionCounter = ProgressionManager.Instance.GetProgessionCounter();
        SetUpButtons();

        base.Awake();
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
        }
    }

    private void LoadMapIndex(int index)
    {
        LoadingManager.Instance.LoadScene(index + 2);
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
