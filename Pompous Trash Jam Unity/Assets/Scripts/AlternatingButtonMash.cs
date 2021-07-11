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

  private void Update()
  {
    if (progressBar.IsFull)
    {
      complete.Invoke();
      progressBar.gameObject.SetActive(false);
    }

    progressBar.Decrease(Time.deltaTime / decreaseInterval);
  }

  public override void OnLeft()
  {
    if (isLeftNext)
    {
      progressBar.Increase(increaseAmount);
      isLeftNext = false;
    }
  }

  public override void OnRight()
  {
    if (!isLeftNext)
    {
      progressBar.Increase(increaseAmount);
      isLeftNext = true;
    }
  }

  public override void Restart()
  {
    progressBar.gameObject.SetActive(true);
    progressBar.SetValue(0);
  }
}
