using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Joystick Refs")]
    public FloatingJoystick moveJoystick;
    public FloatingJoystick lookJoystick;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundMask;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string speedParam = "Speed_f";

    private CharacterController controller;
    private Transform mainCam;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;
    public bool IsMoving => moveJoystick != null && moveJoystick.Direction.magnitude > 0.1f;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main.transform;

        #if UNITY_ANDROID || UNITY_IOS
        moveSpeed = 4f;
        rotationSpeed = 5f;
        #endif

        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleGravity();
        UpdateAnimations();
    }

    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundMask);
        if (isGrounded && velocity.y < 0) velocity.y = -2f;
    }

    private void HandleMovement()
    {
        if (moveJoystick == null) return;

        Vector2 input = moveJoystick.Direction;
        Vector3 moveDir = mainCam.forward * input.y + mainCam.right * input.x;
        moveDir.y = 0;

        if (input.magnitude > 0.1f)
        {
            RotatePlayer(moveDir);
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
    }

    private void RotatePlayer(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        float speed = moveJoystick != null ? moveJoystick.Direction.magnitude : 0f;
        animator.SetFloat(speedParam, Mathf.Clamp01(speed));
    }

    public void SetJoysticks(FloatingJoystick moveJoy, FloatingJoystick lookJoy)
    {
        moveJoystick = moveJoy;
        lookJoystick = lookJoy;
    }

    public void SetSpeedModifier(float modifier)
    {
        currentSpeed = moveSpeed * modifier;
    }
}
