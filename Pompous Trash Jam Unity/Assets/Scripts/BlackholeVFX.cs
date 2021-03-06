using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BlackholeVFX : MonoBehaviour
{
  VisualEffect vfx;
  SpriteRenderer circle;
  float startTime;
  private float duration;

  public void Initialize(float duration)
  {
    this.duration = duration;
  }

  // Start is called before the first frame update
  void Start()
  {
    startTime = Time.time;
    vfx = GetComponent<VisualEffect>();
    circle = GetComponentInChildren<SpriteRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    if ((Time.time - startTime) > duration)
    {
      Destroy(gameObject);
    }
    if ((Time.time - startTime) > duration / 2)
    {
      vfx.SetInt("SpawnRate", 0);
      Color tmp = circle.material.color;
      tmp.a = Mathf.Max(1 - (Time.time - startTime - 5), 0);
      circle.material.SetColor("_BaseColor", tmp);
    }
  }

  void OnTriggerStay2D(Collider2D col)
  {
    BoxDestruction box = col.gameObject.GetComponent<BoxDestruction>();
    if (box && col.gameObject != gameObject)
    {
      box.EnvironmentalDestroy();
    }
  }
}
