using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
  [SerializeField] private GameObject cursor;

  public float CurrentAngle
  {
    get { return cursor.transform.localEulerAngles.z; }
  }
  public float SpinSpeed { get; set; }

  // Update is called once per frame
  void Update()
  {
    Vector3 angles = cursor.transform.localEulerAngles;

    angles.z -= SpinSpeed * Time.deltaTime;
    if (angles.z <= 0)
    {
      angles.z += 360;
    }

    cursor.transform.localEulerAngles = angles;
  }

  public void Restart()
  {
    cursor.transform.localEulerAngles = new Vector3(0, 0, 360);
  }
}
