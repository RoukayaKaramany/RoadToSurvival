using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Radio picked up!");
            Destroy(gameObject);
        }
    }
}
