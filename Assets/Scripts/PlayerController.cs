using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables
    public float walkingSpeed = 4f;
    public Animator animator;
    public AudioClip playerWalkingAudio;
    public float soundSpeedForWalking = 3.5f;
    public bool isPlaySound = false;

    private Rigidbody2D playerRigidbody;
    private Vector3 direction;
    private AudioSource audioSource;

    private float horizontal = 0;
    private float vertical = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get Rigidbody
        playerRigidbody = GetComponent<Rigidbody2D>();

        // Get AudioSource
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = soundSpeedForWalking;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            // Player Inputs
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            // Player direction in a 2d plane
            direction = new Vector3(horizontal, vertical, 0);

            // Player Animations
            AnimateMovement(direction);
        }
    }

    //
    void FixedUpdate()
    {
        // Player Movement
        transform.position += walkingSpeed * Time.deltaTime * direction.normalized;
    }

    // Movement Animation
    void AnimateMovement(Vector3 direction)
    {
        if(animator != null)
        {
            if(horizontal != 0 || vertical !=0)
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
        if (playerWalkingAudio != null && !isPlaySound)
        {
            StartCoroutine(PlaySound());
        }
    }

    // Coroutine -> Walking SoundTrack
    IEnumerator PlaySound()
    {
        AudioSource.PlayClipAtPoint(playerWalkingAudio, transform.position, 1);
        isPlaySound = true;
        yield return new WaitForSeconds(playerWalkingAudio.length / audioSource.pitch);
        isPlaySound = false;
    }
}
