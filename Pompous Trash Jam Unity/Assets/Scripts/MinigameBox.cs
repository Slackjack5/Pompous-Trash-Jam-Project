using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBox : BoxDestruction
{
  protected override void PreDestroy()
  {
    GameManager.StartRandomMinigame();

    GameManager.CurrentMinigame.complete.AddListener(() =>
    {
      base.PreDestroy();
      Destroy(gameObject);
    });
  }

  protected override void Destroy()
  {
    PreDestroy();
  }

  public override void EnvironmentalDestroy()
  {
    base.PreDestroy();
    Destroy(gameObject);
  }
}
