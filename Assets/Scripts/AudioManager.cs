using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HatAudioStates { Destroyed, Collected };
public enum PlayerAudioStates { Damaged};
public class AudioManager : MonoBehaviour
{
    static Object[] hatAudioClips;
    static Object[] playerAudioClips;

    static AudioSource hatAudioSource;
    static AudioSource playerAudioSource;

    static Dictionary<HatAudioStates, AudioClip> hatAudioClipsLibrary = new Dictionary<HatAudioStates, AudioClip>();
    static Dictionary<PlayerAudioStates, AudioClip> playerAudioClipsLibrary = new Dictionary<PlayerAudioStates, AudioClip>();

    void Start()
    {
        PopulateHatAudioClipsList();
        PopulateHatAudioClipsLibrary();

        PopulatePlayerAudioClipsList();
        PopulatePlayerAudioClipsLibrary();
    }
    private static void PopulateHatAudioClipsList()
    {
        hatAudioClips = Resources.LoadAll("Audio/SFX/Hat", typeof(AudioClip));
    }

    private static void PopulateHatAudioClipsLibrary()
	{
        hatAudioClipsLibrary.Add(HatAudioStates.Destroyed, (AudioClip)hatAudioClips[0]);
        hatAudioClipsLibrary.Add(HatAudioStates.Collected, (AudioClip)hatAudioClips[1]);
    }

    private static void PopulatePlayerAudioClipsList()
    {
        playerAudioClips = Resources.LoadAll("Audio/SFX/Player", typeof(AudioClip));
    }

    private static void PopulatePlayerAudioClipsLibrary()
    {
        playerAudioClipsLibrary.Add(PlayerAudioStates.Damaged, (AudioClip)playerAudioClips[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlayHatAudioClip(HatAudioStates audioState, AudioSource audio)
	{
        hatAudioSource = audio;
        AudioClip clipToPlay = hatAudioClipsLibrary[audioState];
        hatAudioSource.PlayOneShot(clipToPlay);
    }

    public static void PlayPlayerAudioClip(PlayerAudioStates audioState, AudioSource audio)
    {
        playerAudioSource = audio;
        AudioClip clipToPlay = playerAudioClipsLibrary[audioState];
        playerAudioSource.PlayOneShot(clipToPlay);
    }


}
