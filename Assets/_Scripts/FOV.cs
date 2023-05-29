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
    [SerializeField] private AudioSource foundPlayer;
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

        // Music Config
        if (ConfigManager.instance != null)
        {
            if(ConfigManager.instance.soundV != 0)
            {
                foundPlayer.volume = ConfigManager.instance.soundV + 0.3f;
            }
            backgroundMusic.volume = ConfigManager.instance.soundV;
        }
        else
        {
            foundPlayer.volume = 0.5f;
            backgroundMusic.volume = 0.1f;
        }
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
                    if (!foundPlayer.isPlaying && hit.collider.CompareTag("Player"))
                    {
                        backgroundMusic.Pause();
                        foundPlayer.Play();

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
        /*
        if (target)
        {
            backgroundMusic.Pause();
            foundPlayer.Play();
        } 
        */
        if (!target)
        {
            foundPlayer.Stop();
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
            }

        }


        return null; // Returns null if there is no target
    }



    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    // Display field of view
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

            // Draws the field of vision
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

