using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Variables
    public float walkingSpeed = 4f;
    public Animator animator;
    public AudioClip playerWalkingAudio;
    public float soundSpeedForWalking = 3.5f;
    public bool isPlaySound = false;
    public int health = 10;  // Players health

    private Rigidbody2D playerRigidbody;
    private Vector3 direction;
    private AudioSource audioSource;

    private float horizontal = 0;
    private float vertical = 0;

    [SerializeField] private AudioSource audioHit;

    void Start()
    {
        // Get Rigidbody
        playerRigidbody = GetComponent<Rigidbody2D>();

        // Get AudioSource
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = soundSpeedForWalking;
        if(ConfigManager.instance != null)
        {
            audioSource.volume = ConfigManager.instance.sfxV;
        }
        
    }

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
        else
        {
            //Debug.Log("Paused");
        }
    }

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

    // Player taking damage function
    public void TakeDamage(int damage)
    {
        // Hit sound
        audioHit.Play();

        // Hit damage
        health -= damage;

        // If players health is 0 or belows dies and Game Over
        if (health <= 0) 
        {
            // Load "GameOver" scene
            //SceneManager.LoadScene("GameOver");
            
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-1);
            Debug.Log("Player died!");
        }
    }
}
