using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableRigidBody : PhysicsObject
{
  [SerializeField] Vector2 forceDirection;

  // Start is called before the first frame update
  protected override void Start()
  {
    base.Start();

    float randTorque = Random.Range(-25, 25);
    float randTorqueX = Random.Range(forceDirection.x - 50, forceDirection.x + 50);
    float randTorqueY = Random.Range(forceDirection.y, forceDirection.y + 50);

    forceDirection.x = randTorqueX;
    forceDirection.y = randTorqueY;
    rb.AddForce(forceDirection);
    rb.AddTorque(randTorque);
  }
}
