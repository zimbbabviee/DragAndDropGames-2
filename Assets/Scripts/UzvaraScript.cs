using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UzvaraScript : MonoBehaviour
{
    public Image victoryWindow;

    public Sprite oneStarSprite;
    public Sprite twoStarsSprite;
    public Sprite threeStarsSprite;
    public TextMeshProUGUI timeText;

    public float threeStarTime = 60f;
    public float twoStarTime = 120f;
    public float oneStarTime = 180f;
    public int menuSceneIndex = 0;

    private float completionTime;
    private int starsEarned;
    private bool victoryShown = false;

    void Awake()
    {
        if (victoryWindow != null)
        {
            victoryWindow.gameObject.SetActive(false);
        }
    }

    public void ShowVictory(float elapsedTime)
    {
        if (victoryShown)
            return;

        victoryShown = true;
        completionTime = elapsedTime;

        CalculateStars();
        DisplayVictoryWindow();
    }

    void CalculateStars()
    {
        if (completionTime <= threeStarTime)
        {
            starsEarned = 3;
        }
        else if (completionTime <= twoStarTime)
        {
            starsEarned = 2;
        }
        else if (completionTime <= oneStarTime)
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

        if (timeText != null)
        {
            int hours = Mathf.FloorToInt(completionTime / 3600);
            int minutes = Mathf.FloorToInt((completionTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(completionTime % 60);
            timeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
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

    public float GetCompletionTime()
    {
        return completionTime;
    }
}
