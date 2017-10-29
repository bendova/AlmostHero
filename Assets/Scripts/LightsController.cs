using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsController : MonoBehaviour 
{
    public int m_maxActiveLights = 4;
    public GameObject[] m_lights;
    public float m_nearLightDistance = 30f;
    public float m_mediumLightDistance = 60f;
    public float m_farLightDistance = 90f;
    public float m_lightChangeInterval = 20f; // seconds
    public float m_minDistanceBetweenLights = 10f;

    private float m_currentTime = 0f;

	// Use this for initialization
	void Start() 
    {
        ToggleLights();
	}

	// Update is called once per frame
	void Update()
    {
        m_currentTime += Time.deltaTime;
        if(m_currentTime >= m_lightChangeInterval)
        {
            ToggleLights();
            m_currentTime = 0.0f;
        }
	}

    private void DeactivateAllLights()
    {
        for (int i = 0; i < m_lights.Length; ++i)
        {
            m_lights[i].SetActive(false);
        }
    }

    private void ToggleLights()
    {
        DeactivateAllLights();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            float distance = m_nearLightDistance;
            List<GameObject> listToActivate = FindRandomLightsNear(player.transform.position, distance);
            foreach (GameObject light in listToActivate)
            {
                //Debug.Log("ToggleLights() activating light: " + light.name);
                light.SetActive(true);
            }
        }
        else 
        {
            Debug.LogError("ToggleLights() could not find Player");
        }
    }

    private List<GameObject> FindRandomLightsNear(Vector3 position, float distance)
    {
        List<GameObject> selectedLights = new List<GameObject>();
        List<GameObject> otherLights = new List<GameObject>();
        foreach(GameObject light in m_lights)
        {
            if(light == null)
            {
                continue;
            }

            bool wasSelected = false;
            if (selectedLights.Count < m_maxActiveLights)
            {
                if (Vector3.Distance(light.transform.position, position) >= distance)
                {
                    if (CoinFlip() && CanSelectLight(selectedLights, light))
                    {
                        selectedLights.Add(light);
                        wasSelected = true;
                    }
                }
            }

            if(!wasSelected)
            {
                otherLights.Add(light);
            }
        }

        if (selectedLights.Count < m_maxActiveLights)
        {
            foreach (GameObject light in otherLights)
            {
                if (CoinFlip())
                {
                    selectedLights.Add(light);
                }
                if (selectedLights.Count == m_maxActiveLights)
                {
                    break;
                }
            }
        }

        return selectedLights;
    }

    private bool CoinFlip()
    {
        return (Random.value < 0.5f);
    }

    private bool CanSelectLight(List<GameObject> selectedLights, GameObject light)
    {
        bool canSelect = true;
        foreach (GameObject it in selectedLights)
        {
            if(Vector3.Distance(it.transform.position, light.transform.position) <= m_minDistanceBetweenLights)
            {
                canSelect = false;
                break;
            }
        }
        return canSelect;
    }
}
