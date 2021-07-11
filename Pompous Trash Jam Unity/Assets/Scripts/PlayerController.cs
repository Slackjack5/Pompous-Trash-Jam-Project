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
  [SerializeField] private LayerMask whatIsBox;
  [SerializeField] private LayerMask whatIsGround;
  [SerializeField] private Transform groundCheckPosition;
  [SerializeField] private Transform frontCheckPosition;
  [SerializeField] private float hitboxWidth = 1f;
  [SerializeField] private float hitboxHeight = 1f;

  private Rigidbody2D rb;

  private bool isFacingRight = true;
  private bool isGrounded = false;
  private bool isJumpKeyHeld = false;
  private float jumpTimeCounter;
  private Vector2 velocity;
  private float xInput;

  const float touchDistance = .1f;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    isGrounded = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, touchDistance, whatIsGround | whatIsBox);
  }

  private void FixedUpdate()
  {
    // Move
    if (GameManager.IsGameActive)
    {
      float targetVelocityX = baseSpeed * xInput * Time.fixedDeltaTime;
      Vector2 targetVelocity = new Vector2(targetVelocityX, rb.velocity.y);
      rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothTime);
    }
    else if (GameManager.IsMinigameActive)
    {
      rb.velocity = Vector2.zero;
    }

    // Jump
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
    GUI.Label(new Rect(10, 50, 400, 30), "xInput: " + xInput);
  }

  public void OnAttack()
  {
    if (GameManager.IsGameActive)
    {
      // Hit boxes beneath player
      RaycastHit2D groundHit = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, touchDistance, whatIsBox);
      if (groundHit)
      {
        groundHit.transform.GetComponent<BoxDestruction>().Destroy();
      }

      // Hit boxes in front of player
      Vector2 origin = frontCheckPosition.position;
      origin.x += hitboxWidth / 2;
      Vector2 size = new Vector2(hitboxWidth, hitboxHeight);
      RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, size, /* angle = */ 0f, Vector2.right, /* distance = */ 0f, whatIsBox);
      foreach (RaycastHit2D hit in hits)
      {
        hit.transform.GetComponent<BoxDestruction>().Hit(isFacingRight);
      }
    }
    
    if (GameManager.IsMinigameActive)
    {
      GameManager.CurrentMinigame.OnFire();
    }
  }

  // OnJump is called on both press and release of the jump key
  public void OnJump()
  {
    isJumpKeyHeld = !isJumpKeyHeld;

    if (GameManager.IsGameActive)
    {
      if (isJumpKeyHeld && isGrounded)
      {
        jumpTimeCounter = maxJumpTime;
      }
    }
  }

  public void OnMove(InputValue value)
  {
    Vector2 motionVector = value.Get<Vector2>();
    xInput = motionVector.x;

    if (xInput < 0 && isFacingRight || xInput > 0 && !isFacingRight)
    {
      Flip();
    }
  }

  private void Flip()
  {
    isFacingRight = !isFacingRight;

    Vector3 theScale = transform.localScale;
    theScale.x *= -1;
    transform.localScale = theScale;
  }
}
