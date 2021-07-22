using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBox : BoxDestruction
{
  [SerializeField] private float fieldOfImpact = 3f;
  [SerializeField] private float explosiveForce = 600f;
  [SerializeField] private LayerMask layerToHit;

  protected Collider2D[] hitObjects;

  protected override void PreFreeze()
  {
    base.PreFreeze();

    Explosion();
  }

  protected void Explosion()
  {
    hitObjects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, layerToHit);
    foreach (Collider2D obj in hitObjects)
    {
      PhysicsObject physicsObject = obj.GetComponent<PhysicsObject>();
      if (physicsObject)
      {
        physicsObject.Launch(transform.position, explosiveForce);
      }
    }
  }
}
