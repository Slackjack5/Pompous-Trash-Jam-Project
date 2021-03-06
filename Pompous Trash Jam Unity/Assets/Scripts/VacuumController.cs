using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using EZCameraShake;

public class VacuumController : MonoBehaviour
{
  [SerializeField] private ScoreTracker scoreTracker;
  [SerializeField] private string trash = "Trash";

  Rigidbody2D bone;
  VisualEffect vfx;

  private bool vacuumStarted=false;
  // Start is called before the first frame update
  void Start()
  {
    bone = GetComponent<Rigidbody2D>();
    vfx = GameObject.Find("SuctionVFX").GetComponent<VisualEffect>();
    vfx.SetInt("SpawnRate", 0);
    vfx.SetVector2("VFXCenter", bone.position); // set center initially and do not change it
  }

  void FixedUpdate()
  {
    if (GameManager.IsGameActive && !GameManager.TubeMinigame.IsReady && Input.GetMouseButton(0))
    {
      vfx.SetInt("SpawnRate", 64);
      //Camera Shake
      CameraShaker.Instance.ShakeOnce(1f, 1f, .1f, 1f);

      //Sound
      if(vacuumStarted==false)
      {
        AkSoundEngine.PostEvent("Play_VacuumLoop", gameObject);
        vacuumStarted = true;
      }
    }
    else
    {
      vfx.SetInt("SpawnRate", 0);
      //Sound
      if(vacuumStarted == true)
      {
        AkSoundEngine.PostEvent("Play_VacuumEnd", gameObject);
        AkSoundEngine.PostEvent("Stop_VacuumLoop", gameObject);
        vacuumStarted = false;
      }
    }

    Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

    if (GameManager.IsGameActive && !GameManager.TubeMinigame.IsReady)
    {
      vfx.SetFloat("VFXAngle", Mathf.Deg2Rad * (bone.rotation + 90));
      Vector2 normal = (mousePos - bone.position).normalized;
      bone.AddForce(normal * 500);
    }
  }

  void OnCollisionEnter2D(Collision2D col)
  {
    if (col.gameObject.layer == LayerMask.NameToLayer(trash))
    {
      scoreTracker.Increase(1);
      Destroy(col.gameObject);
    }
  }
}
