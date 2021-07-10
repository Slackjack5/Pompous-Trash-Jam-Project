using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDestruction : MonoBehaviour
{
  [SerializeField] private int maxHealth = 3;
  [SerializeField] private float hitForce = 10f;

  public GameObject destructable;

  private Rigidbody2D rb;

  private int currentHealth;

  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();

    currentHealth = maxHealth;
  }

  public void Hit(bool isHitRight)
  {
    int direction = isHitRight ? 1 : -1;
    rb.AddForce(new Vector2(hitForce * direction, hitForce));

    currentHealth--;
    if (currentHealth <= 0)
    {
      Destroy();
    }
  }

  public void Destroy()
  {
    Instantiate(destructable, transform.position, Quaternion.identity);
    DestroyGameObject();
  }

  private void DestroyGameObject()
  {
    Destroy(gameObject);
  }
}
