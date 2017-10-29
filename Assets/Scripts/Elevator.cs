using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour 
{
    public Transform m_bottomPos;
    public Transform m_topPos;
    public float m_elevateSpeed = 0.1f; // Units per second
    public bool m_isDown = true;

    private Transform m_transform;
    private Rigidbody2D m_rigidBody;
    private bool m_isActive = false;

	// Use this for initialization
	void Start()
    {
        m_transform = GetComponent<Transform>();
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_transform.position = m_isDown ? m_bottomPos.position : m_topPos.position;
	}
	
	// Update is called once per frame
	void Update() 
    {
        if(m_isActive)
        {
            if(m_isDown)
            {
                if (m_transform.position.y >= m_topPos.position.y)
                {
                    m_transform.position = m_topPos.position;
                    m_isDown = false;
                    m_isActive = false;
                }
            }
            else
            {
                if (m_transform.position.y <= m_bottomPos.position.y)
                {
                    m_transform.position = m_bottomPos.position;
                    m_isDown = true;
                    m_isActive = false;
                }
            }

            if(m_isActive)
            {
                float speedY = m_isDown ? m_elevateSpeed : -m_elevateSpeed;
                m_rigidBody.velocity = new Vector2(0, speedY);
            }
            else 
            {
                m_rigidBody.velocity = Vector2.zero;
            }
        }
	}

    public void Activate()
    {
        m_isActive = true;
    }
}
