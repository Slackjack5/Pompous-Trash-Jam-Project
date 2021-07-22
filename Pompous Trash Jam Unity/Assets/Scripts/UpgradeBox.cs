using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBox : SpecialBox
{
  [SerializeField] private Powerup[] powerups;

  protected override void PreFreeze()
  {
    base.PreFreeze();

    PowerupManager.Instance.Activate(powerups[Random.Range(0, powerups.Length)]);
  }
}
