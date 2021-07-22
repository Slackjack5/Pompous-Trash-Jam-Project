using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      StartCoroutine(eraseParticle());
  }

    // Update is called once per frame
    void Update()
    {
        
    }

  IEnumerator eraseParticle()
  {
    yield return new WaitForSeconds(10f);
    Destroy(gameObject);
  }
}
