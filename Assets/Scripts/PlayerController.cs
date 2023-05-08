using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables
    public float speed = 4f;
    public Animator animator;
    private Rigidbody2D myRigidbody;
    private Vector3 direction;
    public bool playerIsArmed = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get Rigidbody
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Player Inputs
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Player direction in a 2d plane
        direction = new Vector3(horizontal, vertical, 0);

        // Player Animations
        AnimateMovement(direction);

    }

    //
    void FixedUpdate()
    {
        // Player Movement
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    // Movement Animation
    void AnimateMovement(Vector3 direction)
    {
        if(animator != null)
        {
            if(direction.magnitude > 0)
            {
                animator.SetBool("isMoving", true);

                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }
}
