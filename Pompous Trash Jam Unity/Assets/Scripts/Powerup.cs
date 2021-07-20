using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
  [SerializeField] private float duration;
  
  protected PowerupType powerupType;

  public enum PowerupType
  {
    SUPER_STRENGTH
  }

  public float Duration
  {
    get { return duration; }
  }

  public PowerupType Type 
  {
    get { return powerupType; }
  }
}
