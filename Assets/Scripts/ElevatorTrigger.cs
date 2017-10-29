using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour 
{
    public Elevator m_elevator;

    private bool m_isPlayerInBounds = false;

	// Use this for initialization
	void Start() 
    {
		
	}
	
	// Update is called once per frame
	void Update() 
    {
		if(m_isPlayerInBounds)
        {
            if(Input.GetButtonUp("Use"))
            {
                Debug.Log("Update() activating elevator");

                m_elevator.Activate();
            }
        }
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("OnTriggerEnter2D");
            m_isPlayerInBounds = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("OnTriggerExit2D");
            m_isPlayerInBounds = false;
        }
    }
}
