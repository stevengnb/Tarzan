using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Animator animator;
    private Rigidbody rb;
    [SerializeField] private float speed, coef;
    [SerializeField] private LayerMask ground;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float time;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera characterCam;
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;

    [Header("Crouching")]
    [SerializeField] private float yScale;

    [Header("Throw Rock")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform attackPos;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject thrown;
    [SerializeField] private float rockCooldown;
    [SerializeField] private float rockForce;
    [SerializeField] private float rockUpwardForce;

    private bool isReadyThrow;
    private float startYScale;

    private int isWalkHash;
    private int isBackwardWalkHash;
    private int isRunHash;
    private int isJumpHash;
    private int isPunchHash;
    private int isFallHash;

    private float xRotate;
    private float yRotate;

    private bool isJumping = false;
    private bool enableMovement;
    public bool isGrounded;
    public bool isFrozen = false;
    public bool isGrappling;

    private float tempSpeed;
    private Vector3 velocitySet;
    private PlayerHand hand;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        hand = FindObjectOfType<PlayerHand>();

        Debug.Log("HAND NYA = " + hand);

        isWalkHash = Animator.StringToHash("isWalk");
        isBackwardWalkHash = Animator.StringToHash("isBWalk");
        isRunHash = Animator.StringToHash("isRun");
        isJumpHash = Animator.StringToHash("isJump");
        isPunchHash = Animator.StringToHash("isPunch");
        isFallHash = Animator.StringToHash("isFalling");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isReadyThrow = true;
        isGrounded = true;
        startYScale = transform.localScale.y;
        tempSpeed = speed;
    }

    public void FixedUpdate()
    {
        if (!isFrozen)
        {
            Movement();
        }
    }

    public void Update()
    {
        if(Time.timeScale == 0f)
        {
            FirstPerson();
        }

        AnimationControl();
        InputMovement();
        SurfaceAlignment();
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
        }

        if(isFrozen)
        {
            FreezePlayer();
        } else
        {
            UnfreezePlayer();
        }

        if(!isGrappling)
        {
            isGrounded = true;
            rb.drag = 1;
        } else
        {
            isGrounded = false;
            rb.drag = 0;
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumping = true;
        isGrounded = false;
    }

    private void ResetIsGrappling()
    {
        isGrappling = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            isGrounded = true;
            animator.SetBool(isJumpHash, false);
        }

        if(enableMovement)
        {
            enableMovement = false;
            ResetIsGrappling();
            GetComponent<Grappling>().GrappleEnds();
        }
    }

    public void Movement()
    {

        FirstPerson();

        if (isGrappling)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        

        Vector3 frontBack = Camera.main.transform.forward;
        frontBack.y = 0f;
        frontBack.Normalize();

        Vector3 leftRight = Camera.main.transform.right;
        leftRight.y = 0f;
        leftRight.Normalize();

        Vector3 moveDirection = frontBack * verticalInput + leftRight * horizontalInput;
        moveDirection.Normalize();

        Vector3 counterMovement = new Vector3(-rb.velocity.x, 0, -rb.velocity.z);

        float currSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currSpeed = speed * 3f;
        }

        rb.AddForce(moveDirection * currSpeed);
        rb.AddForce(counterMovement * coef);
    }

    private void FirstPerson()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * sensitivityY;

        xRotate -= mouseY;
        yRotate += mouseX;

        xRotate = Mathf.Clamp(xRotate, -70f, 80f);

        characterCam.transform.localRotation = Quaternion.Euler(xRotate, 0, 0);
        
        if(Time.timeScale != 0f)
        {
            transform.rotation = Quaternion.Euler(0, yRotate, 0);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        } else
        {
            Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            Camera.main.transform.rotation = Quaternion.Euler(xRotate, yRotate, 0);
        }
    }

    private void SurfaceAlignment()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit infoRayHit = new RaycastHit();
        Quaternion rotationRef = Quaternion.Euler(0, 0, 0);

        if (Physics.Raycast(ray, out infoRayHit, ground))
        {
            Quaternion newRotate = Quaternion.Euler(rotationRef.eulerAngles.x, transform.eulerAngles.y, rotationRef.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotate, curve.Evaluate(time));
        }
    }

    private void AnimationControl()
    {
        bool isWalk = animator.GetBool(isWalkHash);
        bool isRun = animator.GetBool(isRunHash);
        bool isJump = animator.GetBool(isJumpHash);

        bool walkingForward = Input.GetKey(KeyCode.W);
        bool walkingLeft = Input.GetKey(KeyCode.A);
        bool walkingBackward = Input.GetKey(KeyCode.S);
        bool walkingRight = Input.GetKey(KeyCode.D);
        bool running = Input.GetKey(KeyCode.LeftShift);
        bool jumping = Input.GetKeyDown(KeyCode.Space);

        if (!isWalk && (walkingForward || walkingLeft || walkingRight))
        {
            animator.SetBool(isWalkHash, true);
        }
        else if (!isWalk && walkingBackward)
        {
            animator.SetBool(isBackwardWalkHash, true);
        }

        if (isWalk && !(walkingForward || walkingLeft || walkingRight))
        {
            animator.SetBool(isWalkHash, false);
        }
        else if (!isWalk && !walkingBackward)
        {
            animator.SetBool(isBackwardWalkHash, false);
        }

        if (!isRun && running && (walkingForward || walkingLeft || walkingBackward || walkingRight))
        {
            animator.SetBool(isRunHash, true);
        }

        if (isRun && (!running || !(walkingForward || walkingLeft || walkingBackward || walkingRight)))
        {
            animator.SetBool(isRunHash, false);
        }

        if (!isJump && jumping)
        {
            animator.SetBool(isJumpHash, true);
        }

        if(rb.velocity.y < -2.5f)
        {
            animator.SetBool(isFallHash, true);
        } else
        {
            animator.SetBool(isFallHash, false);
        }
    }

    private void InputMovement()
    {
        bool isPunch = animator.GetBool(isPunchHash);

        bool leftClick = Input.GetMouseButtonDown(0);
        bool crouching = Input.GetKeyDown(KeyCode.LeftControl);
        bool notCrouching = Input.GetKeyUp(KeyCode.LeftControl);
        bool throwRock = Input.GetKeyDown(KeyCode.X);

        if (!isPunch && leftClick)
        {
            animator.SetBool(isPunchHash, true);
            AudioManagerGame.instance.PunchSound();
            StartCoroutine(isPunchFalse(0.5f));
        }

        if(crouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if(notCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        if (throwRock && isReadyThrow)
        {
            isReadyThrow = false;
            GameObject rockToThrow = Instantiate(rock, attackPos.position, cam.rotation, thrown.transform);
            Rigidbody rockToThrowRb = rockToThrow.GetComponent<Rigidbody>();
            Vector3 forces = cam.transform.forward * rockForce + transform.up * rockUpwardForce;
            rockToThrowRb.AddForce(forces, ForceMode.Impulse);
            StartCoroutine(isThrowFalse(rockCooldown));
        }
    }

    private IEnumerator isThrowFalse(float duration)
    {
        yield return new WaitForSeconds(duration); 
        isReadyThrow = true;
    }

    private IEnumerator isPunchFalse(float duration)
    {
        yield return new WaitForSeconds(duration);
        animator.SetBool(isPunchHash, false);
    }


    public void FreezePlayer()
    {
        isFrozen = true;
        speed = 0;
        rb.velocity = Vector3.zero;
    }

    public void UnfreezePlayer()
    {
        isFrozen = false;
        speed = tempSpeed;
    }

    public void GrappleTo(Vector3 targetPos, float trjHeight)
    {
        isGrappling = true;
        velocitySet = CalculateJumpVelocity(transform.position, targetPos, trjHeight);
        Invoke("SetVelocity", 0.15f);
        Invoke("ResetIsGrappling", 3f);
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trjHeight)
    {
        // mengambil posisi dari player sekarang
        Vector3 currentPosition = startPoint;

        // mengambil kekuatan gravitasi pada game
        float gravity = Physics.gravity.y;

        // menghitung waktu yang diperlukan untuk grapple menggunakan
        // rumus yang didapatkan dari persamaan kinematika yaitu d = vi * t + 1/2 * a * t^2
        float timeOfFlight = Mathf.Sqrt((2 * trjHeight) / -gravity);

        // menghitung jarak horizontal antara titik grapple dengan posisi sekarang
        Vector3 horizontalDisplacement = endPoint - currentPosition;
        horizontalDisplacement.y = 0f;

        // menghitung kecepatan horizontal dengan rumus jarak dibagi waktu
        Vector3 horizontalVelocity = horizontalDisplacement / timeOfFlight;

        // menghitung kecepatan vertical dengan rumus gerak vertikal (vi^2 = vo^2 - 2 * g * h)
        float verticalVelocity = Mathf.Sqrt(-2 * gravity * trjHeight);

        // menggabungkan kecepatan horizontal dan vertikal untuk membuat movement swing ketika grapple
        Vector3 jumpVelocity = horizontalVelocity + Vector3.up * verticalVelocity;

        return jumpVelocity;
    }

    private void SetVelocity()
    {
        rb.velocity = velocitySet;
        enableMovement = true;
    }
}
