using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private AudioSource soundFXPrefab;

    [Header("SFX Variables")]
    [SerializeField] private float pitchVariance;

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

    private AudioSource chargeSoundSource;

    private void Start()
    {
        PlayerManager.playerManager.playerController.CollisionOccured += WallCollision;
        PlayerManager.playerManager.playerCombat.DamageTaken += PlayerDamaged;
        PlayerManager.playerManager.playerController.Charging += Charging;
        PlayerManager.playerManager.playerController.ChargeEnded += ChargeEnded;

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(0.5f) * 20f);
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(0.5f) * 20f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(0.5f) * 20f);
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerController.CollisionOccured -= WallCollision;
        PlayerManager.playerManager.playerCombat.DamageTaken -= PlayerDamaged;
        PlayerManager.playerManager.playerController.Charging -= Charging;
        PlayerManager.playerManager.playerController.ChargeEnded -= ChargeEnded;
    }

    private void WallCollision(Vector2 dir, Collision2D collision)
    {
        PlaySoundFXVariance(wallCollision, collision.GetContact(0).point, 1f);
    }

    private void PlayerDamaged(int damage)
    {
        //PlaySoundFXVariance(playerDamaged, PlayerManager.playerManager.playerObj.transform.position, 1f);
    }

    private void CollectableCollected(Vector3 position, Collectable.CollectableTypes type)
    {
        //PlaySoundFXVariance(collectableCollected, position, 1f);
    }

    public void ObjectKilled(Vector3 position)
    {
        //PlaySoundFXVariance(objectKilled, position, 1f);
    }

    public void ChargeReleased(Vector3 position)
    {
        //PlaySoundFX(chargeReleased, position, 1f);
    }

    public void ButtonSFX(ButtonAnimations.ButtonAudioStates state, Vector3 position)
    {
        /*
        switch (state)
        {
            case ButtonAnimations.ButtonAudioStates.Enter:
                PlaySoundFX(buttonEnter, position, 1f);
                break;
            case ButtonAnimations.ButtonAudioStates.Up:
                PlaySoundFX(buttonUp, position, 1f);
                break;
            case ButtonAnimations.ButtonAudioStates.Down:
                PlaySoundFX(buttonDown, position, 1f);
                break;
        }
        */
    }

    private void Charging(float chargeDuration)
    {
        /*
        if (chargeSoundSource == null)
        {
            chargeSoundSource = Instantiate(soundFXPrefab, PlayerManager.playerManager.playerObj.transform.position, Quaternion.identity);
            chargeSoundSource.clip = chargeLoop;
            chargeSoundSource.volume = 1f;
            chargeSoundSource.loop = true;
            chargeSoundSource.pitch = Mathf.Lerp(chargePitchRange.x, chargePitchRange.y, chargeDuration / PlayerManager.playerManager.playerStats.maxChargeDuration);
            chargeSoundSource.Play();
        }

        chargeSoundSource.pitch = Mathf.Lerp(chargePitchRange.x, chargePitchRange.y, chargeDuration / PlayerManager.playerManager.playerStats.maxChargeDuration);
        */
    }

    private void ChargeEnded()
    {
        /*
        if (chargeSoundSource != null)
            Destroy(chargeSoundSource.gameObject);
        */
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
