using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VacuumController : MonoBehaviour
{
  Rigidbody2D bone;
  VisualEffect vfx;
  // Start is called before the first frame update
  void Start()
  {
    bone = GetComponent<Rigidbody2D>();
    vfx = GameObject.Find("SuctionVFX").GetComponent<VisualEffect>();
  }

  void FixedUpdate()
  {
    if (Input.GetMouseButton(0))
    {
      vfx.SetInt("SpawnRate", 16);
    }
    else
    {
      vfx.SetInt("SpawnRate", 0);
    }
    Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    vfx.SetVector2("VFXCenter", bone.transform.position);
    Vector2 normal = (mousePos - bone.position).normalized;
    bone.AddForce(normal * 500);
    /*Debug.DrawLine(vacuumPos, mousePos, Color.black, Time.fixedDeltaTime);
    RaycastHit2D[] boxCasts = Physics2D.BoxCastAll(new Vector2(suctionHeight / 2, 0), new Vector2(suctionWidth, suctionHeight), transform.rotation.eulerAngles.z - 360, transform.forward);
    foreach (RaycastHit2D hit in boxCasts)
    {
      hit.collider.CreateMesh(true, true);
      if (hit.rigidbody != null)
      {
        Vector2 hitPos = new Vector2(hit.rigidbody.transform.position.x, hit.rigidbody.transform.position.y);
        normal = (vacuumPos - hitPos).normalized;
        hit.rigidbody.AddForce(normal * 100);
      }
    }*/
  }
}
