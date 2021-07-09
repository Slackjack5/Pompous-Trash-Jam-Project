using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumController : MonoBehaviour
{
  Rigidbody2D bone;
  // Start is called before the first frame update
  void Start()
  {
    bone = GetComponent<Rigidbody2D>();
  }

  void FixedUpdate()
  {
    if (Input.GetMouseButton(0))
    {
      Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
      Vector2 vacuumPos = new Vector2(transform.position.x, transform.position.y);
      Vector2 normal = (mousePos - vacuumPos).normalized;
      bone.AddForce(normal * 500);
      Debug.DrawLine(vacuumPos, mousePos, Color.black, Time.fixedDeltaTime);
      print("force added");
    }
  }
}
