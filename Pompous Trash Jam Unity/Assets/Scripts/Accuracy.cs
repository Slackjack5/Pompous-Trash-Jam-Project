using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accuracy : Minigame
{
  [SerializeField] private Spinner spinner;
  [SerializeField] private float spinSpeed = 180f;
  [SerializeField] private HitRange[] hitRanges;

  [System.Serializable]
  private struct HitRange
  {
    [SerializeField] internal float start;
    [SerializeField] internal float end;
  }

  private void Start()
  {
    spinner.SpinSpeed = spinSpeed;
    spinner.gameObject.SetActive(false);
  }

  public override void OnFire()
  {
    foreach (HitRange hitRange in hitRanges)
    {
      if (spinner.CurrentAngle <= hitRange.start && spinner.CurrentAngle >= hitRange.end)
      {
        AkSoundEngine.PostEvent("Play_FixNoise", gameObject);
        Complete();
        spinner.gameObject.SetActive(false);
      }
    }
  }

  public override void Restart()
  {
    base.Restart();

    spinner.Restart();
    spinner.gameObject.SetActive(true);
  }
}
