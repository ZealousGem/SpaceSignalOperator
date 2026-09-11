using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class ProgressionManager : Singleton<ProgressionManager>
{
    
    public int MaxCount = 0;
    private int ProgressionCounter;

    private List<bool> HasCompleted = new List<bool>();

   private void LoadDefaults()
    {
        ProgressionCounter = 0;

        for (int i = 0; i < MaxCount; i++)
        {
            HasCompleted.Add(false);
        }
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
        for (int i = 0; i < HasCompleted.Count; i++)
        {
            if (index == i && !HasCompleted[i])
            {
                IncreaseNum();
                HasCompleted[i] = true;
                break;
            }
        }
    }
}
