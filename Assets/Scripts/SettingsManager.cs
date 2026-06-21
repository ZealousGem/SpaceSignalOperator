using System;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Dropdown ResoluationDropDown;

    public Dropdown QualityDropDown;

    public Dropdown WindowScaleDropDown;

    public Dropdown VsyncDropDown; 

    private Resolution[] sizes;

    private int resIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        setUpDropwDowns();
    }

    private void setUpDropwDowns()
    {
        SetUpResDropDown();
        setVsyncDropDown();
        setQualityDropDown();
        SetWindowScaleDropdown();
    }

    private void SetWindowScaleDropdown()
    {
        WindowScaleDropDown.ClearOptions();

        List<string> Options = new List<string>{"Fullscreen", "Windowed-Fullscreen", "Windowed"};

        WindowScaleDropDown.AddOptions(Options);

        WindowScaleDropDown.onValueChanged.AddListener(ChangeWindowScale);
    }

    public void ChangeWindowScale(int index)
    {
        switch (index)
        {
            case 0: Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.ExclusiveFullScreen); break;
            case 1: Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow); break;
            case 2: Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed); break;

        }
    }

    private void setQualityDropDown()
    {
        QualityDropDown.ClearOptions();

        List<string> options = new List<string>(QualitySettings.names);
        QualityDropDown.AddOptions(options);

        QualityDropDown.onValueChanged.AddListener(ChangeQuality);
    }

    public void ChangeQuality(int index) => QualitySettings.SetQualityLevel(index, false);

    private void setVsyncDropDown()
    {
        VsyncDropDown.ClearOptions();

        List<string> Options = new List<string>{"on", "off"};
        VsyncDropDown.AddOptions(Options);

        VsyncDropDown.onValueChanged.AddListener(setVysnc);
    }

    public void setVysnc(int index)
    {
        switch (index)
        {
            case 0: QualitySettings.vSyncCount = 1; break;
            case 1: QualitySettings.vSyncCount = 0; break;
            default: Debug.Log("not found"); break;
        }
    }

    private void SetUpResDropDown()
    {
     
        List<Resolution> uniqueRes = new List<Resolution>();
       foreach (var res in Screen.resolutions)
       {
        if (!uniqueRes.Exists(x => x.width == res.width && x.height == res.height))
        {
            uniqueRes.Add(res);
        }
       }
        sizes = uniqueRes.ToArray();

        ResoluationDropDown.ClearOptions();
        List<string> option = new List<string>();
        int sizesIndex = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            string choice = sizes[i].width + "x" + sizes[i].height;
            option.Add(choice);

            if (sizes[i].width == Screen.currentResolution.width && sizes[i].height == Screen.currentResolution.height)
            {
                sizesIndex = i;
            }
        }

        ResoluationDropDown.AddOptions(option);
      //  dropdown.value = sizesIndex;
        resIndex = sizesIndex;
      //  dropdown.RefreshShownValue();
       ResoluationDropDown.onValueChanged.AddListener(delegate { setSize(); });
    }

    public void setSize()
    {
        if (sizes == null || sizes.Length == 0) 
        {
        Debug.LogWarning("Resolutions array is not initialized!");
        return;
        }

        int safeIndex = Mathf.Clamp(resIndex, 0, sizes.Length - 1);
        Resolution resolution = sizes[safeIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        //data.ResolutionInd = safeIndex;
    }



   
}
