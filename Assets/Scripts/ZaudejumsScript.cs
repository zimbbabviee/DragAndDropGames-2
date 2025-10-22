using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZaudejumsScript : MonoBehaviour
{
    public Image gameOverWindow;
    public int menuSceneIndex = 0;

    private bool gameOverShown = false;

    void Awake()
    {
        if (gameOverWindow != null)
        {
            gameOverWindow.gameObject.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverShown)
            return;

        gameOverShown = true;
        DisplayGameOverWindow();
    }

    void DisplayGameOverWindow()
    {
        if (gameOverWindow != null)
        {
            Canvas gameOverCanvas = gameOverWindow.GetComponentInParent<Canvas>();
            if (gameOverCanvas != null)
            {
                gameOverCanvas.sortingOrder = 1000;
            }

            RectTransform rectTransform = gameOverWindow.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.SetAsLastSibling();
            }

            gameOverWindow.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;

        PlainObjectSpawnScript plainObjectSpawner = FindFirstObjectByType<PlainObjectSpawnScript>();
        if (plainObjectSpawner != null)
        {
            plainObjectSpawner.StopSpawning();
        }

        FlyingObjectsControllerScript[] flyingObjects = FindObjectsByType<FlyingObjectsControllerScript>(FindObjectsSortMode.None);
        foreach (FlyingObjectsControllerScript flyingObj in flyingObjects)
        {
            flyingObj.StopMoving();
        }

        TimerScript timerScript = FindFirstObjectByType<TimerScript>();
        if (timerScript != null)
        {
            timerScript.StopTimer();
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
}
