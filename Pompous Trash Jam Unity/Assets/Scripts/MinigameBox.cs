using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBox : BoxDestruction
{
  [SerializeField] private Minigame minigame;

  protected override void PreDestroy()
  {
    base.PreDestroy();

    GameManager.StartMinigame(minigame);
  }

  protected override void Destroy()
  {
    PreDestroy();
    Destroy(gameObject);
  }
}
