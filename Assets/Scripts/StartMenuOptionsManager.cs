using UnityEngine;

public class StartMenuOptionsManager : SettingsManager
{
   private MainMenu StartMenu;
   protected override void Awake()
    {
        base.Awake();
        StartMenu = GetComponent<MainMenu>();
    }

    public override void Back()
    {
        if(StartMenu == null) return;

        StartMenu.Menu(true);
        Menu(false);
    }
}
