using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityBox : SpecialBox
{
  [SerializeField] private GameObject box;
  [SerializeField] private GameObject infinityVFX;

  protected override void Destroy()
  {
    base.Destroy();

    Instantiate(box, new Vector2(transform.position.x + .5f, transform.position.y + .5f), Quaternion.identity);
    Instantiate(box, new Vector2(transform.position.x - .5f, transform.position.y + .5f), Quaternion.identity);
    Instantiate(box, new Vector2(transform.position.x, transform.position.y + .5f), Quaternion.identity);

    Instantiate(infinityVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

    StartCoroutine(FreezeImpact());
  }
}
