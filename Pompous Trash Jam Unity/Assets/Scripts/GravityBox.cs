using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBox : SpecialBox
{
  [SerializeField] private GameObject wormhole;

  protected override void Destroy()
  {
    base.Destroy();

    Instantiate(wormhole, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);

    StartCoroutine(FreezeImpact());
  }
}
