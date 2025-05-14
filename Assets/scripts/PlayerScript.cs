using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 2.0f;
    public float playerSprint = 3.0f;

    [Header("Player Animator and Gravity")]
    public CharacterController CC;
    public float gravity = -9.81f;
    Vector3 velocity;
    public Animator animator;

    [Header("Player Jumping and Surface Check")]
    public float jumpRange = 1f;
    public Transform surfaceCheck;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;
    bool onSurface;

    [Header("Camera Rotation")]
    public Transform playerCamera;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;

    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        // Check if player is grounded
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);
        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f; // stick to the ground
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        CC.Move(velocity * Time.deltaTime);

        // Handle movement
        PlayerMove();
        Jump();
        Sprint();
    }

    void PlayerMove()
    {   
        animator.SetBool ("Idle", false);
        animator.SetBool ("Walk", true);
        animator.SetBool ("Running", false);
        animator.SetBool ("RifleWalk", false);
        animator.SetBool ("IdleAim", false);

        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            CC.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
        }else{
            animator.SetBool ("Idle", true);
            animator.SetBool ("Walk", false);
            animator.SetBool ("Running", false);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && onSurface)
        {
            animator.SetBool("Idle",false);
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpRange * -2f * gravity);
        }else{
            animator.SetBool("Idle",true);
            animator.ResetTrigger("Jump");
        }
    }

   void Sprint()
{
    if (Input.GetButton("Sprint") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && onSurface)
    {
        float vertical_axis = Input.GetAxisRaw("Vertical");
        float horizontal_axis = Input.GetAxisRaw("Horizontal");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if (direction.magnitude >= 0.1f)
        {

            animator.SetBool("Walk",false);
            animator.SetBool("Running",true);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            CC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
        }else{
            animator.SetBool("Walk",true);
            animator.SetBool("Running",false);
        }
    }
}

}
