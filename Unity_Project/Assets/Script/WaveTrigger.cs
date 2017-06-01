using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    EnemiesManager enemiesManager;
    
    void Start()
    {
        enemiesManager = FindObjectOfType<EnemiesManager>();
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 12)
        {
            enemiesManager.waitCount++;
            Destroy(gameObject);
        }
    }
}
