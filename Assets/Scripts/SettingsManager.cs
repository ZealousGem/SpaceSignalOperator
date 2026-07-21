
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using JetBrains.Annotations;

public struct SettingsData
{
    public int ScreenScaleInd;
    public int VsyncInd;
    public int ResolutionInd;
    public int QualityInd;
    public float DiageticSoundValue;
    public float NonDiageticSoundValue; 

}

public enum SettingsDataValue {ScreenScale, Resolution, Quality, DiageticSoundValue, NonDiageticSoundValue, Vsync}

public class SettingsManager : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Dropdown ResoluationDropDown;

    public TMP_Dropdown QualityDropDown;

    public TMP_Dropdown WindowScaleDropDown;

    public TMP_Dropdown VsyncDropDown; 

    public Slider DiageticSlider; 

    public Slider NonDiageticSlider; 

    private Resolution[] sizes;

    private bool hasData = false;

    private int resIndex = 0;

    SettingsData data = new SettingsData();

    protected override void Awake()
    {
        base.Awake();
        SetUpUiElements();
    }

    private void Start() => setUpUserChoices();

    private void DefaultSettings()
    {
        data.ResolutionInd = resIndex;
        data.VsyncInd = 0;
        data.ScreenScaleInd = 1;
        data.QualityInd = QualitySettings.GetQualityLevel();

        data.DiageticSoundValue = 1f;
        data.NonDiageticSoundValue = 1f;
    }

    private void setUpUserChoices()
    {
       if ( SettingsDataManager.Instance != null &&  SettingsDataManager.Instance.DataInFile())
        {
            data = SettingsDataManager.Instance.getFileData();
            hasData = true;
        }

        setSettings();
    }

    private void setSettings()
    {
        if (SettingsDataManager.Instance == null) return;

        if (hasData)
        {
            data = SettingsDataManager.Instance.getFileData();
            ApplySettings();        
        }

        else
        {
           DefaultSettings();
           ApplySettings(); 
        }    

    }

    private void ApplySettings()
    {
        
        ChangeQuality(data.QualityInd);
        setSize(data.ResolutionInd);
        ChangeWindowScale(data.ScreenScaleInd);
        setVysnc(data.VsyncInd);
        
        ManageDiageticAudio(data.DiageticSoundValue);
        ManageNonDiageticAudio(data.NonDiageticSoundValue);

        setUI();

        SettingsDataManager.Instance.setData(data);
       
    }

    void setUI()
    {
        ResoluationDropDown.value = data.ResolutionInd;
        ResoluationDropDown.RefreshShownValue();

        QualityDropDown.value = data.QualityInd;
        QualityDropDown.RefreshShownValue();

        VsyncDropDown.value = data.VsyncInd;
        VsyncDropDown.RefreshShownValue();

        WindowScaleDropDown.value = data.ScreenScaleInd;
        WindowScaleDropDown.RefreshShownValue();

        DiageticSlider.value = data.DiageticSoundValue;
        NonDiageticSlider.value = data.NonDiageticSoundValue;

    }

    private void SetUpUiElements()
    {
        SetUpResDropDown();
        setVsyncDropDown();
        setQualityDropDown();
        SetWindowScaleDropdown();

        SetUpSliders();
    }

    private void SetUpSliders()
    {
        if(DiageticSlider == null || NonDiageticSlider == null) throw new UnityException("Sliders have not been binded");

        DiageticSlider.SetValueWithoutNotify(1f);
        NonDiageticSlider.SetValueWithoutNotify(1f);

        DiageticSlider.onValueChanged.AddListener(ManageDiageticAudio);
        NonDiageticSlider.onValueChanged.AddListener(ManageNonDiageticAudio);

    }

    public void ManageDiageticAudio(float volume)
    {
        SoundPlayer.ManageDiageticSound(PerceptialVolume(volume));
        WriteData(SettingsDataValue.DiageticSoundValue, volume);
    }

    public void ManageNonDiageticAudio(float volume)
    { 
        SoundPlayer.ManageNonDiageticSound(PerceptialVolume(volume));
        WriteData(SettingsDataValue.NonDiageticSoundValue, volume);  
    } 

    private float PerceptialVolume(float volume)
    {
        float PerceptialVolume = Mathf.Pow(volume, 2f);
        return PerceptialVolume;
    }

    private void SetWindowScaleDropdown()
    {
        WindowScaleDropDown.ClearOptions();

        List<string> Options = new List<string>{"Fullscreen", "Windowed-Fullscreen", "Windowed"};

        WindowScaleDropDown.AddOptions(Options);

        WindowScaleDropDown.RefreshShownValue();

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

        WriteData(SettingsDataValue.ScreenScale, index);
    }

    private void setQualityDropDown()
    {
        QualityDropDown.ClearOptions();

        List<string> options = new List<string>(QualitySettings.names.ToList());
        QualityDropDown.AddOptions(options);

        QualityDropDown.RefreshShownValue();

        QualityDropDown.onValueChanged.AddListener(ChangeQuality);
    }

    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, false);
        WriteData(SettingsDataValue.Quality, index);
    } 

    private void setVsyncDropDown()
    {
        VsyncDropDown.ClearOptions();

        List<string> Options = new List<string>{"on", "off"};
        VsyncDropDown.AddOptions(Options);

        VsyncDropDown.onValueChanged.AddListener(setVysnc);
    }

    public virtual void Back()
    {
        BaseMainMenu menu = GetComponent<BaseMainMenu>();

        if(menu == null) return;

        menu.Menu(true);
        Menu(false);
        
    }

    public void setVysnc(int index)
    {
        switch (index)
        {
            case 0: QualitySettings.vSyncCount = 1; break;
            case 1: QualitySettings.vSyncCount = 0; break;
            default: Debug.Log("not found"); break;
        }

        WriteData(SettingsDataValue.Vsync, index);
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
        ResoluationDropDown.value = sizesIndex;
        ResoluationDropDown.RefreshShownValue();

       ResoluationDropDown.onValueChanged.AddListener(setSize);

       resIndex = sizesIndex;
    }

    public void setSize(int index)
    {
        if (sizes == null || sizes.Length == 0) 
        {
        Debug.LogWarning("Resolutions array is not initialized!");
        return;
        }

        int safeIndex = Mathf.Clamp(index, 0, sizes.Length - 1);

        Resolution resolution = sizes[safeIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        WriteData(SettingsDataValue.Resolution, safeIndex);
        //data.ResolutionInd = safeIndex;
    }

    private void WriteData(SettingsDataValue type, float value)
    {
        if(SettingsDataManager.Instance == null) return;

        switch (type)
        {
          case SettingsDataValue.Resolution: data.ResolutionInd = (int)value; break;
          case SettingsDataValue.ScreenScale: data.ScreenScaleInd = (int)value; break;
          case SettingsDataValue.Quality: data.QualityInd = (int)value; break;
          case SettingsDataValue.Vsync: data.VsyncInd = (int)value; break;
          case SettingsDataValue.DiageticSoundValue: data.DiageticSoundValue = value; break;
          case SettingsDataValue.NonDiageticSoundValue: data.NonDiageticSoundValue = value; break;
        }

        SettingsDataManager.Instance.setData(data);
    }
   
}
