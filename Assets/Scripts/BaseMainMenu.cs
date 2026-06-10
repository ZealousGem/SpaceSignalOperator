using UnityEngine;

public class BaseMainMenu : MonoBehaviour
{
    public GameObject menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is create

    protected virtual void Awake()
    {
        Menu(false);
    }
    
    protected virtual void Menu(bool state)
    {
        if(menu == null) return;

        menu.SetActive(state);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
