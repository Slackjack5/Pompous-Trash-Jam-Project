using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class applies to objects whose velocity needs to be stored during minigames.
/// </summary>
public class PhysicsObject : MonoBehaviour
{
  protected Rigidbody2D rb;

  private Vector2 storedVelocity;

  // Start is called before the first frame update
  protected virtual void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    if (GameManager.IsGameActive && storedVelocity != Vector2.zero)
    {
      // Restore velocity
      rb.velocity = storedVelocity;
      storedVelocity = Vector2.zero;
    }

    if (GameManager.IsMinigameActive)
    {
      if (storedVelocity == Vector2.zero)
      {
        storedVelocity = rb.velocity;
      }

      rb.velocity = Vector2.zero;
    }
  }
}
