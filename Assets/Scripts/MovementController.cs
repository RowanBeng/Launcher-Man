using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class MovementController : MonoBehaviour
{
    #region Public Variables
    [Header("Public Fields")]
    public float speed;
    public float dashDistance;
    public float jumpVelocity;
    public Transform frontCheck;
    public float wallSlidingSpeed;
    public Vector2 frontCheckSize;

    #endregion

    #region Private Variables
    private float m_MoveX;
    private float m_MoveY;
    private Vector3 m_LastDirection = Vector3.left;
    private int m_AirJumpCount;
    private int m_AirJumpCountMax;
    private bool m_MovePlayer;
    private bool m_IsTouchingFront;
    private bool m_WallSliding;
    private SpriteRenderer mySpriteRenderer;
    [FormerlySerializedAs("boxCollider2D")]
    [Header("Private Fields")]
    [SerializeField] private BoxCollider2D playerBoxCollider2D;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private LayerMask platformsLayerMask;

    #endregion

    #region Unity Event Functions

    private void Awake()
    { 
        m_AirJumpCountMax = 999999;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

        CheckForMovementInput();
    }

    private void FixedUpdate()
    {
        if (m_MovePlayer)
        {
            MovePlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FallDetector") {
            SceneManager.LoadScene("Lost");
        }
    }
 

    #endregion

    #region Private Functions

    private void CheckForMovementInput()
    {
        if (IsGrounded())
        {
            m_AirJumpCount = 0;
            m_WallSliding = false;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            m_MovePlayer = true;
            m_MoveX = -1;
            m_MoveY = 0;
            TurnTowardsDirection(-1);
            mySpriteRenderer.flipX = true;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            m_MovePlayer = true;
            m_MoveX = 1;
            m_MoveY = 0;
            TurnTowardsDirection(1);
            mySpriteRenderer.flipX = false;

        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
            if (IsGrounded()) Jump();
            else if (m_AirJumpCount < m_AirJumpCountMax)
            {
                Jump();
                m_AirJumpCount++;
            }
        }

        if (m_MovePlayer)
        {
            WallCheck();
        }
    }

    private void MovePlayer()
    {
        var moveDirection = new Vector3(m_MoveX, m_MoveY).normalized;
        transform.position += moveDirection * (speed * Time.deltaTime);
        m_LastDirection = moveDirection;
        m_MovePlayer = false;
    }

    private void Dash()
    {
        transform.position += m_LastDirection * dashDistance;
    }

    private void Jump()
    {
        playerRigidbody.velocity = Vector2.up * jumpVelocity;
    }

    private bool IsGrounded()
    {
        var bounds = playerBoxCollider2D.bounds;
        var raycastHit2D = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2D.collider != null;
    }

    private void TurnTowardsDirection(int direction)
    {
        var position = frontCheck.localPosition;
        position = new Vector2(0.55f * direction, position.y);
        frontCheck.localPosition = position;
    }

    private void WallCheck()
    {
        m_IsTouchingFront = Physics2D.OverlapBox(frontCheck.position, frontCheckSize, 0f, platformsLayerMask);
        if (!IsGrounded() && m_IsTouchingFront)
        {
            m_WallSliding = true;
        }
        else
        {
            m_WallSliding = false;
        }

        if (!m_WallSliding) return;
        var velocity = playerRigidbody.velocity;
        playerRigidbody.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -wallSlidingSpeed, float.MaxValue));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(frontCheck.position, frontCheckSize);
        Gizmos.color = Color.green;
    }

    #endregion

    #region Public Functions


    #endregion
}