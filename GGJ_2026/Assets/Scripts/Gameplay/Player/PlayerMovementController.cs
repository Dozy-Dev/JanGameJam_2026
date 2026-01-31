using DG.Tweening;
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

    [Header("Attack")]
    [SerializeField] private InputActionReference punchAction;

    public bool FacingRight { get; private set; } = true;

    [Header("Placeholder Wiggle (DOTween)")]
    [SerializeField] private Transform visualRoot;
    [SerializeField] private float wiggleRot = 6f;
    [SerializeField] private float wiggleX = 0.06f;
    [SerializeField] private float wiggleSpeed = 0.12f;
    [SerializeField] private float wiggleMoveThreshold = 0.15f;

    private Tween wiggleTween;
    private Vector3 visualStartLocalPos;
    private Quaternion visualStartLocalRot;
    private bool isWiggling;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        if (visualRoot == null)
            visualRoot = transform;

        visualStartLocalPos = visualRoot.localPosition;
        visualStartLocalRot = visualRoot.localRotation;

        //punchAction.ToInputAction().performed += ctx => PerformPunch();
    }

    private void OnEnable()
    {
        if (moveAction != null)
            moveAction.action.Enable();

        if(punchAction != null)
            punchAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null)
            moveAction.action.Disable();

        if(punchAction != null)
            punchAction.action.Disable();
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
        UpdateWiggle();
    }

    private void PerformPunch()
    {
        // player animation
        if (visualRoot != null)
        {
            Animator anim;
            if (visualRoot.TryGetComponent<Animator>(out anim))
            {
                anim.SetTrigger("Punch");
            }
        }
    }

    private void ApplyMovement2D()
    {
        Vector2 input = moveInput;
        if (input.magnitude < inputDeadZone)
            input = Vector2.zero;

        if( visualRoot != null)
        {
            Animator anim;
            if( visualRoot.TryGetComponent<Animator>(out anim))
            {
                anim.SetFloat("MoveSpeed", input.magnitude);
            }
        }

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
            StopWiggle();
        }
    }


    private void UpdateWiggle()
    {
        if (visualRoot == null) return;

        // Use actual velocity so it wiggles when shoved, not just when input is held
        float speed = rb.linearVelocity.magnitude;

        if (speed > wiggleMoveThreshold && !isWiggling)
            StartWiggle();
        else if (speed <= wiggleMoveThreshold && isWiggling)
            StopWiggle();
    }

    private void StartWiggle()
    {
        isWiggling = true;

        wiggleTween?.Kill();

        Sequence seq = DOTween.Sequence();

        seq.Append(visualRoot.DOLocalMoveX(visualStartLocalPos.x + wiggleX, wiggleSpeed).SetEase(Ease.InOutSine));
        seq.Join(visualRoot.DOLocalRotate(new Vector3(0f, 0f, wiggleRot), wiggleSpeed).SetEase(Ease.InOutSine));

        seq.Append(visualRoot.DOLocalMoveX(visualStartLocalPos.x - wiggleX, wiggleSpeed).SetEase(Ease.InOutSine));
        seq.Join(visualRoot.DOLocalRotate(new Vector3(0f, 0f, -wiggleRot), wiggleSpeed).SetEase(Ease.InOutSine));

        seq.SetLoops(-1, LoopType.Restart);
        wiggleTween = seq;
    }

    private void StopWiggle()
    {
        isWiggling = false;

        wiggleTween?.Kill();
        wiggleTween = null;

        visualRoot.DOLocalMove(visualStartLocalPos, 0.10f).SetEase(Ease.OutSine);
        visualRoot.DOLocalRotateQuaternion(visualStartLocalRot, 0.10f).SetEase(Ease.OutSine);
    }

}
