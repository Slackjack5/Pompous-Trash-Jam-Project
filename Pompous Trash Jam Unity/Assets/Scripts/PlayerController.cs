using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EZCameraShake;
public class PlayerController : PhysicsObject
{
  [SerializeField] private float baseSpeed = 800f;
  [SerializeField] private float movementSmoothTime = 0.1f;
  [SerializeField] private float baseJumpSpeed = 7f;
  [SerializeField] private float maxJumpTime = 0.3f;
  [SerializeField] private LayerMask whatIsBox;
  [SerializeField] private LayerMask whatIsGround;
  [SerializeField] private Transform groundCheckPosition;
  [SerializeField] private Transform frontCheckPosition;
  [SerializeField] private float hitboxWidth = 1f;
  [SerializeField] private float hitboxHeight = 1f;
  [SerializeField] private float meleeCooldownTime = 0.5f;
  [SerializeField] private float stunTime = 4f;
  [SerializeField] private bool isHittingFront = false;

  private Vector2 boxCastSize;
  private float currentMeleeCooldown;
  private bool isFacingRight = true;
  private bool isGrounded = false;
  private bool isJumpKeyHeld = false;
  private bool isStunned = false;
  private float jumpTimeCounter;
  private Vector2 velocity;
  private float xInput;
  
  const float touchDistance = .1f;

    //Animations
    [SerializeField] private Animator myAnimator;
    // Start is called before the first frame update
  protected override void Start()
  {
    base.Start();

    boxCastSize = new Vector2(transform.localScale.x - touchDistance, touchDistance);
  }

  protected override void Update()
  {
    base.Update();

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
    if (GameManager.IsGameActive)
    {
      // Move
      if (!isStunned)
      {
        float targetVelocityX = baseSpeed * xInput * Time.fixedDeltaTime;
        Vector2 targetVelocity = new Vector2(targetVelocityX, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothTime);
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

      //Animation
      if (xInput != 0 && !isStunned)
      {
        myAnimator.SetFloat("Speed", 1);
      }
      else
      {
        myAnimator.SetFloat("Speed", 0);
      }
    }

    if (isGrounded)
    {
      //animation
      myAnimator.SetBool("isJumping", false);
    }
    else
    {
      //animation
      myAnimator.SetBool("isJumping", true);
    }
  }

  public void AttackFinished()
    {
        //animation
        myAnimator.SetBool("isAttacking", false);
    }

  public override void Launch(Vector3 origin, float force)
  {
    base.Launch(origin, force);

    StartCoroutine(Stun());
  }

  public void OnAttack()
  {
    if (GameManager.IsGameActive && currentMeleeCooldown <= 0)
    {
      // Hit boxes in front of player
      if (isHittingFront)
      {
        Vector2 origin = frontCheckPosition.position;
        if (isFacingRight)
        {
          origin.x += hitboxWidth / 2;
        }
        else
        {
          origin.x -= hitboxWidth / 2;
        }

        Vector2 size = new Vector2(hitboxWidth, hitboxHeight);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, size, /* angle = */ 0f, Vector2.right, /* distance = */ 0f, whatIsBox);

        foreach (RaycastHit2D hit in hits)
        {
          hit.transform.GetComponent<BoxDestruction>().Hit(isFacingRight);
        }

        //Screen shake
        if (hits.Length > 0)
        {
          CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1f);
        }
      }
      else
      {
        // Hit boxes beneath player
        RaycastHit2D groundHit = Physics2D.BoxCast(groundCheckPosition.position, boxCastSize, /* angle = */ 0f, Vector2.down, /* distance = */ 0f, whatIsBox);
        if (groundHit)
        {
          groundHit.transform.GetComponent<BoxDestruction>().Hit(isFacingRight);
        }
      }
      
      currentMeleeCooldown = meleeCooldownTime;

     //animation
      myAnimator.SetBool("isAttacking", true);
    }
  }

  // OnJump is called on both press and release of the jump key
  public void OnJump()
  {
    isJumpKeyHeld = !isJumpKeyHeld;
    if (GameManager.IsGameActive && !isStunned && isJumpKeyHeld && isGrounded)
    {
      jumpTimeCounter = maxJumpTime;
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

  private IEnumerator Stun()
  {
    isStunned = true;
    yield return new WaitForSeconds(stunTime);
    isStunned = false;
  }
}
