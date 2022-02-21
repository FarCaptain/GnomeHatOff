using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HatAudioStates { Destroyed, Collected };
public enum PlayerAudioStates { Damaged};
public enum LogAudioStates { Rolling};
public class AudioManager : MonoBehaviour
{
    static Object[] hatAudioClips;
    static Object[] playerAudioClips;
    static Object[] logAudioClips;

    static AudioSource hatAudioSource;
    static AudioSource playerAudioSource;
    static AudioSource logAudioSource;

    static Dictionary<HatAudioStates, AudioClip> hatAudioClipsLibrary = new Dictionary<HatAudioStates, AudioClip>();
    static Dictionary<PlayerAudioStates, AudioClip> playerAudioClipsLibrary = new Dictionary<PlayerAudioStates, AudioClip>();
    static Dictionary<LogAudioStates, AudioClip> logAudioClipsLibrary = new Dictionary<LogAudioStates, AudioClip>();
	private void Awake()
	{
        PopulateHatAudioClipsList();
        PopulateHatAudioClipsLibrary();

        PopulatePlayerAudioClipsList();
        PopulatePlayerAudioClipsLibrary();

        PopulateLogAudioClipsList();
        PopulateLogAudioClipsLibrary();
    }
	void Start()
    {
        
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

    private static void PopulateLogAudioClipsList()
    {
        logAudioClips = Resources.LoadAll("Audio/SFX/Log", typeof(AudioClip));
    }

    private static void PopulateLogAudioClipsLibrary()
    {
         logAudioClipsLibrary.Add(LogAudioStates.Rolling, (AudioClip)logAudioClips[0]);
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

    public static void PlayLogAudioClip(LogAudioStates audioState, AudioSource audio)
	{
        logAudioSource = audio;
        AudioClip clipToPlay = logAudioClipsLibrary[audioState];
        logAudioSource.PlayOneShot(clipToPlay);
    }
}
