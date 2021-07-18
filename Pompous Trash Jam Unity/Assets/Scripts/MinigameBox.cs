using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBox : BoxDestruction
{
  protected override void PreDestroy()
  {
    base.PreDestroy();

    GameManager.StartRandomMinigame();
  }

  protected override void Destroy()
  {
    PreDestroy();
    Destroy(gameObject);
  }
}
