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

  private float currentFreezeTime = 0;
  private int currentHealth;
  private bool isEnvironmentallyDestroyed = false;
  private bool isFrozen = false;
  private Color originalColor;

  protected override void Start()
  {
    Shader.SetGlobalFloat("_ShockTime", -99999);
    base.Start();

    spriteRenderer = GetComponent<SpriteRenderer>();

    currentHealth = maxHealth;
    originalColor = spriteRenderer.color;

    isEnvironmentallyDestroyed = false;
  }

  protected override void Update()
  {
    base.Update();

    if (isFrozen)
    {
      currentFreezeTime += Time.deltaTime;
      if (currentFreezeTime >= freezeTime)
      {
        GameManager.ActivateGame();
        PostFreeze();
        Destroy(gameObject);
      }
    }
  }

  public void FixedUpdate()
  {
    float healthRatio = ((float)currentHealth / (float)maxHealth);
    if (healthRatio > 0.1)
    {
      spriteRenderer.color = new Color(originalColor.r * healthRatio, originalColor.g * healthRatio, originalColor.b * healthRatio, originalColor.a);
    }
  }

  public void PlayerHit(bool isHitRight, float force, bool isMinigameBoxHit)
  {
    int direction = isHitRight ? 1 : -1;
    rb.AddForce(new Vector2(force * direction, force));

    currentHealth--;
    GameObject.Find("Capsule Player").GetComponent<PlayerController>().SpawnPunch();
    //Sound
    AkSoundEngine.PostEvent("Play_BoxHit", gameObject);
    AkSoundEngine.PostEvent("Play_Crit", gameObject);
    if (currentHealth <= 0)
    {
      //Sound
      AkSoundEngine.PostEvent("Play_BoxBreak", gameObject);

      if (isMinigameBoxHit)
      {
        EnvironmentalDestroy();
      }
      else
      {
        Destroy();
      }
    }
  }

  public void EnvironmentalDamage()
  {
    currentHealth--;
    if (currentHealth <= 0 && !isEnvironmentallyDestroyed)
    {
      isEnvironmentallyDestroyed = true;
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

  // Do things before freeze impact
  protected virtual void PreFreeze()
  {
    spriteRenderer.enabled = false;

    Instantiate(trashPieces, transform.position, Quaternion.identity);

    CameraShaker.Instance.ShakeOnce(2.5f, 2.5f, .2f, 2f);
  }

  // Do things after freeze impact
  protected virtual void PostFreeze()
  {
    destroyed.Invoke();
  }

  // Destroy with freeze impact
  protected virtual void Destroy()
  {
    PreFreeze();
    if (GameManager.IsGameActive)
    {
      FreezeImpact();
    }
  }

  // Destroy without freeze impact
  public virtual void EnvironmentalDestroy()
  {
    PreFreeze();
    Destroy(gameObject);
  }

  private void FreezeImpact()
  {
    GameManager.DeactivateGame();
    isFrozen = true;
  }
}
