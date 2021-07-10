using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  [SerializeField] private float baseSpeed = 800f;
  [SerializeField] private float movementSmoothTime = 0.1f;
  [SerializeField] private float baseJumpSpeed = 10f;
  [SerializeField] private float maxJumpTime = 0.3f;
  [SerializeField] private LayerMask whatIsGround;
  [SerializeField] private Transform groundCheckPosition;

  private bool isGrounded = false;
  private bool isJumpKeyHeld = false;
  private float jumpTimeCounter;
  private Rigidbody2D rb;
  private Vector2 velocity;
  private float xInput;

  const float groundCheckDistance = .1f;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    isGrounded = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, groundCheckDistance, whatIsGround);
  }

  private void FixedUpdate()
  {
    float targetVelocityX = baseSpeed * xInput * Time.fixedDeltaTime;
    Vector2 targetVelocity = new Vector2(targetVelocityX, rb.velocity.y);
    rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothTime);

    if (isJumpKeyHeld && jumpTimeCounter > 0)
    {
      rb.velocity = new Vector2(rb.velocity.x, baseJumpSpeed);
      jumpTimeCounter -= Time.fixedDeltaTime;
    }
    else
    {
      jumpTimeCounter = 0;
    }
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(10, 10, 400, 30), "jumpTimeCounter: " + jumpTimeCounter);
  }

  // OnJump is called on both press and release of the jump key
  public void OnJump()
  {
    isJumpKeyHeld = !isJumpKeyHeld;

    if (isJumpKeyHeld && isGrounded)
    {
      jumpTimeCounter = maxJumpTime;
    }
  }

  public void OnMove(InputValue value)
  {
    Vector2 motionVector = value.Get<Vector2>();
    xInput = motionVector.x;
  }
}
