using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EZCameraShake;
public class BoxDestruction : PhysicsObject
{
  [SerializeField] private int maxHealth = 2;
  [SerializeField] private float freezeTime = 0.1f;
  [SerializeField] private GameObject trashPieces;
  [SerializeField] private UnityEvent destroyed;

  private SpriteRenderer spriteRenderer;

  private int currentHealth;
  private Color originalColor;

  protected override void Start()
  {
    Shader.SetGlobalFloat("_ShockTime", -99999);
    base.Start();

    spriteRenderer = GetComponent<SpriteRenderer>();

    currentHealth = maxHealth;
    originalColor = spriteRenderer.color;
  }

  public void FixedUpdate()
  {
    float healthRatio = ((float)currentHealth / (float)maxHealth);
    if (healthRatio > 0.1)
    {
      spriteRenderer.color = new Color(originalColor.r * healthRatio, originalColor.g * healthRatio, originalColor.b * healthRatio, originalColor.a);
    }
  }

  public void Hit(bool isHitRight, float force)
  {
    int direction = isHitRight ? 1 : -1;
    rb.AddForce(new Vector2(force * direction, force));

    currentHealth--;

    //Sound
    AkSoundEngine.PostEvent("Play_BoxHit", gameObject);
    if (currentHealth <= 0)
    {
      //Sound
      AkSoundEngine.PostEvent("Play_BoxBreak", gameObject);

      Destroy();
    }
  }

  public void EnvironmentalDamage()
  {
    currentHealth--;
    if (currentHealth <= 0)
    {
      EnvironmentalDestroy();
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.relativeVelocity.magnitude > 10)
    {
      //Sound
      AkSoundEngine.PostEvent("Play_BoxThud", gameObject);
    }
  }
  protected virtual void PreDestroy()
  {
    spriteRenderer.enabled = false;

    Instantiate(trashPieces, transform.position, Quaternion.identity);

    CameraShaker.Instance.ShakeOnce(2.5f, 2.5f, .2f, 2f);
  }

  protected virtual void PostDestroy()
  {
    destroyed.Invoke();
  }

  protected virtual void Destroy()
  {
    PreDestroy();
    StartCoroutine(FreezeImpact());
  }

  public virtual void EnvironmentalDestroy()
  {
    PreDestroy();
    Destroy(gameObject);
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
#if UNITY_EDITOR
    Destroy();
#endif
  }
}
