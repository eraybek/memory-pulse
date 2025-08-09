using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Button Sounds")]
    [SerializeField] private AudioClip buttonHighlightSound; 
    [SerializeField] private AudioClip buttonClickSound; 
    
    [Header("Game Sounds")]
    [SerializeField] private AudioClip correctSound; 
    [SerializeField] private AudioClip wrongSound; 
    [SerializeField] private AudioClip gameOverSound; 

    [Header("Audio Settings")]
    [SerializeField] private float soundVolume = 1f;
    
    private AudioSource audioSource;
    
    public static SoundManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = soundVolume;
    }
    
    public void PlayButtonHighlight(int buttonIndex)
    {
        if (buttonHighlightSound != null)
        {
            audioSource.PlayOneShot(buttonHighlightSound, soundVolume);
        }
    }
    
    public void PlayButtonClick(int buttonIndex)
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound, soundVolume);
        }
    }
    
    public void PlayCorrectSound()
    {
        if (correctSound != null)
        {
            audioSource.PlayOneShot(correctSound, soundVolume);
        }
    }
    
    public void PlayWrongSound()
    {
        if (wrongSound != null)
        {
            audioSource.PlayOneShot(wrongSound, soundVolume);
        }
    }
    
    public void PlayGameOverSound()
    {
        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound, soundVolume);
        }
    }
    
    public void SetVolume(float volume)
    {
        soundVolume = Mathf.Clamp01(volume);
        if (audioSource != null)
        {
            audioSource.volume = soundVolume;
        }
    }
}
