using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonMash : Minigame
{
  [SerializeField] private ProgressBar progressBar;
  [SerializeField] private float increaseAmount = 50f;
  [SerializeField] private float decreaseInterval = 0.01f;

  private void Start()
  {
    progressBar.SetMaxValue(100);
    progressBar.SetValue(0);
    progressBar.gameObject.SetActive(false);
  }

  protected override void DoUpdate()
  {
    if (progressBar.IsFull)
    {
      Complete();
      progressBar.gameObject.SetActive(false);
    }
    else
    {
      progressBar.Decrease(Time.deltaTime / decreaseInterval);
    }
  }

  public override void OnFire()
  {
    progressBar.Increase(increaseAmount);
  }

  public override void Restart()
  {
    base.Restart();
    progressBar.SetValue(0);
    progressBar.gameObject.SetActive(true);
  }
}
