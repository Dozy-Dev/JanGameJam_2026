using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeedX = 8f;
    [SerializeField] private float moveSpeedY = 5.5f;

    [Header("Movement Smoothing")]
    [SerializeField] private float timeToFullSpeed = 0.10f;
    [SerializeField] private float timeToStop = 0.08f;
    [SerializeField] private float inputDeadZone = 0.1f;

    [SerializeField] private InputActionReference moveAction;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool movementEnabled = true;

    private float smoothVelX;
    private float smoothVelY;

    public bool FacingRight { get; private set; } = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void OnEnable()
    {
        if (moveAction != null)
            moveAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null)
            moveAction.action.Disable();
    }

    private void Update()
    {
        if (!movementEnabled)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        UpdateFacing(moveInput.x);
    }

    private void FixedUpdate()
    {
        ApplyMovement2D();
    }

    private void ApplyMovement2D()
    {
        Vector2 input = moveInput;
        if (input.magnitude < inputDeadZone)
            input = Vector2.zero;

        float targetVX = input.x * moveSpeedX;
        float targetVY = input.y * moveSpeedY;

        Vector2 v = rb.linearVelocity;

        float smoothTimeX = Mathf.Abs(targetVX) > 0.01f ? timeToFullSpeed : timeToStop;
        float smoothTimeY = Mathf.Abs(targetVY) > 0.01f ? timeToFullSpeed : timeToStop;

        float newVX = Mathf.SmoothDamp(v.x, targetVX, ref smoothVelX, smoothTimeX);
        float newVY = Mathf.SmoothDamp(v.y, targetVY, ref smoothVelY, smoothTimeY);

        rb.linearVelocity = new Vector2(newVX, newVY);
    }

    private void UpdateFacing(float inputX)
    {
        if (Mathf.Abs(inputX) < 0.01f) return;

        bool shouldFaceRight = inputX > 0f;
        if (shouldFaceRight != FacingRight)
        {
            FacingRight = shouldFaceRight;
            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(s.x) * (FacingRight ? 1f : -1f);
            transform.localScale = s;
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;

        if (!enabled)
        {
            rb.linearVelocity = Vector2.zero;
            smoothVelX = 0f;
            smoothVelY = 0f;
        }
    }
}
