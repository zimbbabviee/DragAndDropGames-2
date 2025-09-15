using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TimerScript : MonoBehaviour
{
    private int sec = 0;
    private int min = 0;
    private TMP_Text _TimerText;
    [SerializeField] private int delta = 0;

    void Start()
    {
        _TimerText = GameObject.Find("TimerText").GetComponent<TMP_Text>();
        StartCoroutine(ITimer());
    }
    IEnumerator ITimer()
    {
        while(true)
        {
            if(sec == 59)
            {
                min++;
                sec = -1;
            }
            sec += delta;
            _TimerText.text = min.ToString("D2") + " : " + sec.ToString("D2");
            yield return new WaitForSeconds(1);

        }
    }
}