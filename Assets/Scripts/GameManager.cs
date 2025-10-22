using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] dropPlaces;
    public UzvaraScript uzvaraScript;
    public TimerScript timerScript;

    private int totalPlaces;
    private int placedCorrectly = 0;
    private bool gameCompleted = false;

    void Start()
    {

        if (dropPlaces == null || dropPlaces.Length == 0)
        {
            return;
        }

        totalPlaces = dropPlaces.Length;

        if (uzvaraScript == null)
        {
            uzvaraScript = FindFirstObjectByType<UzvaraScript>();
        }

        if (timerScript == null)
        {
            timerScript = FindFirstObjectByType<TimerScript>();
        }
    }

    public void OnVehiclePlaced()
    {

        if (gameCompleted)
        {
            return;
        }

        placedCorrectly++;
        if (placedCorrectly >= totalPlaces)
        {
            CompleteGame();
        }
    }

    void CompleteGame()
    {
        if (gameCompleted)
        {
            return;
        }

        gameCompleted = true;

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

        if (timerScript != null)
        {
            float elapsedTime = timerScript.GetElapsedTime();
            timerScript.StopTimer();
            if (uzvaraScript != null)
            {
                uzvaraScript.ShowVictory(elapsedTime);
            }
        }
        else
        {
            if (uzvaraScript != null)
            {
                uzvaraScript.ShowVictory(0f);
            }
        }

    }

    public void ResetGame()
    {
        placedCorrectly = 0;
        gameCompleted = false;

        if (timerScript != null)
        {
            timerScript.ResetTimer();
        }
    }

    public int GetPlacedCount()
    {
        return placedCorrectly;
    }

    public int GetTotalPlaces()
    {
        return totalPlaces;
    }

    public bool IsGameCompleted()
    {
        return gameCompleted;
    }
}
