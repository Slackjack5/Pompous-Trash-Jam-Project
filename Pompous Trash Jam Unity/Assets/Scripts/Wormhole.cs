using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole : MonoBehaviour
{
    //Explosion
    public float fieldofImpact;
    public float force;
    public float torque;
    public LayerMask LayerToHit;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("EndWormHole");
    }

    private void Explosion()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldofImpact, LayerToHit);
        foreach (Collider2D obj in objects)
        {
            float randTorque = Random.Range(-25, 25);
            Vector2 direction = obj.transform.position - transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
            obj.GetComponent<Rigidbody2D>().AddForce(transform.up * force / 2);
            obj.GetComponent<Rigidbody2D>().AddTorque(randTorque);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EndWormHole()
    {

        yield return new WaitForSeconds(5f);
        Explosion();
        Destroy(gameObject);
    }
}
