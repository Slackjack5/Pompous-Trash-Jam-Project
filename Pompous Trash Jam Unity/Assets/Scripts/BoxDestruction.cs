using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDestruction : MonoBehaviour
{
  [SerializeField] private int maxHealth = 3;
  [SerializeField] private float hitForce = 10f;

    public GameObject destructable;
    public GameObject Wormhole;
    public bool Explosive;
    public bool gravityBox;

    public float fieldofImpact;
    public float force;
    public float torque;
    [SerializeField] Vector2 forceDirection;
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

    public LayerMask LayerToHit;

  private void DestroyGameObject()
  {
    Destroy(gameObject);
  }

    private void Explosion()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldofImpact, LayerToHit);
        foreach (Collider2D obj in objects)
        {
            float randTorque = Random.Range(-25, 25);
            Vector2 direction = obj.transform.position - transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
            obj.GetComponent<Rigidbody2D>().AddForce(transform.up * force/2);
            obj.GetComponent<Rigidbody2D>().AddTorque(randTorque);
        }
    }

    public void Destroy()
    {
        Instantiate(destructable, transform.position, Quaternion.identity);
        if (Wormhole)
        {
            Instantiate(Wormhole, new Vector2(transform.position.x,transform.position.y+2), Quaternion.identity);
        }
        DestroyGameObject();
    }

    //Debugging Code
    private void OnMouseDown()
    {
        if (Explosive)
        {
            Explosion();
        }
        Destroy();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fieldofImpact);
    }
}
