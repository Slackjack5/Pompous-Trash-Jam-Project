using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole : MonoBehaviour
{
  [SerializeField] private GameObject blackHoleVFX;
  [SerializeField] private Material blackHoleVFXMat;
  [SerializeField] private float duration = 5f;

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
    temp.GetComponent<BlackholeVFX>().Initialize(duration);
    SpriteRenderer tempSR = temp.GetComponentInChildren<SpriteRenderer>();
    tempSR.material = new Material(blackHoleVFXMat);

    StartCoroutine("EndWormHole");
    //Audio
    AkSoundEngine.PostEvent("Pause_Music", gameObject);
    AkSoundEngine.PostEvent("LowPass_Music", gameObject);
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
    AkSoundEngine.PostEvent("Resume_Music", gameObject);
  }

  // Update is called once per frame
  void Update()
  {

  }

  IEnumerator EndWormHole()
  {
    yield return new WaitForSeconds(duration);
    Explosion();
    Destroy(gameObject);
  }
}
