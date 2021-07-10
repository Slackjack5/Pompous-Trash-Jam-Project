using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDestruction : MonoBehaviour
{
  public GameObject destructable;

  public void Destroy()
  {
    Instantiate(destructable, transform.position, Quaternion.identity);
    DestroyGameObject();
  }

  private void DestroyGameObject()
  {
    Destroy(gameObject);
  }
}
