using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BlackholeVFX : MonoBehaviour
{
  VisualEffect vfx;
  SpriteRenderer circle;
  float startTime;
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
    if ((Time.time - startTime) > 10)
    {
      Destroy(gameObject);
    }
    if ((Time.time - startTime) > 5)
    {
      vfx.SetInt("SpawnRate", 0);
      Color tmp = circle.material.color;
      tmp.a = Mathf.Max(1 - (Time.time - startTime - 5), 0);
      circle.material.SetColor("_BaseColor", tmp);
      print(circle.material.GetColor("_BaseColor"));
    }
  }

  void OnTriggerStay2D(Collider2D col)
  {
    if (col.gameObject.GetComponent<BoxDestruction>() != null && col.gameObject != gameObject)
    {
      BoxDestruction box = col.gameObject.GetComponent<BoxDestruction>();
      box.BlackHoleDestroy();
      Destroy(col.gameObject);
    }
  }
}
