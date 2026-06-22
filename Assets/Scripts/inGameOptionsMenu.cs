using UnityEngine;

public class inGameOptionsMenu : SettingsManager
{
    // Update is called once per frame
    private PauseMenu PauseMenu;

    protected override void Awake()
    {
        base.Awake();
        PauseMenu = GetComponent<PauseMenu>();
    }

    public override void Back()
    {
        if( PauseMenu == null) return;

        PauseMenu.Menu(true);
        Menu(false);
    }


}
