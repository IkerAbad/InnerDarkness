using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float walkingSpeed = 1f;
    [SerializeField] public Transform[] enemyPath;
    public int currentTargetIndex = 0;
    private bool isReversing = false;

    public SpriteRenderer spriteRenderer;

    public Sprite[] directionSpritesUp;
    public Sprite[] directionSpritesDown;
    public Sprite[] directionSpritesLeft;
    public Sprite[] directionSpritesRight;
    
    [Space]
    [Space]
    [Space]
    
    public Sprite[] directionSpritesUpShoot;
    public Sprite[] directionSpritesDownShoot;
    public Sprite[] directionSpritesLeftShoot;
    public Sprite[] directionSpritesRightShoot;

    public float spriteUpdateDelay = 0.2f; // Tiempo entre cambios de sprite
    public float spriteUpdateTimer = 0f; // Temporizador para controlar los cambios de sprite
    public int currentSpriteIndex = 0; // �ndice del sprite actual

    public int repeatCount = 2; // N�mero de veces que se repiten las primeras dos animaciones

    // Variables para el disparo
    public GameObject projectilePrefab;
    public float projectileSpeed = 3.5f;
    public float fireRate = 1f;
    private float nextFire = 0f;

    private AudioSource walkingSound;

    [SerializeField] private AudioClip[] shootingAudios;
    [SerializeField] private GameObject shootingPrefab;

    private FOV fov;

    private int directionToChange = 0; //0 <- 1 -> 2 arriba y 3 abajo
    void Start()
    {
        fov = GetComponent<FOV>();

        walkingSound = GetComponent<AudioSource>();
        
        if (enemyPath.Length > 0)
        {
            SetTarget(enemyPath[0]);
        }
    }

    private void Shoot()
    {
        GameObject x = Instantiate(shootingPrefab, this.transform);

        int random = Random.Range(0, shootingAudios.Length);

        x.GetComponent<AudioSource>().clip = shootingAudios[random];
        
        x.GetComponent<AudioSource>().Play();
        
        Destroy(x, 5);
        
        walkingSound.Stop();
        
        fov.viewRadius = 8;
        fov.viewAngle = 150;
        
        Vector3 target = FindObjectOfType<PlayerController>().transform.position;  // Obtiene la posici�n del jugador
        Vector3 direction = (target - transform.position);  // Calcula la direcci�n hacia el jugador
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);  // Crea un proyectil
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;  // Lanza el proyectil en la direcci�n del jugador

        Debug.Log("Shooting -> " + rb.velocity);
    }

    void Update()
    {
        if (!PauseMenu.isPaused && enemyPath[currentTargetIndex])
        {
            Transform currentTarget = enemyPath[currentTargetIndex];
            float step = walkingSpeed * Time.deltaTime;

            if (fov.target == null) // solo se mueve si no hay un objetivo
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

                SetTarget(enemyPath[currentTargetIndex]);
            }

            // Actualizar el temporizador para controlar los cambios de sprite
            spriteUpdateTimer += Time.deltaTime;
            if (spriteUpdateTimer >= spriteUpdateDelay)
            {
                spriteUpdateTimer = 0f; // Reiniciar el temporizador
                UpdateSprite(); // Cambiar el sprite
            }

            // Add shooting logic here
            if (fov.target != null)
            {
                if (Time.time > nextFire/2)
                {
                    switch (this.directionToChange)
                    {
                    case 0:
                        if (currentSpriteIndex < directionSpritesLeft.Length - repeatCount)
                        {
                            currentSpriteIndex++;
                        }
                        else
                        {
                            currentSpriteIndex = Random.Range(0, directionSpritesLeft.Length - repeatCount);
                        }
                        spriteRenderer.sprite = directionSpritesLeft[currentSpriteIndex]; // Move left
                        break;
                    case 1:
                        if (currentSpriteIndex < directionSpritesRight.Length - repeatCount)
                        {
                            currentSpriteIndex++;
                        }
                        else
                        {
                            currentSpriteIndex = Random.Range(0, directionSpritesRight.Length - repeatCount);
                        }
                        spriteRenderer.sprite = directionSpritesRight[currentSpriteIndex]; // Move right
                        break;
                    case 2:
                        if (currentSpriteIndex < directionSpritesUp.Length - repeatCount)
                        {
                            currentSpriteIndex++;
                        }
                        else
                        {
                            currentSpriteIndex = Random.Range(0, directionSpritesUp.Length - repeatCount);
                        }
                        spriteRenderer.sprite = directionSpritesUp[currentSpriteIndex]; // Move up
                        break;
                    case 3:
                        if (currentSpriteIndex < directionSpritesDown.Length - repeatCount)
                        {
                            currentSpriteIndex++;
                        }
                        else
                        {
                            currentSpriteIndex = Random.Range(0, directionSpritesDown.Length - repeatCount);
                        }
                        spriteRenderer.sprite = directionSpritesDown[currentSpriteIndex]; // Move down
                        break;
                }
                }

                if ( Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    Shoot();
                }
            }
            else
            {
                fov.viewRadius = 6;
                fov.viewAngle = 90;
            }
        }
    }

    private void UpdateSprite()
    {
        Vector3 direction = enemyPath[currentTargetIndex].position - transform.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) // If moving more in x direction
        {
            if (direction.x > 0)
            {
                directionToChange = 1; //Derecha
                
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
                directionToChange = 0; //Izquierda
                
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
                directionToChange = 2; //Arriba
                
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
                directionToChange = 3; //Abajo
                
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
}

