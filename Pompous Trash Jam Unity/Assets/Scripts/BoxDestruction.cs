using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDestruction : PhysicsObject
{
  [SerializeField] private int maxHealth = 3;
  [SerializeField] private float hitForce = 10f;
  [SerializeField] private float freezeTime = 0.4f;

  public GameObject destructable;
  public GameObject Wormhole;
  public GameObject Box;
  public GameObject explosionVFX;
  public GameObject blackHoleVFX;
  public Material blackHoleVFXMat;
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
    Shader.SetGlobalFloat("_ShockTime", -99999);
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
      obj.GetComponent<Rigidbody2D>().AddForce(transform.up * force / 2);
      obj.GetComponent<Rigidbody2D>().AddTorque(randTorque);
    }
  }

  private void Destroy()
  {
    Instantiate(destructable, transform.position, Quaternion.identity);
    if (Explosive)
    {
      Explosion();
    }
    if (gravityBox)
    {
      Instantiate(Wormhole, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    }
    else if (infinityBox)
    {
      Instantiate(Box, new Vector2(transform.position.x + .5f, transform.position.y + .5f), Quaternion.identity);
      Instantiate(Box, new Vector2(transform.position.x - .5f, transform.position.y + .5f), Quaternion.identity);
      Instantiate(Box, new Vector2(transform.position.x, transform.position.y + .5f), Quaternion.identity);
    }
    else if (upgradeBox)
    {
      int randNumber = Random.Range(0, 3);
      if (randNumber == 0)
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

    StartCoroutine(Freeze());
  }

  private IEnumerator Freeze()
  {
    GameManager.DeactivateGame();
    yield return new WaitForSeconds(freezeTime);
    GameManager.ActivateGame();
    if (Explosive && !gravityBox)
    {
      Shader.SetGlobalFloat("_ShockTime", Time.time);
      Vector2 focalPoint = new Vector2(Camera.main.WorldToScreenPoint(transform.position, Camera.main.stereoActiveEye).x / Screen.width, Camera.main.WorldToScreenPoint(transform.position, Camera.main.stereoActiveEye).y / Screen.height);
      Shader.SetGlobalVector("_FocalPoint", focalPoint);
      Instantiate(explosionVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    }
    if (gravityBox)
    {
      Shader.SetGlobalFloat("_ShockTime", Time.time);
      Vector2 focalPoint = new Vector2(Camera.main.WorldToScreenPoint(transform.position, Camera.main.stereoActiveEye).x / Screen.width, Camera.main.WorldToScreenPoint(transform.position, Camera.main.stereoActiveEye).y / Screen.height);
      Shader.SetGlobalVector("_FocalPoint", focalPoint);
      GameObject temp = Instantiate(blackHoleVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
      SpriteRenderer tempSR = temp.GetComponentInChildren<SpriteRenderer>();
      tempSR.material = new Material(blackHoleVFXMat);
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
