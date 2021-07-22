using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class ExplosiveBox : SpecialBox
{
  [SerializeField] private string tube = "Tube";
  [SerializeField] private float speedToExplode = 10f;
  [SerializeField] private GameObject explosionVFX;

  private bool isExploded = false;

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (!isExploded && collision.gameObject.layer != LayerMask.NameToLayer(tube) && rb.velocity.magnitude >= speedToExplode)
    {
      EnvironmentalDestroy();
      isExploded = true;
    }
  }

  protected override void PreFreeze()
  {
    base.PreFreeze();

    foreach (Collider2D obj in hitObjects)
    {
      if (obj.gameObject.GetInstanceID() != gameObject.GetInstanceID())
      {
        BoxDestruction box = obj.GetComponent<BoxDestruction>();
        if (box)
        {
          box.EnvironmentalDamage();
        }
      }
    }

    AkSoundEngine.PostEvent("Play_Explosion", gameObject);

    //Camera Shake
    CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1f);

    Shader.SetGlobalFloat("_ShockTime", Time.time);
    Vector2 focalPoint = new Vector2(Camera.main.WorldToScreenPoint(transform.position, Camera.main.stereoActiveEye).x / Screen.width, Camera.main.WorldToScreenPoint(transform.position, Camera.main.stereoActiveEye).y / Screen.height);
    Shader.SetGlobalVector("_FocalPoint", focalPoint);
    Instantiate(explosionVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
  }
}
