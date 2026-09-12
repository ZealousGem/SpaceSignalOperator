using System.Collections.Generic;

public class ProgressionManager : Singleton<ProgressionManager>
{
    
   public int MaxCount = 0;
   private int ProgressionCounter;
   private List<bool> IsAvailable = new List<bool>();

    public override void Awake()
    {
        base.Awake();
        LoadDefaults();
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
    }
}
