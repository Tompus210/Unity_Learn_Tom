using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float turnSmoothTime = 0.1f;

    [Header("Jump & Gravity")]
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Refs")]
    public Transform cameraTransform;   // MainCamera Transform
    public Transform groundCheck;       // Empty-Child an den Füßen
    public float groundCheckRadius = 0.3f;
    public LayerMask groundMask;        // auf "Ground" setzen

    CharacterController controller;
    float turnSmoothVelocity;
    Vector3 velocity;
    bool isGrounded;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- Grounded Check ---
        isGrounded = Physics.CheckSphere(
            groundCheck.position, 
            groundCheckRadius, 
            groundMask, 
            QueryTriggerInteraction.Ignore
        );

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f; // sanft auf Boden „kleben“

        // --- Input (Legacy Input Manager) ---
        float h = Input.GetAxisRaw("Horizontal"); // A/D oder ←/→
        float v = Input.GetAxisRaw("Vertical");   // W/S oder ↑/↓

        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        // --- Kamera-ausgerichtete Bewegung + sanftes Drehen ---
        if (inputDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg
                              + cameraTransform.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y, targetAngle, 
                ref turnSmoothVelocity, turnSmoothTime
            );

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        // --- Jump ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // --- Gravity ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

