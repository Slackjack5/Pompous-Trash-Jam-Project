using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperStrength : Powerup
{
  [SerializeField] private float hitForce = 1500f;

  public float HitForce { get { return hitForce; } }

  private void Start()
  {
    powerupType = PowerupType.SUPER_STRENGTH;
  }
}
