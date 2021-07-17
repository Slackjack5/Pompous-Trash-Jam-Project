using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class BoxDestruction : PhysicsObject
{
  [SerializeField] private int maxHealth = 2;
  [SerializeField] private float hitForce = 500f;
  [SerializeField] private float freezeTime = 0.1f;
  [SerializeField] private GameObject destructable;

  private SpriteRenderer spriteRenderer;

  private int currentHealth;

  protected override void Start()
  {
    Shader.SetGlobalFloat("_ShockTime", -99999);
    base.Start();

    spriteRenderer = GetComponent<SpriteRenderer>();

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

  protected virtual void PreDestroy()
  {
    spriteRenderer.enabled = false;

    Instantiate(destructable, transform.position, Quaternion.identity);

    CameraShaker.Instance.ShakeOnce(2.5f, 2.5f, .2f, 2f);
  }

  protected virtual void PostDestroy()
  {

  }

  protected virtual void Destroy()
  {
    PreDestroy();
    StartCoroutine(FreezeImpact());
  }

  public void BlackHoleDestroy()
  {
    PreDestroy();
  }

  protected IEnumerator FreezeImpact()
  {
    GameManager.DeactivateGame();
    yield return new WaitForSeconds(freezeTime);
    GameManager.ActivateGame();
    PostDestroy();
    Destroy(gameObject);
  }

  //Debugging Code
  private void OnMouseDown()
  {
    Destroy();
  }
}
