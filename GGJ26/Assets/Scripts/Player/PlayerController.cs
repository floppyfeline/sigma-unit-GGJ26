using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerSpace;
    [SerializeField] private GroundedCheck groundCheck;
    [SerializeField] private WallCheck wallCheck;
    
    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravityBuildup = 0.05f;

    private PlayerInputs moveInput;
    private Rigidbody rb;
    private bool movementEnabled = true;

    [SerializeField] private Transform visualTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(movementEnabled) HandleMovement();

        HandleParenting();
    }

    public void SetInputs(PlayerInputs inputs)
    {
        moveInput = inputs;
    }

    private void HandleMovement()
    {
        Vector3 camForward = moveInput.CameraRotation * Vector3.forward;
        Vector3 camRight   = moveInput.CameraRotation * Vector3.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection =
            camRight * moveInput.Move.x +
            camForward * moveInput.Move.y;

        Vector3 moveVector = moveDirection * moveSpeed;

        if(!wallCheck.IsWalled) rb.linearVelocity = new Vector3(moveVector.x, rb.linearVelocity.y, moveVector.z);

        if(!groundCheck.IsGrounded)
        {
            rb.linearVelocity += Vector3.down * gravityBuildup;
        }
        
        #region Clamp rotation to 45° Steps

        if (moveInput.Move != Vector2.zero)
        {
            Vector3 moveDir =
                camRight * moveInput.Move.x +
                camForward * moveInput.Move.y;

            moveDir.y = 0f;
            moveDir.Normalize();

            // 0° reference = world +Z
            Vector2 move2D = new Vector2(moveDir.x, moveDir.z);

            float angle = Mathf.Atan2(move2D.x, move2D.y) * Mathf.Rad2Deg;

            float snapped = Mathf.Round(angle / 45f) * 45f;

            visualTransform.rotation = Quaternion.Euler(0f, snapped, 0f);
        }

        #endregion
    }
    private void HandleParenting()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f, Constants.LAYER_MovingPlatform))
        {
            transform.SetParent(hit.transform, true);
        }
        else
        {
            transform.SetParent(playerSpace, true);
        }
    }
    public void Jump()
    {
        if(groundCheck.IsGrounded) rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    public void ToggleMovement(bool toggle)
    {
        movementEnabled = toggle;
        rb.linearVelocity = Vector3.zero;
    }
}
