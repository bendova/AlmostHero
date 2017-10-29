using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour 
{
    [HideInInspector]
    public static UiManager s_instance;

    public Text m_timeRemainingText;
    public Text m_youWonText;
    public Text m_youLostText;
    public Text m_restartText;

    UiManager()
    {
        s_instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        m_timeRemainingText.text = "";
        m_youWonText.gameObject.SetActive(false);
        m_youLostText.gameObject.SetActive(false);
        m_restartText.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update() 
    {
		
	}

    public void ShowGameTime(float timeInSeconds)
    {
        var timespan = TimeSpan.FromSeconds(timeInSeconds);
        int minutes = (int)(timeInSeconds / 60f);
        int seconds = (int)(timeInSeconds % 60f);
        string minutesStr = minutes.ToString().PadLeft(2, '0');
        string secondsStr = seconds.ToString().PadLeft(2, '0');
        m_timeRemainingText.text = string.Format("Time until dawn: {0}:{1}", minutesStr, secondsStr);
    }

    public void ShowGameWon()
    {
        m_youWonText.gameObject.SetActive(true);
        m_restartText.gameObject.SetActive(true);
    }

    public void ShowGameLost()
    {
        m_youLostText.gameObject.SetActive(true);
        m_restartText.gameObject.SetActive(true);
    }
}
