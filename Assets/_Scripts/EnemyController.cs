using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    // Private variables
    private bool isReversing = false;
    private AudioSource walkingSound;
    private FOV fov;
    private int directionToChange = 0; //0 left, 1 right, 2 up, 3 down
    private bool followingPlayer = false;
    [SerializeField] private AudioClip[] shootingAudios;
    [SerializeField] private GameObject shootingPrefab;
    [SerializeField] private Transform player;

    // Public variables
    public float walkingSpeed = 1f;
    public Transform[] enemyPath;
    public int currentTargetIndex = 0;
    public bool alreadySeen = false;
    public bool allowToshoot = false;

    // Shooting Variables
    private float nextFire = 0f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 3.5f;
    public float fireRate = 1f;

    // Sprites
    public SpriteRenderer spriteRenderer;

    // Moving Sprites
    public Sprite[] directionSpritesUp;
    public Sprite[] directionSpritesDown;
    public Sprite[] directionSpritesLeft;
    public Sprite[] directionSpritesRight;
    
    // Shooting Sprites
    public Sprite[] directionSpritesUpShoot;
    public Sprite[] directionSpritesDownShoot;
    public Sprite[] directionSpritesLeftShoot;
    public Sprite[] directionSpritesRightShoot;

    public float spriteUpdateDelay = 0.2f; // Time between sprites
    public float spriteUpdateTimer = 0f; // Timer for sprite changes
    public int currentSpriteIndex = 0; // Current sprite index
    public int currentSpriteIndexShoot = 0; // Current shooting sprite index
    public int repeatCount = 2; // Number of times the first two animations are repeated

   
    void Start()
    {
        fov = GetComponent<FOV>();
        walkingSound = GetComponent<AudioSource>();

        if (enemyPath.Length > 0)
        {
            SetTarget(enemyPath[0]);
        }
    }

    private bool Shoot()
    {
        GameObject x = Instantiate(shootingPrefab, this.transform);

        int random = Random.Range(0, shootingAudios.Length);

        x.GetComponent<AudioSource>().clip = shootingAudios[random];
        
        x.GetComponent<AudioSource>().Play();
        
        Destroy(x, 5);
        
        walkingSound.Stop();
        
        fov.viewRadius = 8;
        fov.viewAngle = 150;

        // Gets player position
        Vector3 target = FindObjectOfType<PlayerController>().transform.position;
        // Gets player direction
        Vector3 direction = (target - transform.position);  

        bool aux = false;
        
        // Raycast to player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, fov.viewRadius, fov.obstacleMask);

        if (hit.collider != null)
        {
            if (!hit.collider.CompareTag("Player"))
            {
                aux = true;
            }
        }
        // Creates proyectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);  
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        // Sends proyectile to players position
        rb.velocity = direction * projectileSpeed;  
        //Debug.Log("Shooting -> " + rb.velocity);

        return aux;
    }

    void Update()
    {
        if (Vector2.Distance(player.position,transform.position) < fov.viewRadius)
        {
            if (alreadySeen)
            {
                followingPlayer = true;
                fov.target = player;
                SetTarget(player);
            }
        }
        else
        {
            // Out of range
            followingPlayer = false;
            alreadySeen = false;
        }
        
        if (!PauseMenu.isPaused && enemyPath[currentTargetIndex])
        {
            Transform currentTarget = enemyPath[currentTargetIndex];
            float step = walkingSpeed * Time.deltaTime;

            if (fov.target == null) // moves if there is no target
            {
                transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, step);
            }

            if (Vector2.Distance(transform.position, currentTarget.position) <= 0.01f)
            {
                walkingSound.Play();
                
                if (isReversing)
                {
                    currentTargetIndex--;
                    if (currentTargetIndex < 0)
                    {
                        currentTargetIndex = 1;
                        isReversing = false;
                    }
                }
                else
                {
                    currentTargetIndex++;
                    if (currentTargetIndex >= enemyPath.Length)
                    {
                        currentTargetIndex = enemyPath.Length - 2;
                        isReversing = true;
                    }
                }

                if (!followingPlayer)
                {
                    SetTarget(enemyPath[currentTargetIndex]);
                }
                else
                {
                    // Follows player
                    fov.target = player;
                }
            }
            
            if (fov.target != null)
            {
                if (Time.time > nextFire)
                {
                    switch (this.directionToChange)
                    {
                    case 0:
                        currentSpriteIndexShoot = Random.Range(0, directionSpritesLeftShoot.Length);
                        
                        spriteRenderer.sprite = directionSpritesLeftShoot[currentSpriteIndexShoot]; // Move left
                        break;
                    case 1:
                        currentSpriteIndexShoot = Random.Range(0, directionSpritesRightShoot.Length);
                        
                        spriteRenderer.sprite = directionSpritesRightShoot[currentSpriteIndexShoot]; // Move right
                        break;
                    case 2:
                        currentSpriteIndexShoot = Random.Range(0, directionSpritesUpShoot.Length);
                        
                        spriteRenderer.sprite = directionSpritesUpShoot[currentSpriteIndexShoot]; // Move up
                        break;
                    case 3:
                        currentSpriteIndexShoot = Random.Range(0, directionSpritesDownShoot.Length);
                        
                        spriteRenderer.sprite = directionSpritesDownShoot[currentSpriteIndexShoot]; // Move down
                        break;
                }
                }

                bool x = false;
                
                if ( Time.time > nextFire)
                {
                    Vector3 direction = player.position - transform.position;

                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) // If moving more in x direction
                    {
                        if (direction.x > 0)
                        {
                            directionToChange = 1; //Derecha
                        }
                        else
                        {
                            directionToChange = 0; //Izquierda
                        }
                    }
                    else // If moving more in y direction
                    {
                        if (direction.y > 0)
                        {
                            directionToChange = 2; //Arriba
                        }
                        else
                        {
                            directionToChange = 3; //Abajo
                        }
                    }
                    
                    nextFire = Time.time + fireRate; 
                    x = Shoot();
                }
                
               
                
                /*if (x)
                {
                    int aux = 0;
                    float distance = float.MaxValue;

                    for (int i = 0; i < enemyPath.Length; i++)
                    {
                        if (Vector2.Distance(player.position,enemyPath[i].position) < distance)
                        {
                            distance = Vector2.Distance(player.position, enemyPath[i].position);
                            aux = i;
                        }
                    }

                    currentTargetIndex = aux;
                }*/
            }
            else
            {
                // Update timer to track sprite changes
                spriteUpdateTimer += Time.deltaTime;
                if (spriteUpdateTimer >= spriteUpdateDelay)
                {
                    spriteUpdateTimer = 0f; // Reset timer
                    UpdateSprite(); // Change sprite
                }
                
                fov.viewRadius = 5f;
                fov.viewAngle = 90;
            }
        }
    }

    private void UpdateSprite()
    {
        Vector3 direction = new Vector3();
        
        if (!followingPlayer)
        {
            direction  = enemyPath[currentTargetIndex].position - transform.position;
        }
        else
        {
            direction = player.position - transform.position;
        }

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) // If moving more in x direction
        {
            if (direction.x > 0)
            {
                directionToChange = 1; // Right
                
                if (currentSpriteIndex < directionSpritesRight.Length - repeatCount)
                {
                    currentSpriteIndex++;
                }
                else
                {
                    currentSpriteIndex = Random.Range(0, directionSpritesRight.Length - repeatCount);
                }
                spriteRenderer.sprite = directionSpritesRight[currentSpriteIndex]; // Move right
            }
            else
            {
                directionToChange = 0; // Left
                
                if (currentSpriteIndex < directionSpritesLeft.Length - repeatCount)
                {
                    currentSpriteIndex++;
                }
                else
                {
                    currentSpriteIndex = Random.Range(0, directionSpritesLeft.Length - repeatCount);
                }
                spriteRenderer.sprite = directionSpritesLeft[currentSpriteIndex]; // Move left
            }
        }
        else // If moving more in y direction
        {
            if (direction.y > 0)
            {
                directionToChange = 2; // Up
                
                if (currentSpriteIndex < directionSpritesUp.Length - repeatCount)
                {
                    currentSpriteIndex++;
                }
                else
                {
                    currentSpriteIndex = Random.Range(0, directionSpritesUp.Length - repeatCount);
                }
                spriteRenderer.sprite = directionSpritesUp[currentSpriteIndex]; // Move up
            }
            else
            {
                directionToChange = 3; //Down
                
                if (currentSpriteIndex < directionSpritesDown.Length - repeatCount)
                {
                    currentSpriteIndex++;
                }
                else
                {
                    currentSpriteIndex = Random.Range(0, directionSpritesDown.Length - repeatCount);
                }
                spriteRenderer.sprite = directionSpritesDown[currentSpriteIndex]; // Move down
            }
        }
    }

    public void SetTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) // If moving more in x direction
        {
            if (direction.x > 0)
            {
                currentSpriteIndex = Random.Range(0, directionSpritesRight.Length - repeatCount);
                spriteRenderer.sprite = directionSpritesRight[currentSpriteIndex]; // Move right
            }
            else
            {
                currentSpriteIndex = Random.Range(0, directionSpritesLeft.Length - repeatCount);
                spriteRenderer.sprite = directionSpritesLeft[currentSpriteIndex]; // Move left
            }
        }
        else // If moving more in y direction
        {
            if (direction.y > 0)
            {
                currentSpriteIndex = Random.Range(0, directionSpritesUp.Length - repeatCount);
                spriteRenderer.sprite = directionSpritesUp[currentSpriteIndex]; // Move up
            }
            else
            {
                currentSpriteIndex = Random.Range(0, directionSpritesDown.Length - repeatCount);
                spriteRenderer.sprite = directionSpritesDown[currentSpriteIndex]; // Move down
            }
        }
    }

    public Transform GetNextWaypoint()
    {
        return enemyPath[currentTargetIndex];
    }

    // If enemy collides with player, Game Over
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
        }
    }
}

