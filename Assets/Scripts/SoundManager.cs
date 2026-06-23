using UnityEngine;
using System.Threading.Tasks;

public enum SoundType{NonDiagetic, Diagetic}

[System.Serializable]
public class Sound
{
    AudioSource source;
    public string nameClip;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 3f)]
    public float pitch;
    public float maxvol;
    public bool loop = false;
    
    public SoundType type;

    public bool isPlaying = false;

    public void setSource(AudioSource sourceClip)
    {
        source = sourceClip;
        source.clip = clip;
        source.volume = volume;
        maxvol = volume;
        source.pitch = pitch;
        source.playOnAwake = isPlaying;
        source.loop = loop;
    }

    public void SetVolume(float vol)
    {
        if (source != null)
        {
            if (source.volume <= maxvol)
            {
                source.volume = vol;
            }
            else
            {
                source.volume = maxvol;
            }
        }
    }

    public async void FadeOutTransition(float duration)
    {
        if(source == null) return;

        float oringalsound = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if(source == null) return;

            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(oringalsound, 0f, elapsed / duration);

            await Task.Yield();
        }

        if (source != null)
        {
         source.Stop();
         source.volume = oringalsound;    
        }
        
    }

    public void Stop()
    {
        source.Stop();
    }

    public void Play()
    {
        //Debug.Log("Playing sound: " + nameClip);
        source.Play();
    }
}

public class SoundManager : Singleton<SoundManager>
{

     [SerializeField]
     private Sound[] sounds;

     void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject play = new GameObject("Sound : " + i + " : " + sounds[i].nameClip);
            play.transform.SetParent(this.transform);
            sounds[i].setSource(play.AddComponent<AudioSource>());
        }

        PlaySound("theme");
    }

    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].nameClip == name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void FadeOutTransition(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].nameClip == name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void StopMusic(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].nameClip == name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void VolumeAmount(float vol)
    {
        for (int i = 0; i < sounds.Length; i++)
        {  
            if (sounds[i].type == SoundType.Diagetic) sounds[i].SetVolume(vol);
        }
    }
    public void MusicAmount(float vol)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].type == SoundType.NonDiagetic)  sounds[i].SetVolume(vol);   
            
        }
    }
}
