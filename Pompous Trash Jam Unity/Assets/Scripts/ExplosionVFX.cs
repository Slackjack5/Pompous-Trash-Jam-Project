using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ExplosionVFX : MonoBehaviour
{
  VisualEffect vfx;
  float startTime;
  // Start is called before the first frame update
  void Start()
  {
    startTime = Time.time;
    vfx = GetComponent<VisualEffect>();
  }

  // Update is called once per frame
  void Update()
  {
    if ((Time.time - startTime) > 5)
    {
      Destroy(gameObject);
    }
  }
}
