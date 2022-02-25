using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private Transform cam;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Animator animator;

    private int isWalkingHash;
    private int isRunningHash;
    private bool isWalking;
    private bool isRunning;

    private float speed = 6f;
    private float constSpeed = 6f;
    private float jumpHeight = 2.4f;
    private float gravityValue = -35f;
    private Vector3 playerVelocity;
    private Vector3 direction;

    private bool groundedPlayer;
    private bool ground;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private bool isCrouching = false;

    private new GameObject gameObject;
    private Vector3 center;
    private float radius;
    private bool isShout = false;

    private AudioSource audioShout;

    // Start is called before the first frame update
    void Start()
    {
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        audioShout = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isWalking = animator.GetBool(isWalkingHash);
        isRunning = animator.GetBool(isRunningHash);

        center = this.transform.position;

        IsGrounded();
        Gravity();
        Move();
        Sprint();
        Crouch();
        Jump();
        Shout();
        Echolocation();
    }

    void IsGrounded()
    {
        ground = controller.isGrounded;
        if (ground && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        if (controller.isGrounded == true)
        {
            groundedPlayer = true;
        }
    }

    void Gravity()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            if (!isWalking)
            {
                // then set the isWalking boolean to be true
                animator.SetBool(isWalkingHash, true);
            }
        }
        else
        {
            if (isWalking)
            {
                // then set the isWalking boolean to be false
                animator.SetBool(isWalkingHash, false);
            }
        }
    }
    
    void Sprint()
    {
        if (isCrouching == false)
        {
            if (Input.GetButtonDown("Sprint"))
            {
                speed = 10f;
                if (!isRunning)
                {
                    // then set the isRunning boolean to be true
                    animator.SetBool(isRunningHash, true);
                }
            }

            if (Input.GetButtonUp("Sprint"))
            {
                speed = constSpeed;
                if (isRunning)
                {
                    // then set the isRunning boolean to be false
                    animator.SetBool(isRunningHash, false);
                }
            }
        }
    }

    void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = true;
            speed /= 3;
            jumpHeight /= 3;
            player.transform.localScale *= 0.5f;
        }

        if (Input.GetButtonUp("Crouch"))
        {
            isCrouching = false;
            speed *= 3;
            jumpHeight *= 3;
            player.localScale = player.transform.localScale / 0.5f;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            groundedPlayer = false;
        }
    }

    void Shout()
    {
        if (Input.GetKey("q"))
        {
            isShout = true;
            audioShout.Play();
        }
    }

    void Echolocation()
    {
        gameObject = GameObject.FindWithTag("Echolocation");

        gameObject.GetComponent<Renderer>().sharedMaterial.SetVector("_Center", center);

        if (isShout == true)
        {
            radius += 10 * Time.deltaTime;
            gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Radius", radius);

        }
        if (radius > 60 && isShout == true)
        {
            radius = 0;
            isShout = false;
        }
    }

}
