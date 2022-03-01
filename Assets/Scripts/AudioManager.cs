using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HatAudioStates { Deposit, Destroyed, Collected };
public enum PlayerAudioStates { Damaged};
public enum LogAudioStates { Rolling, Shaking };
public enum MushroomManAudioStates {Falling, Landed, Collected };
public enum GameGeneralAudioStates {RoundBegin, RoundEnd, HatRushBegin }
//public enum GeneralGameAudioStates { };
public class AudioManager : MonoBehaviour
{
    static Object[] hatAudioClips;
    static Object[] playerAudioClips;
    static Object[] logAudioClips;
    static Object[] mushroomManAudioClips;
    static Object[] gameGeneralAudioClips;

    static AudioSource hatAudioSource;
    static AudioSource playerAudioSource;
    static AudioSource logAudioSource;
    static AudioSource mushroomManAudioSource;
    static AudioSource gameGeneralSFXAudioSource;

    static Dictionary<HatAudioStates, AudioClip> hatAudioClipsLibrary = new Dictionary<HatAudioStates, AudioClip>();
    static Dictionary<PlayerAudioStates, AudioClip> playerAudioClipsLibrary = new Dictionary<PlayerAudioStates, AudioClip>();
    static Dictionary<LogAudioStates, AudioClip> logAudioClipsLibrary = new Dictionary<LogAudioStates, AudioClip>();
	static Dictionary<MushroomManAudioStates, AudioClip> mushroomManAudioClipsLibrary = new Dictionary<MushroomManAudioStates, AudioClip>();
    static Dictionary<GameGeneralAudioStates, AudioClip> gameGeneralAudioClipsLibrary = new Dictionary<GameGeneralAudioStates, AudioClip>();

    private void Awake()
	{
        PopulateHatAudioClipsList();
        PopulateHatAudioClipsLibrary();

        PopulatePlayerAudioClipsList();
        PopulatePlayerAudioClipsLibrary();

        PopulateLogAudioClipsList();
        PopulateLogAudioClipsLibrary();

        PopulateMushroomManAudioClipsList();
        PopulateMushroomManAudioClipsLibrary();

        PopulateGameGeneralAudioClipsList();
        PopulateGameGeneralAudioClipsLibrary();

        gameGeneralSFXAudioSource = gameObject.GetComponent<AudioSource>();

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
        hatAudioClipsLibrary.Add(HatAudioStates.Deposit, (AudioClip)hatAudioClips[0]);
        hatAudioClipsLibrary.Add(HatAudioStates.Destroyed, (AudioClip)hatAudioClips[1]);
        hatAudioClipsLibrary.Add(HatAudioStates.Collected, (AudioClip)hatAudioClips[2]);
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
        logAudioClipsLibrary.Add(LogAudioStates.Shaking, (AudioClip)logAudioClips[1]);
    }

    private static void PopulateMushroomManAudioClipsList()
    {
        mushroomManAudioClips = Resources.LoadAll("Audio/SFX/MushroomMan", typeof(AudioClip));
    }

    private static void PopulateMushroomManAudioClipsLibrary()
    {
        mushroomManAudioClipsLibrary.Add(MushroomManAudioStates.Falling, (AudioClip)mushroomManAudioClips[0]);
        mushroomManAudioClipsLibrary.Add(MushroomManAudioStates.Landed, (AudioClip)mushroomManAudioClips[1]);
        mushroomManAudioClipsLibrary.Add(MushroomManAudioStates.Collected, (AudioClip)mushroomManAudioClips[1]);
    }

    private static void PopulateGameGeneralAudioClipsList()
    {
        gameGeneralAudioClips = Resources.LoadAll("Audio/SFX/GameGeneral", typeof(AudioClip));
    }

    private static void PopulateGameGeneralAudioClipsLibrary()
    {
        gameGeneralAudioClipsLibrary.Add(GameGeneralAudioStates.RoundBegin, (AudioClip)gameGeneralAudioClips[0]);
		gameGeneralAudioClipsLibrary.Add(GameGeneralAudioStates.RoundEnd, (AudioClip)gameGeneralAudioClips[1]);
        gameGeneralAudioClipsLibrary.Add(GameGeneralAudioStates.HatRushBegin, (AudioClip)gameGeneralAudioClips[2]);
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

    public static void PlayMushroomManAudioClip(MushroomManAudioStates audioState, AudioSource audio)
    {
        mushroomManAudioSource = audio;
        AudioClip clipToPlay = mushroomManAudioClipsLibrary[audioState];
        mushroomManAudioSource.PlayOneShot(clipToPlay);
    }
    public static void PlayGeneralGameAudioClip(GameGeneralAudioStates audioState)
    {
        AudioClip clipToPlay = gameGeneralAudioClipsLibrary[audioState];
        gameGeneralSFXAudioSource.PlayOneShot(clipToPlay);
    }

}