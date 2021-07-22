using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBox : BoxDestruction
{
  protected override void PreFreeze()
  {
    GameManager.StartRandomMinigame();

    GameManager.CurrentMinigame.complete.AddListener(() =>
    {
      base.PreFreeze();
      Destroy(gameObject);
    });
  }

  protected override void Destroy()
  {
    PreFreeze();
  }

  public override void EnvironmentalDestroy()
  {
    base.PreFreeze();
    Destroy(gameObject);
  }
}
