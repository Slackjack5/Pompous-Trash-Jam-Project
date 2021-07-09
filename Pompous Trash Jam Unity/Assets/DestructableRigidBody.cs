using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableRigidBody : MonoBehaviour
{

    [SerializeField] Vector2 forceDirection;

    [SerializeField] float torque;

    [SerializeField] Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        float randTorque = Random.Range(-25, 25);
        float randTorqueX = Random.Range(forceDirection.x - 50, forceDirection.x + 50);
        float randTorqueY = Random.Range(forceDirection.y, forceDirection.y + 50);

        forceDirection.x = randTorqueX;
        forceDirection.y = randTorqueY;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(forceDirection);
        rb2d.AddTorque(randTorque);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
