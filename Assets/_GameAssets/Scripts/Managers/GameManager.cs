using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreNumberText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button repeatButton;
    
    [Header("Game References")]
    [SerializeField] private MemoryManager memoryManager;
    
    private int currentScore = 0;
    private int currentRound = 1;
    private bool gameStarted = false;
    
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        SetupUI();
        SetupButtons();
    }
    
    private void SetupUI()
    {
        UpdateScoreUI();
        
        if (repeatButton != null)
            repeatButton.interactable = false;
    }
    
    private void SetupButtons()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        
        if (repeatButton != null)
        {
            repeatButton.onClick.AddListener(RepeatSequence);
        }
    }
    
    public void StartGame()
    {
        gameStarted = true;
        currentScore = 0;
        
        UpdateScoreUI();
        
        if (startButton != null)
            startButton.interactable = false;
        
        if (repeatButton != null)
            repeatButton.interactable = true;
        
        if (memoryManager != null)
        {
            memoryManager.StartNewGame();
        }
    }
    
    public void RepeatSequence()
    {
        if (memoryManager != null && gameStarted)
        {
            memoryManager.RepeatCurrentSequence();
        }
    }
    
    public void OnRoundCompleted()
    {
        if (!gameStarted) return;
        
        currentScore++;
        currentRound++;
        
        Debug.Log($"Round completed! +1 point. Total Score: {currentScore}");
        
        UpdateScoreUI();
    }
    
    public void OnGameOver(int finalScore)
    {
        gameStarted = false;
        
        if (startButton != null)
            startButton.interactable = true;
        
        if (repeatButton != null)
            repeatButton.interactable = false;
        
        Debug.Log($"Game Over! Final Score: {finalScore}");
    }
    
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score";
        
        if (scoreNumberText != null)
        {
            scoreNumberText.text = currentScore.ToString();
            Debug.Log($"Score UI updated: {currentScore}");
        }
        else
        {
            Debug.LogError("scoreNumberText reference is null! Not assigned in Inspector.");
        }
    }
    
    public int CurrentScore => currentScore;
    public bool IsGameStarted => gameStarted;
}
