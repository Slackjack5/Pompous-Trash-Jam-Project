using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBox : BoxDestruction
{
  [SerializeField] private float fieldOfImpact = 3f;
  [SerializeField] private float explosiveForce = 600f;
  [SerializeField] private LayerMask layerToHit;

  protected override void PreDestroy()
  {
    base.PreDestroy();

    Explosion();
  }

  protected void Explosion()
  {
    Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, layerToHit);
    foreach (Collider2D obj in objects)
    {
      PhysicsObject physicsObject = obj.GetComponent<PhysicsObject>();
      if (physicsObject)
      {
        physicsObject.Launch(transform.position, explosiveForce);
      }
    }
  }
}
