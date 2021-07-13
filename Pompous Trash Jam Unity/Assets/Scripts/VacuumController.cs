using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VacuumController : MonoBehaviour
{
  [SerializeField] private TrashCounter trashCounter;
  [SerializeField] private string trash = "Trash";

  Rigidbody2D bone;
  VisualEffect vfx;
  // Start is called before the first frame update
  void Start()
  {
    bone = GetComponent<Rigidbody2D>();
    vfx = GameObject.Find("SuctionVFX").GetComponent<VisualEffect>();
    vfx.SetVector2("VFXCenter", bone.position); // set center initially and do not change it
  }

  void FixedUpdate()
  {
    if (Input.GetMouseButton(0))
    {
      vfx.SetInt("SpawnRate", 64);
    }
    else
    {
      vfx.SetInt("SpawnRate", 0);
    }
    Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    vfx.SetFloat("VFXAngle", Mathf.Deg2Rad * (bone.rotation + 90));
    Vector2 normal = (mousePos - bone.position).normalized;
    bone.AddForce(normal * 500);
  }

  void OnCollisionEnter2D(Collision2D col)
  {
    if (col.gameObject.layer == LayerMask.NameToLayer(trash) && col.gameObject.name != "Capsule Player")
    {
      trashCounter.Decrease(1);
      Destroy(col.gameObject);
    }
  }
}
