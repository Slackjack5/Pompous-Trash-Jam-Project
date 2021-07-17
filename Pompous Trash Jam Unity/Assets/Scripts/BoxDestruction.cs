using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class BoxDestruction : PhysicsObject
{
  [SerializeField] private int maxHealth = 3;
  [SerializeField] private float hitForce = 10f;
  [SerializeField] private float freezeTime = 0.4f;
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

  protected virtual void Destroy()
  {
    spriteRenderer.enabled = false;

    Instantiate(destructable, transform.position, Quaternion.identity);

    CameraShaker.Instance.ShakeOnce(2.5f, 2.5f, .2f, 2f);
  }

  protected IEnumerator FreezeImpact()
  {
    GameManager.DeactivateGame();
    yield return new WaitForSeconds(freezeTime);
    GameManager.ActivateGame();

    Destroy(gameObject);
  }

  //Debugging Code
  private void OnMouseDown()
  {
    Destroy();
  }
}
