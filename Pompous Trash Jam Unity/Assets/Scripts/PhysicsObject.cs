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
  protected virtual void Update()
  {
    if (GameManager.IsGameActive && storedVelocity != Vector2.zero)
    {
      // Restore velocity
      rb.velocity = storedVelocity;
      storedVelocity = Vector2.zero;
    }
    
    if (!GameManager.IsGameActive)
    {
      if (storedVelocity == Vector2.zero)
      {
        storedVelocity = rb.velocity;
      }

      rb.velocity = Vector2.zero;
    }
  }

  public virtual void Launch(Vector3 origin, float force) 
  {
    Vector2 direction = (transform.position - origin).normalized;
    rb.AddForce(direction * force);
    rb.AddForce(transform.up * force / 2);

    float randTorque = Random.Range(-25, 25);
    rb.AddTorque(randTorque);
  }
}
