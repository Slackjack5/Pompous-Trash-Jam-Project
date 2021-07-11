using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumColliderController : MonoBehaviour
{
  public Rigidbody2D vacuumBone;
  void OnTriggerStay2D(Collider2D col)
  {
    if (col.attachedRigidbody != null && Input.GetMouseButton(0))
    {
      float dist = Vector2.Distance(col.attachedRigidbody.transform.position, vacuumBone.transform.position);
      Vector2 normal = (vacuumBone.transform.position - col.attachedRigidbody.transform.position).normalized;
      float forceMult = Mathf.Min(1 / dist, 1f) * 125;
      col.attachedRigidbody.AddForce(normal * forceMult);
    }
    print("inside trigger");
  }
}
