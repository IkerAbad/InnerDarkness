using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Variables
    public float walkingSpeed = 3f;
    public Animator animator;
    public AudioClip enemyWalkingAudio;
    public bool isPlaySound = false;

    public Transform[] enemyPath;

    public float visionRadius; //Longitud
    public float visionAngle; //Angulo de visión

    private AudioSource audioSource;
    private Rigidbody2D enemyRigidbody;
    private LineRenderer lineRenderer;

    public Transform currentTarget;
    public bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get Rigidbody
        enemyRigidbody = GetComponent<Rigidbody2D>();

        // Get AudioSource
        audioSource = GetComponent<AudioSource>();
        if (ConfigManager.instance != null)
        {
            audioSource.volume = ConfigManager.instance.sfxV;
        }
        // Get LineRenderer
        lineRenderer = GetComponent<LineRenderer>();

        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused && currentTarget)
        {
            //Vector2 direction = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, walkingSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    // Movement Animation
    void AnimateMovement(Vector3 direction)
    {

    }

    // Function Walking Sound
    private void WalkSound()
    {
        if (enemyWalkingAudio != null && !isPlaySound)
        {
            StartCoroutine(PlaySound());
        }
    }

    // Coroutine -> Walking SoundTrack
    IEnumerator PlaySound()
    {
        AudioSource.PlayClipAtPoint(enemyWalkingAudio, transform.position, 1);
        isPlaySound = true;
        yield return new WaitForSeconds(enemyWalkingAudio.length / audioSource.pitch);
        isPlaySound = false;
    }

    private void OnDrawGizmos()
    {
        if (currentTarget)
        {
            Gizmos.color = Color.red;

            // Calcula los ángulos de inicio y fin del cono de visión
            float halfAngle = visionAngle / 2;
            float startAngle = -halfAngle;
            float endAngle = +halfAngle;

            Debug.Log("startAngle -> " + startAngle);
            Debug.Log("endAngle -> " + endAngle);

            Vector2 direction;
            if(currentTarget != null)
            {
                direction = (currentTarget.position - transform.position);
            } else
            {
                direction = transform.up;
            }


            // Convierte los ángulos a radianes
            startAngle *= Mathf.Deg2Rad;
            endAngle *= Mathf.Deg2Rad;

            Debug.Log("startAngle -> " + startAngle);
            Debug.Log("endAngle -> " + endAngle);

            // Calcula los vectores de inicio y fin del cono de visión
            Vector2 startPoint = transform.position + (Quaternion.Euler(0, 0, startAngle) * direction) * visionRadius;
            Vector2 endPoint = transform.position + (Quaternion.Euler(0, 0, endAngle) * direction) * visionRadius;

            Gizmos.DrawLine(transform.position, startPoint);
            Gizmos.DrawLine(transform.position, endPoint);
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}
