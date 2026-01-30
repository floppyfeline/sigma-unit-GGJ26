using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private PlayerInputs moveInput;
    private Rigidbody rb;

    [SerializeField] private Transform visualTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 moveDirection =  moveInput.CameraRotation* new Vector3(moveInput.Move.x, 0, moveInput.Move.y);
        rb.linearVelocity = moveDirection * moveSpeed;

        if (moveInput.Move != Vector2.zero)
        {
            Vector2 worldRight = (Vector3.forward + Vector3.right).normalized;
            float playerRotationInDegrees = Vector2.SignedAngle(worldRight, moveInput.Move);

            // Clamp rotation to 45° increments
            int wraps = (int)(playerRotationInDegrees / 45f + 0.5f);
            visualTransform.localEulerAngles = new Vector3(0, wraps * 45f, 0);
        }
    }

    public void SetInputs(PlayerInputs inputs)
    {
        moveInput = inputs;
    }
}
