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
  [SerializeField] private float meleeCooldownTime = 0.5f;

  private Rigidbody2D rb;

  private Vector2 boxCastSize;
  private float currentMeleeCooldown;
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

    boxCastSize = new Vector2(transform.localScale.x - touchDistance, touchDistance);
  }

  private void Update()
  {
    // Use BoxCast instead of Raycast so that ground checking spans the width of the player
    isGrounded = Physics2D.BoxCast(groundCheckPosition.position, boxCastSize, /* angle = */ 0f, Vector2.down, /* distance = */ 0f, whatIsGround | whatIsBox);

    // Update melee cooldown
    if (currentMeleeCooldown > 0)
    {
      currentMeleeCooldown -= Time.deltaTime;
    }
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
    else
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
    GUI.Label(new Rect(100, 50, 400, 30), "currentMeleeCooldown: " + currentMeleeCooldown);
    GUI.Label(new Rect(300, 50, 400, 30), "isJumpKeyHeld: " + isJumpKeyHeld);
    GUI.Label(new Rect(450, 50, 400, 30), "isGrounded: " + isGrounded);
  }

  public void OnAttack()
  {
    if (GameManager.IsGameActive && currentMeleeCooldown <= 0)
    {
      // Hit boxes in front of player
      Vector2 origin = frontCheckPosition.position;
      origin.x += hitboxWidth / 2;
      Vector2 size = new Vector2(hitboxWidth, hitboxHeight);
      RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, size, /* angle = */ 0f, Vector2.right, /* distance = */ 0f, whatIsBox);

      foreach (RaycastHit2D hit in hits)
      {
        hit.transform.GetComponent<BoxDestruction>().Hit(isFacingRight);
      }

      currentMeleeCooldown = meleeCooldownTime;
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

  public void OnPause()
  {
    GameManager.TogglePause();
  }

  public void OnMainFire()
  {
    if (GameManager.IsMinigameActive)
    {
      GameManager.CurrentMinigame.OnFire();
    }
  }

  public void OnUp()
  {
    if (GameManager.IsMinigameActive)
    {
      GameManager.CurrentMinigame.OnUp();
    }
  }

  public void OnDown()
  {
    if (GameManager.IsMinigameActive)
    {
      GameManager.CurrentMinigame.OnDown();
    }
  }

  public void OnLeft()
  {
    if (GameManager.IsMinigameActive)
    {
      GameManager.CurrentMinigame.OnLeft();
    }
  }

  public void OnRight()
  {
    if (GameManager.IsMinigameActive)
    {
      GameManager.CurrentMinigame.OnRight();
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
