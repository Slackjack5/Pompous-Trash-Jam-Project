using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sequence : Minigame
{
  [SerializeField] private ProgressBar progressBar;
  [SerializeField] private TextMeshProUGUI firstButtonText;
  [SerializeField] private TextMeshProUGUI secondButtonText;
  [SerializeField] private TextMeshProUGUI thirdButtonText;

  [System.Serializable]
  private enum SequenceAction
  {
    UP, DOWN, LEFT, RIGHT
  }

  private SequenceAction[] sequenceActions = new SequenceAction[3];
  private int currentProgress = 0;

  private void Start()
  {
    progressBar.SetMaxValue(3);
    progressBar.SetValue(0);

    GenerateRandomSequence();

    progressBar.gameObject.SetActive(false);
  }

  protected override void DoUpdate()
  {
    if (progressBar.IsFull)
    {
      Complete();
      progressBar.gameObject.SetActive(false);
    }
  }

  public override void OnUp()
  {
    if (sequenceActions[currentProgress] == SequenceAction.UP)
    {
      Next();
    }
  }

  public override void OnDown()
  {
    if (sequenceActions[currentProgress] == SequenceAction.DOWN)
    {
      Next();
    }
  }

  public override void OnLeft()
  {
    if (sequenceActions[currentProgress] == SequenceAction.LEFT)
    {
      Next();
    }
  }

  public override void OnRight()
  {
    if (sequenceActions[currentProgress] == SequenceAction.RIGHT)
    {
      Next();
    }
  }

  public override void Restart()
  {
    base.Restart();

    progressBar.SetValue(0);
    currentProgress = 0;

    GenerateRandomSequence();

    progressBar.gameObject.SetActive(true);
  }

  private void GenerateRandomSequence()
  {
    for (int i = 0; i < sequenceActions.Length; i++)
    {
      sequenceActions[i] = (SequenceAction) Random.Range(0, 3);
    }

    firstButtonText.text = GetButtonText(sequenceActions[0]);
    secondButtonText.text = GetButtonText(sequenceActions[1]);
    thirdButtonText.text = GetButtonText(sequenceActions[2]);
  }

  private void Next()
  {
    currentProgress++;
    progressBar.SetValue(currentProgress);
  }

  private string GetButtonText(SequenceAction sequenceAction)
  {
    if (sequenceAction == SequenceAction.UP)
    {
      return "W";
    }
    else if (sequenceAction == SequenceAction.DOWN)
    {
      return "S";
    }
    else if (sequenceAction == SequenceAction.LEFT)
    {
      return "A";
    }
    else if (sequenceAction == SequenceAction.RIGHT)
    {
      return "D";
    }
    else
    {
      return "";
    }
  }
}
