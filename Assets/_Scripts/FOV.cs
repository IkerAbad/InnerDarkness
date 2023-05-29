using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    // Public variables
    public float viewRadius;
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public int segments = 36;
    public Transform target;

    // Private variables
    private EnemyController enemyController;
    [SerializeField] private AudioSource findedPlayer;
    [SerializeField] private AudioSource backgroundMusic;


    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        InvokeRepeating("FindVisibleTargets", 0, 0.2f);
    }

    void Update()
    {
        // Target detection
        target = FindVisibleTargets();
    }

    Transform FindVisibleTargets()
    {
        EnemyController enemyController = GetComponent<EnemyController>();
        Transform nextWaypoint = enemyController.GetNextWaypoint();

        if (nextWaypoint != null)
        {
            Vector3 direction = (nextWaypoint.position - transform.position).normalized;

            float startAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - viewAngle / 2;
            float angleStep = viewAngle / segments;

            for (int i = 0; i <= segments; i++)
            {
                float angle = startAngle + i * angleStep;
                Vector3 dir = DirFromAngle(angle, true);
                
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);

                if (hit.collider != null)
                {
                    if (!findedPlayer.isPlaying && hit.collider.CompareTag("Player"))
                    {
                        backgroundMusic.Pause();
                        findedPlayer.Play();

                        enemyController.alreadySeen = true;
                    }

                    // Check if raycast has hit a target
                    if (((1 << hit.collider.gameObject.layer) & targetMask) != 0)
                    {
                        //Debug.Log("Target found: " + hit.collider.gameObject.name);
                        return hit.collider.transform; // returns found target
                    }
                }
            }
        }

        if (!backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
            findedPlayer.Stop();
        }

        return null; // Devolver null si no se ha encontrado ning�n objetivo
    }



    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    // Para visualizar el campo de visi�n
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        EnemyController enemyController = GetComponent<EnemyController>();
        Transform nextWaypoint = enemyController.GetNextWaypoint();

        if (nextWaypoint != null)
        {
            Vector3 direction = (nextWaypoint.position - transform.position).normalized;

            float startAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - viewAngle / 2;
            float endAngle = startAngle + viewAngle;

            Vector3 startDir = DirFromAngle(startAngle, true);
            Vector3 endDir = DirFromAngle(endAngle, true);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + startDir * viewRadius);
            Gizmos.DrawLine(transform.position, transform.position + endDir * viewRadius);

            // Dibujar el arco del campo de visi�n
            Gizmos.color = Color.green;
            float angleStep = viewAngle / segments;
            for (int i = 0; i <= segments; i++)
            {
                float angle = startAngle + i * angleStep;
                Vector3 dir = DirFromAngle(angle, true);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);
                if (hit.collider != null)
                {
                    Gizmos.DrawLine(transform.position, hit.point);
                }
                else
                {
                    Gizmos.DrawLine(transform.position, transform.position + dir * viewRadius);
                }
            }
        }
    }
}

