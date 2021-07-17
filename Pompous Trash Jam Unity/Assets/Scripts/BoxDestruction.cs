using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class BoxDestruction : PhysicsObject
{
  [SerializeField] private int maxHealth = 3;
  [SerializeField] private float hitForce = 10f;
  [SerializeField] private float freezeTime = 0.4f;
  [SerializeField] private float explodeSpeed = 10f;
  [SerializeField] private string tube = "Tube";

  public GameObject destructable;
  public GameObject Wormhole;
  public GameObject Box;
  public GameObject explosionVFX;
  public GameObject infinityVFX;
  public bool Explosive;
  public bool gravityBox;
  public bool infinityBox;
  public bool upgradeBox;
  public float fieldofImpact;
  public float force;
  public float torque;
  public LayerMask LayerToHit;

  private SpriteRenderer spriteRenderer;

  private int currentHealth;
  private bool isExploded = false;

  protected override void Start()
  {
    Shader.SetGlobalFloat("_ShockTime", -99999);
    base.Start();

    spriteRenderer = GetComponent<SpriteRenderer>();

    currentHealth = maxHealth;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (Explosive && !isExploded && collision.gameObject.layer != LayerMask.NameToLayer(tube) && rb.velocity.magnitude >= explodeSpeed)
    {
      Destroy();
      isExploded = true;
    }
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
      PhysicsObject physicsObject = obj.GetComponent<PhysicsObject>();
      if (physicsObject)
      {
        physicsObject.Launch(transform.position, force);
      }
    }
  }

  private void Destroy()
  {
    spriteRenderer.enabled = false;

    Instantiate(destructable, transform.position, Quaternion.identity);
    if (Explosive)
    {
      Explosion();
    }
    if (gravityBox)
    {
      Instantiate(Wormhole, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
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
    CameraShaker.Instance.ShakeOnce(2.5f, 2.5f, .2f, 2f);

    StartCoroutine(FreezeImpact());
  }

  private IEnumerator FreezeImpact()
  {
    GameManager.DeactivateGame();
    yield return new WaitForSeconds(freezeTime);
    GameManager.ActivateGame();
    if (Explosive && !gravityBox && !infinityBox)
    {
      Shader.SetGlobalFloat("_ShockTime", Time.time);
      Vector2 focalPoint = new Vector2(Camera.main.WorldToScreenPoint(transform.position, Camera.main.stereoActiveEye).x / Screen.width, Camera.main.WorldToScreenPoint(transform.position, Camera.main.stereoActiveEye).y / Screen.height);
      Shader.SetGlobalVector("_FocalPoint", focalPoint);
      Instantiate(explosionVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    }
    if (infinityBox)
    {
      Instantiate(infinityVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
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
