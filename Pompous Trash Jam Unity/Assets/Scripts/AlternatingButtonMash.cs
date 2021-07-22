using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingButtonMash : Minigame
{
  [SerializeField] private ProgressBar progressBar;
  [SerializeField] private float increaseAmount = 25f;
  [SerializeField] private float decreaseInterval = 0.01f;

  private bool isLeftNext = true;

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

  public override void OnLeft()
  {
    if (isLeftNext)
    {
      AkSoundEngine.PostEvent("Play_FixNoise", gameObject);
      progressBar.Increase(increaseAmount);
      isLeftNext = false;
    }
  }

  public override void OnRight()
  {
    if (!isLeftNext)
    {
      AkSoundEngine.PostEvent("Play_FixNoise", gameObject);
      progressBar.Increase(increaseAmount);
      isLeftNext = true;
    }
  }

  public override void Restart()
  {
    base.Restart();
    progressBar.SetValue(0);
    progressBar.gameObject.SetActive(true);
  }
}
