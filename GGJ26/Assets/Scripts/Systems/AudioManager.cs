using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Clip
{
    public string name;
    public AudioClip clip;
    public float volume;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource source;

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        source = GetComponent<AudioSource>();
    }

    [SerializeField] private List<Clip> audioClips;

    public void PlayAudioClip(string name)
    {
        for(int i = 0; i < audioClips.Count; i++)
        {
            if(name.ToLower() == audioClips[i].name.ToLower())
            {
                source.PlayOneShot(audioClips[i].clip, audioClips[i].volume);
                return;
            }
        }
    }
}
