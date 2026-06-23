using UnityEngine;

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
        this.source = sourceClip;
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
