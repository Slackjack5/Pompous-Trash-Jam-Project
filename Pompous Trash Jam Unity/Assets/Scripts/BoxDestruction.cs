using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDestruction : PhysicsObject
{
  [SerializeField] private int maxHealth = 3;
  [SerializeField] private float hitForce = 10f;
  public GameObject destructable;
  public GameObject Wormhole;
  public GameObject Box;
  public bool Explosive;
  public bool gravityBox;
  public bool infinityBox;
  public bool upgradeBox;
  public float fieldofImpact;
  public float force;
  public float torque;
  public LayerMask LayerToHit;
  private int currentHealth;

  protected override void Start()
  {
    base.Start();

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
        if (Explosive)
        {
            Explosion();
        }
        if (gravityBox)
        {
            Instantiate(Wormhole, new Vector2(transform.position.x,transform.position.y+2), Quaternion.identity);
        }
        else if (infinityBox)
        {
            Instantiate(Box, new Vector2(transform.position.x+.5f, transform.position.y + .5f), Quaternion.identity);
            Instantiate(Box, new Vector2(transform.position.x - .5f, transform.position.y + .5f), Quaternion.identity);
            Instantiate(Box, new Vector2(transform.position.x, transform.position.y + .5f), Quaternion.identity);
        }
        else if (upgradeBox)
        {
            int randNumber = Random.Range(0, 3);
            if (randNumber==0)
            {
                Debug.Log("Spawned Power Up 1");
            }
            else if (randNumber == 1)
            {
                Debug.Log("Spawned Power Up 2");
            }
            else if (randNumber == 2)
            {
                Debug.Log("Spawned Power Up 3");
            }
        }
        DestroyGameObject();
    }
    //Debugging Code
    private void OnMouseDown()
    {
        Destroy();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fieldofImpact);
    }
}
