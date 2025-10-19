using UnityEngine;
using System.Collections;
using TMPro;
public class TimerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    private float elapsedTime;
    private bool timerRunning = true;

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerRunning = true;
    }
}