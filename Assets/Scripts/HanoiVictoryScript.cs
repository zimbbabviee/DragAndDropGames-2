using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HanoiVictoryScript : MonoBehaviour
{
    public Image victoryWindow;

    public Sprite oneStarSprite;
    public Sprite twoStarsSprite;
    public Sprite threeStarsSprite;
    public TextMeshProUGUI movesText;

    public int threeStarMoves = 63;
    public int twoStarMoves = 80;
    public int oneStarMoves = 120;
    public int menuSceneIndex = 0;

    private int completionMoves;
    private int starsEarned;
    private bool victoryShown = false;

    void Awake()
    {
        if (victoryWindow != null)
        {
            victoryWindow.gameObject.SetActive(false);
        }
    }

    public void ShowVictory(int totalMoves)
    {
        if (victoryShown)
            return;

        victoryShown = true;
        completionMoves = totalMoves;

        CalculateStars();
        DisplayVictoryWindow();
    }

    void CalculateStars()
    {
        if (completionMoves <= threeStarMoves)
        {
            starsEarned = 3;
        }
        else if (completionMoves <= twoStarMoves)
        {
            starsEarned = 2;
        }
        else if (completionMoves <= oneStarMoves)
        {
            starsEarned = 1;
        }
        else
        {
            starsEarned = 1;
        }
    }

    void DisplayVictoryWindow()
    {
        if (victoryWindow != null)
        {
            switch (starsEarned)
            {
                case 3:
                    victoryWindow.sprite = threeStarsSprite;
                    break;
                case 2:
                    victoryWindow.sprite = twoStarsSprite;
                    break;
                case 1:
                    victoryWindow.sprite = oneStarSprite;
                    break;
            }

            Canvas victoryCanvas = victoryWindow.GetComponentInParent<Canvas>();
            if (victoryCanvas != null)
            {
                victoryCanvas.sortingOrder = 1000;
            }

            RectTransform rectTransform = victoryWindow.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.SetAsLastSibling();
            }

            victoryWindow.gameObject.SetActive(true);
        }

        if (movesText != null)
        {
            movesText.text = completionMoves.ToString();
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneIndex);
    }

    public int GetStarsEarned()
    {
        return starsEarned;
    }

    public int GetCompletionMoves()
    {
        return completionMoves;
    }
}
