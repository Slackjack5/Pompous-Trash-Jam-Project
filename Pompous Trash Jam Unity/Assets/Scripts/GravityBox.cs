using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBox : SpecialBox
{
  [SerializeField] private GameObject wormhole;

  protected override void PostFreeze()
  {
    base.PostFreeze();
    AkSoundEngine.PostEvent("Play_Wormhole", gameObject);
    Instantiate(wormhole, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
  }
}
