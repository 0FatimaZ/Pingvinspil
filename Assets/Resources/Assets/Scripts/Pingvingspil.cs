using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PenguinController : MonoBehaviour
{
    public float moveSpeed = 10;
    public float gravity = 9.81f;
    public float jumpForce = 10;
    public float rotationSpeed = 1.0f;

    private Animator anim;
    private CharacterController ctrl;

    private Vector3 velocity;
    private float movement;

    private Vector3 initialPosition;
    public float points = 0;
    // public - man kan ændre variablen i unity

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ctrl = GetComponent<CharacterController>();
        if(!ctrl.isGrounded)
        {
            velocity.y = -0.1f;
        }

        initialPosition = transform.position;

    }

    void Update()
    {
        // If not standing on the ground, fall
        if(!ctrl.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        // If standing on the ground, handle input
        else
        {
            Vector3 rotation = new Vector3(0f, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0f);
            transform.Rotate(rotation);

            movement = Input.GetAxis("Vertical");
            if (Mathf.Abs(movement) > 0.5)
            {
                anim.SetBool("Running", true);
            }
            else
            {
                anim.SetBool("Running", false);
            }

            if (Input.GetButtonDown("Jump"))
            {
                anim.SetBool("InAir", true);
                velocity.y = jumpForce;
            }
            else
            {
                anim.SetBool("InAir", false);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 move = transform.TransformDirection(new Vector3(0, 0, movement * moveSpeed));
        ctrl.Move((move + velocity) * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "water")
        {
            ctrl.enabled = false;
            transform.position = initialPosition;
            ctrl.enabled = true;
            transform.rotation = Quaternion.identity;
            velocity = Vector3.down * 0.1f;
        }
        
        if(other.gameObject.tag == "water")
        {
           SceneManager.LoadScene("Losescreen"); 
        }

        if(other.gameObject.tag == "gem")
        {
            Destroy(other.gameObject, 0.5f);
            points = points + 1;
            win();
        }
    }

    void win()
    // void - kun når der er conditions
    {
        if(points == 3)
        {
            SceneManager.LoadScene("Winscreen");
        }
    }

    
}
