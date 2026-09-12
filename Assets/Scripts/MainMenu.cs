using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Buttons")]
    [SerializeField] private Button StartButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button QuitButton;

    private StartMenuOptionsManager _OptionsMenu;
    private PlayMenu playMenu;

    protected override void Awake()
    {
         _OptionsMenu = GetComponent<StartMenuOptionsManager>();
         playMenu = GetComponent<PlayMenu>();

         StartButton.onClick.AddListener(PlayMenu);
         OptionsButton.onClick.AddListener(OptionsMenu);
         QuitButton.onClick.AddListener(OnApplicationQuit);
    }

    private void OptionsMenu()
    {
        DOTween.KillAll();
         Menu(false);
        _OptionsMenu.Menu(true);
    }

    private void PlayMenu()
    {
        DOTween.KillAll();
        Menu(false);
        playMenu.Menu(true);
    }

    private void OnApplicationQuit()
    {
        DOTween.KillAll();
        Application.Quit();
    }
}
