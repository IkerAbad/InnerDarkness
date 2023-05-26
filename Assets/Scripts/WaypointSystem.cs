using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    // Update is called once per frame
    void Update()
    {
        if (AllEnemiesReady())
        {
            foreach (GameObject enemyPrefab in enemyPrefabs)
            {
                EnemyController enemyController = enemyPrefab.GetComponent<EnemyController>();
                if (enemyController.currentTarget == null)
                    enemyController.SetTarget(enemyController.enemyPath[0]);

                CheckTargets(enemyController);
            }
        }
    }

    private void CheckTargets(EnemyController enemyController)
    {
        //Debug.Log(enemyController.enemyPath.Length);

        if (Vector2.Distance(enemyController.transform.position, enemyController.currentTarget.position) < 0.01f)
        {
            int currentIndex = Array.IndexOf(enemyController.enemyPath, enemyController.currentTarget);
            Debug.Log(currentIndex);
            if (currentIndex < enemyController.enemyPath.Length - 1)
            {
                Debug.Log("next");
                enemyController.SetTarget(enemyController.enemyPath[currentIndex + 1]);
            }
            else
            {
                Debug.Log("0");
                Array.Reverse(enemyController.enemyPath);
                enemyController.SetTarget(enemyController.enemyPath[0]);
            }
        }
    }

    private bool AllEnemiesReady()
    {
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            EnemyController enemyController = enemyPrefab.GetComponent<EnemyController>();
            if(!enemyController.ready)
            {
                return false;
            }
        }
        return true;
    }
}
