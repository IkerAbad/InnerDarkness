using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    void Start()
    {
        EnemyController[] enemyControllers = GetComponentsInChildren<EnemyController>();
        foreach (EnemyController enemyController in enemyControllers)
        {
            Transform[] enemyPath = enemyController.enemyPath;
            if (enemyPath.Length > 0)
            {
                enemyController.SetTarget(enemyPath[0]);
            }
        }
    }
}
