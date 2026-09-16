using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct ProgressionData
{
   public int ProgressionCounter {get; set;}
   public List<bool> IsAvailable {get; set;}
}

public class ProgressionManager : Singleton<ProgressionManager>
{
    
   public int MaxCount = 0;
   private int ProgressionCounter;
   private List<bool> IsAvailable = new List<bool>();
   private ProgressionData data = new ProgressionData();
   private const string fileName = "ProgressionData.json";
   private string persistentPath => Path.Combine(Application.persistentDataPath, fileName);

    public override void Awake()
    {
        base.Awake();
        DataInFile();
    }

    private void DataInFile()
    {
      if (File.Exists(persistentPath))
      {
        try
        {
            string json = File.ReadAllText(persistentPath);
            if (string.IsNullOrEmpty(json)){setParameters(); return;} 
            
            data = JsonUtility.FromJson<ProgressionData>(json);
            setParameters();
          //  Debug.Log("File found " + Application.persistentDataPath);
            return;
        } 

        catch 
        {
             Debug.Log("File Corrupted");
             setParameters();
            return;
            // Handle corrupted JSON
        }
      }
         Debug.Log("File not found");
         setParameters();
        
         // No file found in either location
    }

    private void setParameters()
    {
        if (!EqualityComparer<ProgressionData>.Default.Equals(data, default))
        {
            ProgressionCounter = data.ProgressionCounter;
            IsAvailable = data.IsAvailable;
        }

        else
        {
            Debug.Log("defaults");
            LoadDefaults();
        }

        setData();
    } 

    private void setData()
    {
        data.ProgressionCounter = ProgressionCounter;
        data.IsAvailable = IsAvailable;
         

        if (File.Exists(persistentPath))
        {
           string json = JsonUtility.ToJson(data, true);
           File.WriteAllText(persistentPath, json);
        }
       
    }

   private void LoadDefaults()
    {
        ProgressionCounter = 0;

        for (int i = 0; i < MaxCount; i++)
        {
            IsAvailable.Add(false);
        }

        CompleteLevel(0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void IncreaseNum()
    {
        if(ProgressionCounter == MaxCount) return;

        ProgressionCounter += 1;
    }

    public int GetProgessionCounter()
    {
        return ProgressionCounter;
    }

    public void CompleteLevel(int index)
    {
        if(index > MaxCount) return;
        
        for (int i = 0; i < IsAvailable.Count; i++)
        {
            if (index == i && !IsAvailable[i])
            {
                IncreaseNum();
                IsAvailable[i] = true;
                break;
            }
        }

        setData();
    }
}
