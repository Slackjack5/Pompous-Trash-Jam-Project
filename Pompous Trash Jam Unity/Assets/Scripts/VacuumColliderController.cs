using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumColliderController : MonoBehaviour
{
  public Rigidbody2D vacuumBone;
  void OnTriggerStay2D(Collider2D col)
  {
    if (GameManager.IsGameActive && !GameManager.TubeMinigame.IsReady && col.attachedRigidbody != null && Input.GetMouseButton(0) && col.gameObject.name != "Capsule Player")
    {
      float dist = Vector2.Distance(col.attachedRigidbody.transform.position, vacuumBone.transform.position);
      dist = Mathf.Max(dist, 0.2f);
      Vector2 normal = (vacuumBone.transform.position - col.attachedRigidbody.transform.position).normalized;
      float forceMult = Mathf.Min(1 / dist, 1f) * 75;
      col.attachedRigidbody.AddForce(normal * forceMult);
    }
  }
}
