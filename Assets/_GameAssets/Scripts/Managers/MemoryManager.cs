using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryManager : MonoBehaviour
{
    [SerializeField] private List<Button> memoryButtons;
    [SerializeField] private Color highlightColor = Color.white;
    [SerializeField] private float highlightDuration = 0.5f;
    [SerializeField] private float delayBetweenHighlights = 0.3f;
    [SerializeField] private float delayBetweenRounds = 1.5f;

    private List<int> sequence = new List<int>();
    private int playerProgress = 0;
    private bool playerTurn = false;
    private bool gameOver = false;

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
            else
            {
                StartCoroutine(StartGame());
            }
        }
    }

    private IEnumerator StartGame()
    {
        gameOver = false;
        sequence.Clear();
        yield return new WaitForSeconds(1f);
        AddRandomToSequence();
        yield return ShowSequence();
        playerTurn = true;
    }

    private void AddRandomToSequence()
    {
        int randomIndex = Random.Range(0, memoryButtons.Count);
        sequence.Add(randomIndex);
    }

    private IEnumerator ShowSequence()
    {
        playerTurn = false;

        for (int i = 0; i < sequence.Count; i++)
        {
            int index = sequence[i];
            yield return HighlightButton(index);
            yield return new WaitForSeconds(delayBetweenHighlights);
        }

        playerProgress = 0;
        playerTurn = true;
    }

    private IEnumerator HighlightButton(int index)
    {
        Image img = memoryButtons[index].GetComponent<Image>();
        Color originalColor = img.color;

        img.color = highlightColor;
        
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonHighlight(index);
        }
        
        yield return new WaitForSeconds(highlightDuration);
        img.color = originalColor;
    }

    private IEnumerator NextRound()
    {
        yield return new WaitForSeconds(delayBetweenRounds);
        
        AddRandomToSequence();
        yield return ShowSequence();
    }

    public void OnButtonClick(int index)
    {
        if (!playerTurn || gameOver) return;

        if (index < 0 || index >= memoryButtons.Count)
        {
            Debug.LogError($"Invalid button index: {index}. Button count: {memoryButtons.Count}");
            return;
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonClick(index);
        }

        if (sequence[playerProgress] == index)
        {
            playerProgress++;
            
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayCorrectSound();
            }
            
            if (playerProgress >= sequence.Count)
            {
                playerTurn = false;
                
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnRoundCompleted();
                }
                
                StartCoroutine(NextRound());
            }
        }
        else
        {
            gameOver = true;
            playerTurn = false;
            
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayWrongSound();
                SoundManager.Instance.PlayGameOverSound();
            }
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver(sequence.Count - 1);
            }
            
            Debug.Log($"Wrong! Game over. Your score: {sequence.Count - 1}. Press 'R' to restart.");
        }
    }

    public void StartNewGame()
    {
        StartCoroutine(StartGame());
    }

    public void RepeatCurrentSequence()
    {
        if (!gameOver && sequence.Count > 0)
        {
            Debug.Log("Repeating sequence...");
            StartCoroutine(ShowSequence());
        }
    }

    public int GetCurrentScore()
    {
        return sequence.Count - 1;
    }

    public int GetCurrentRound()
    {
        return sequence.Count;
    }

    public bool IsPlayerTurn()
    {
        return playerTurn;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }
}
