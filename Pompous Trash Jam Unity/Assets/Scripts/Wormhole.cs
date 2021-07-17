using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole : MonoBehaviour
{
  [SerializeField] private GameObject blackHoleVFX;
  [SerializeField] private Material blackHoleVFXMat;

  //Explosion
  public float fieldofImpact;
  public float force;
  public float torque;
  public LayerMask LayerToHit;

  // Start is called before the first frame update
  void Start()
  {
    Shader.SetGlobalFloat("_ShockTime", Time.time);
    Vector2 focalPoint = new Vector2(Camera.main.WorldToScreenPoint(transform.position, Camera.main.stereoActiveEye).x / Screen.width, Camera.main.WorldToScreenPoint(transform.position, Camera.main.stereoActiveEye).y / Screen.height);
    Shader.SetGlobalVector("_FocalPoint", focalPoint);
    GameObject temp = Instantiate(blackHoleVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    SpriteRenderer tempSR = temp.GetComponentInChildren<SpriteRenderer>();
    tempSR.material = new Material(blackHoleVFXMat);

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
