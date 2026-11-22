using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject currentCheckpoint;

    void Start()
    {
        currentCheckpoint = null;
    }

    public void RespawnPlayer()
    {
        FindObjectOfType<PlayerController>().transform.position = currentCheckpoint.transform.position;
        FindObjectOfType<PlayerStats>().ComeBackToLife();
    }
}
