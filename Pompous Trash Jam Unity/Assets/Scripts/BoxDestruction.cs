using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class BoxDestruction : PhysicsObject
{
  [SerializeField] private int maxHealth = 2;
  [SerializeField] private float hitForce = 500f;
  [SerializeField] private float freezeTime = 0.1f;
  [SerializeField] private GameObject trashPieces;

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

    //Sound
    AkSoundEngine.PostEvent("Play_BoxHit", gameObject);
    if (currentHealth <= 0)
    {
      //Sound
      AkSoundEngine.PostEvent("Play_BoxBreak", gameObject);
      Destroy();
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if(collision.relativeVelocity.magnitude>10)
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
