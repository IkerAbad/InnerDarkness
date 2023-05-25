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

    private AudioSource audioSource;
    private Rigidbody2D enemyRigidbody;

    private float horizontal = 0;
    private float vertical = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get Rigidbody
        enemyRigidbody = GetComponent<Rigidbody2D>();

        // Get AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            // Enemy Movement

            // Enemy Animations
        }
    }

    // Movement Animation
    void AnimateMovement(Vector3 direction)
    {
        if (animator != null)
        {
            if (horizontal != 0 || vertical != 0)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);

                WalkSound();
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
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
}
