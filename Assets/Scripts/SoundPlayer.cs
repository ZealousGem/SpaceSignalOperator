
public static class SoundPlayer 
{
   public static void PlaySound(string name)
   {
        if(SoundManager.Instance == null) return;
        
        SoundManager.Instance.PlaySound(name);

   }

   public static void StopSound(string name)
   {
        if(SoundManager.Instance == null) return;
        
        SoundManager.Instance.StopMusic(name);

   }

    public static void PauseSound()
    {
         if(SoundManager.Instance == null) return;
         
         SoundManager.Instance.PauseAudio();
         
    }

    public static void UnpauseSound()
    {
         if(SoundManager.Instance == null) return;

         SoundManager.Instance.UnPauseAudio();
    } 

    public static void FadeoutSong(string name)
    {
          if(SoundManager.Instance == null) return;
          
          SoundManager.Instance.FadeOutTransition(name);

    }

    public static void FadeInSound(string name)
     {
          if(SoundManager.Instance == null) return;

          SoundManager.Instance.FadeInTransition(name);
     }

     public static void FadeOutSound(string name)
     {
          if(SoundManager.Instance == null) return;

          SoundManager.Instance.FadeOutTransition(name);
     }
}
