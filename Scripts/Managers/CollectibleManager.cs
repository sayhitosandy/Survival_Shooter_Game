using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour {

    public PlayerHealth playerHealth;
    public GameObject CollectibleType;

    public Transform[] spawnPoints;
    int count = 50;

    void Start()
    {
        CollectibleType.SetActive(false);
    }

    void Update()
    {
        if (ScoreManager.score >= count)
        {
            Spawn();
        }
    }

    void Spawn ()
    {
        
        if(playerHealth.currentHealth <= 0f)
        {
            return;
        }

        count += 50;
        Vector3 spawnPointPosition = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
        CollectibleType.transform.position = spawnPointPosition;    
        CollectibleType.SetActive(true);

/*        if (ScoreManager.score > 0 && ScoreManager.score % 50 == 0)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Vector3 spawnPointPosition = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
            Instantiate(CollectibleType, spawnPointPosition, gameObject.transform.rotation);
            isSpawned = true;
        }
        else
        {
            isSpawned = false;
        }
 */
    }
}
