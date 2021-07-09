using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  [SerializeField] private float baseSpeed = 1000f;
  [SerializeField] private float movementSmoothTime = 0.4f;

  private Rigidbody2D rb;
  private Vector2 velocity;
  private float xInput;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  private void FixedUpdate()
  {
    float targetVelocityX = baseSpeed * xInput * Time.fixedDeltaTime;
    Vector2 targetVelocity = new Vector2(targetVelocityX, rb.velocity.y);
    rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothTime);
  }

  public void OnMove(InputValue value)
  {
    Vector2 motionVector = value.Get<Vector2>();
    xInput = motionVector.x;
  }
}
