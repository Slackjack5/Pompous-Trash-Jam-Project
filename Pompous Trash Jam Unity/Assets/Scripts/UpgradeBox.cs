using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBox : SpecialBox
{
  protected override void Destroy()
  {
    base.Destroy();

    int randNumber = Random.Range(0, 3);
    if (randNumber == 0)
    {
      Debug.Log("Spawned Power Up 1");
    }
    else if (randNumber == 1)
    {
      Debug.Log("Spawned Power Up 2");
    }
    else if (randNumber == 2)
    {
      Debug.Log("Spawned Power Up 3");
    }

    StartCoroutine(FreezeImpact());
  }
}
