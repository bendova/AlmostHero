using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    public enum EGameState
    {
        STOPPED,
        PAUSED,
        RUNNING,
        WON,
        LOST
    }

    [HideInInspector]
    public static GameManager s_instance;

    // Inspector variables
    public float m_timePerRoundInSeconds = 300f;


    // Private variables
    private EGameState m_gameState = EGameState.STOPPED;
    private float m_gameTimeRemaining = 0f;

    GameManager()
    {
        s_instance = this;
    }

	// Use this for initialization
	void Start ()
    {
        StartGame();
	}

	// Update is called once per frame
	void Update () 
    {
		if(m_gameState == EGameState.RUNNING)
        {
            CheckCheats();

            m_gameTimeRemaining -= Time.deltaTime;
            if(m_gameTimeRemaining <= 0f)
            {
                TriggerGameWon();
            }

            UiManager.s_instance.ShowGameTime(m_gameTimeRemaining);
        }

        if ((m_gameState == EGameState.WON) || (m_gameState == EGameState.LOST))
        {
            if(Input.GetButtonUp("Restart"))
            {
                RestartGame();
            }
        }
	}

    private void CheckCheats()
    {
        if (Input.GetButtonUp("Cheat_Lose"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().DieNow();
        }
        else if (Input.GetButtonUp("Cheat_Win"))
        {
            m_gameTimeRemaining = 0f;
        }
    }

    private void StartGame()
    {
        m_gameState = EGameState.RUNNING;
        m_gameTimeRemaining = m_timePerRoundInSeconds;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void TriggerGameWon()
    {
        m_gameTimeRemaining = 0f;
        m_gameState = EGameState.WON;
        UiManager.s_instance.ShowGameWon();
    }

    private void TriggerGameLost()
    {
        m_gameState = EGameState.LOST;
        UiManager.s_instance.ShowGameLost();
    }

    public bool IsGameRunning()
    {
        return (m_gameState == EGameState.RUNNING);
    }

    public void OnPlayerDied()
    {
        if(m_gameState == EGameState.RUNNING)
        {
            TriggerGameLost();
        }
    }
}
