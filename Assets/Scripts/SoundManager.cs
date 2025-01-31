using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private AudioSource soundFXPrefab;
    [SerializeField] private AudioSource musicSource;

    [Header("SFX Variables")]
    [SerializeField] private float pitchVariance;
    [SerializeField] private float scoreboardPitchStep;

    [Header("Charging Variables")]
    [SerializeField] private Vector2 chargePitchRange;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip wallCollision;
    [SerializeField] private AudioClip playerDamaged;
    [SerializeField] private AudioClip collectableCollected;
    [SerializeField] private AudioClip objectKilled;
    [SerializeField] private AudioClip chargeLoop;
    [SerializeField] private AudioClip chargeReleased;
    [SerializeField] private AudioClip buttonEnter;
    [SerializeField] private AudioClip buttonDown;
    [SerializeField] private AudioClip buttonUp;
    [SerializeField] private List<AudioClip> speedTierSounds = new List<AudioClip>();
    [SerializeField] private AudioClip cannonShot;

    [Header("Music Tracks")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip pauseMenuMusic;

    [Header("Transition")]
    [SerializeField] private AnimationCurve volumeCurve;
    [SerializeField] private float transitionDuration;
    private Coroutine transitionRoutine;

    private MusicStates currentState;

    public enum MusicStates
    {
        MainMenu,
        Gameplay,
        PauseMenu,
    }

    private AudioSource chargeSoundSource;
    private AudioSource speedTierSoundSource;

    private bool wasSetup;

    private void Start()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(0.5f) * 20f);
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(0.5f) * 20f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(0.5f) * 20f);

        PlayMusicTrack(MusicStates.MainMenu);
    }

    public void Setup()
    {
        PlayerManager.playerManager.playerController.CollisionOccured += WallCollision;
        PlayerManager.playerManager.playerCombat.DamageTaken += PlayerDamaged;
        PlayerManager.playerManager.playerController.Charging += Charging;
        PlayerManager.playerManager.playerController.ChargeEnded += ChargeEnded;
        PlayerManager.playerManager.playerController.SpeedTierChanged += SpeedTierChanged;
        PlayerManager.playerManager.playerController.CollectableCollected += CollectableCollected;
        PlayerManager.playerManager.playerController.ChargeStarted += ChargeStarted;
        PlayerManager.playerManager.playerController.CannonShot += CannonShot;

        wasSetup = true;
    }

    private void OnDestroy()
    {
        if (!wasSetup)
            return;

        PlayerManager.playerManager.playerController.CollisionOccured -= WallCollision;
        PlayerManager.playerManager.playerCombat.DamageTaken -= PlayerDamaged;
        PlayerManager.playerManager.playerController.Charging -= Charging;
        PlayerManager.playerManager.playerController.ChargeEnded -= ChargeEnded;
        PlayerManager.playerManager.playerController.SpeedTierChanged -= SpeedTierChanged;
        PlayerManager.playerManager.playerController.CollectableCollected -= CollectableCollected;
        PlayerManager.playerManager.playerController.ChargeStarted -= ChargeStarted;
        PlayerManager.playerManager.playerController.CannonShot -= CannonShot;
    }

    // Music
    public void TransitionMusic(MusicStates newState)
    {
        AudioClip newClip;
        switch (newState)
        {
            case MusicStates.MainMenu:
                newClip = mainMenuMusic;
                break;
            case MusicStates.Gameplay:
                newClip = gameplayMusic;
                break;
            case MusicStates.PauseMenu:
                newClip = pauseMenuMusic;
                break;

            default:
                newClip = gameplayMusic;
                break;
        }

        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(TransitionRoutine(newClip));
    }

    private IEnumerator TransitionRoutine(AudioClip newClip)
    {
        float currentTime = 0f;
        while (currentTime < transitionDuration)
        {
            yield return null;
            currentTime += Time.unscaledDeltaTime;

            musicSource.volume = volumeCurve.Evaluate(1 - currentTime / transitionDuration);
        }

        currentTime = 0f;
        musicSource.clip = newClip;
        musicSource.volume = 0f;
        musicSource.loop = true;
        musicSource.Play();

        while (currentTime < transitionDuration)
        {
            yield return null;
            currentTime += Time.unscaledDeltaTime;

            musicSource.volume = volumeCurve.Evaluate(currentTime / transitionDuration);
        }
    }

    private void PlayMusicTrack(MusicStates track)
    {
        currentState = track;
        switch (currentState)
        {
            case MusicStates.MainMenu:
                musicSource.clip = mainMenuMusic;
                break;
            case MusicStates.Gameplay:
                musicSource.clip = gameplayMusic;
                break;
            case MusicStates.PauseMenu:
                musicSource.clip = pauseMenuMusic;
                break;
        }

        musicSource.volume = 1f;
        musicSource.loop = true;
        musicSource.Play();
    }

    // SFX
    public void ScoreboardSound(int count)
    {
        AudioSource audioSource = Instantiate(soundFXPrefab, PlayerManager.playerManager.playerObj.transform.position, Quaternion.identity);
        audioSource.clip = objectKilled;
        audioSource.volume = 1f;
        audioSource.pitch = 1f + (count * scoreboardPitchStep);
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    private void WallCollision(Vector2 dir, Collision2D collision)
    {
        PlaySoundFXVariance(wallCollision, collision.GetContact(0).point, 1f);
    }

    private void PlayerDamaged(int damage)
    {
        PlaySoundFXVariance(playerDamaged, PlayerManager.playerManager.playerObj.transform.position, 1f);
    }

    public void CollectableCollected(Vector3 position, Collectable.CollectableTypes type)
    {
        PlaySoundFX(collectableCollected, position, 1f);
    }

    public void ObjectKilled(Vector3 position)
    {
        PlaySoundFXVariance(objectKilled, position, 1f);
    }

    private void CannonShot()
    {
        PlaySoundFX(cannonShot, PlayerManager.playerManager.playerObj.transform.position, 0.85f);
    }

    public void ChargeReleased(Vector3 position)
    {
        PlaySoundFX(chargeReleased, position, 1f);
    }

    public void ButtonSFX(ButtonAnimations.ButtonAudioStates state, Vector3 position)
    {
        switch (state)
        {
            case ButtonAnimations.ButtonAudioStates.Enter:
                PlaySoundFX(buttonEnter, position, 1f);
                break;
            case ButtonAnimations.ButtonAudioStates.Up:
                //PlaySoundFX(buttonUp, position, 1f);
                break;
            case ButtonAnimations.ButtonAudioStates.Down:
                PlaySoundFX(buttonDown, position, 1f);
                break;
        }
    }

    private void Charging(float chargeDuration)
    {
        if (chargeSoundSource == null)
        {
            chargeSoundSource = Instantiate(soundFXPrefab, PlayerManager.playerManager.playerObj.transform.position, Quaternion.identity);
            chargeSoundSource.clip = chargeLoop;
            chargeSoundSource.volume = 0.25f;
            chargeSoundSource.loop = true;
            chargeSoundSource.pitch = Mathf.Lerp(chargePitchRange.x, chargePitchRange.y, chargeDuration / PlayerManager.playerManager.playerStats.maxChargeDuration);
            chargeSoundSource.Play();
        }

        chargeSoundSource.pitch = Mathf.Lerp(chargePitchRange.x, chargePitchRange.y, chargeDuration / PlayerManager.playerManager.playerStats.maxChargeDuration);
    }

    private void SpeedTierChanged(int tier, bool isIncreasing)
    {
        if (!isIncreasing)
            return;

        if (speedTierSoundSource != null)
            Destroy(speedTierSoundSource);

        speedTierSoundSource = Instantiate(soundFXPrefab, PlayerManager.playerManager.playerObj.transform.position, Quaternion.identity);
        speedTierSoundSource.clip = speedTierSounds[tier];
        speedTierSoundSource.volume = 0.6f;
        speedTierSoundSource.Play();

        float clipLength = speedTierSoundSource.clip.length;
        Destroy(speedTierSoundSource.gameObject, clipLength);
    }

    private void ChargeStarted()
    {
        if (speedTierSoundSource != null)
            Destroy(speedTierSoundSource);

        speedTierSoundSource = Instantiate(soundFXPrefab, PlayerManager.playerManager.playerObj.transform.position, Quaternion.identity);
        speedTierSoundSource.clip = speedTierSounds[0];
        speedTierSoundSource.volume = 0.6f;
        speedTierSoundSource.Play();

        float clipLength = speedTierSoundSource.clip.length;
        Destroy(speedTierSoundSource.gameObject, clipLength);
    }

    private void ChargeEnded()
    {
        if (chargeSoundSource != null)
            Destroy(chargeSoundSource.gameObject);
    }

    // Play Sounds
    private void PlaySoundFX(AudioClip audioClip, Vector3 spawnPosition, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXPrefab, spawnPosition, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    private void PlaySoundFXVariance(AudioClip audioClip, Vector3 spawnPosition, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXPrefab, spawnPosition, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = 1.0f + Random.Range(-pitchVariance, pitchVariance);
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    // Volume Control
    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }
}
