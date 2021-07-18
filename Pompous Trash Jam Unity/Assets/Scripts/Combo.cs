using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Combo : MonoBehaviour
{
  [SerializeField] private float maxComboTime = 2f;
  [SerializeField] private int maxComboUpgradeCount = 10;
  [SerializeField] private int[] comboMultipliers;
  [SerializeField] private string[] exclamations;
  [SerializeField] private TextMeshProUGUI comboMultiplierText;
  [SerializeField] private TextMeshProUGUI shadowComboMultiplierText;
  [SerializeField] private TextMeshProUGUI exclamation;
  [SerializeField] private TextMeshProUGUI shadowExclamation;
  [SerializeField] private Image comboProgress;
  [SerializeField] private float maxFillTime = 0.2f;
  [SerializeField] private float maxScaleTime = 1f;
  [SerializeField] private float comboMaxSize = 1.2f;

  private RectTransform rectTransform;

  private float currentComboTime;
  private int currentComboUpgradeCount;
  private float startFillAmount;
  private float currentFillTime;
  private float currentResetFillTime;
  private float currentScaleTime;

  public int ComboMultiplierIndex { get; private set; }

  private void Start()
  {
    rectTransform = GetComponent<RectTransform>();
  }

  private void Update()
  {
    // Update text
    comboMultiplierText.text = comboMultipliers[ComboMultiplierIndex] + "x";
    shadowComboMultiplierText.text = comboMultipliers[ComboMultiplierIndex] + "x";
    exclamation.text = exclamations[ComboMultiplierIndex];
    shadowExclamation.text = exclamations[ComboMultiplierIndex];

    if (GameManager.IsGameActive)
    {
      if (currentScaleTime > 0)
      {
        currentScaleTime -= Time.deltaTime;
        ReduceSize();
      }

      if (currentFillTime > 0)
      {
        currentFillTime -= Time.deltaTime;
        Fill();
      }

      if (currentResetFillTime > 0)
      {
        currentResetFillTime -= Time.deltaTime;
        ReduceFill();
      }

      if (currentComboTime > 0)
      {
        currentComboTime -= Time.deltaTime;
      }
      else
      {
        if (currentComboUpgradeCount > 0)
        {
          ResetFill();
        }

        ComboMultiplierIndex = 0;
        currentComboUpgradeCount = 0;
      }
    }
  }

  public int GetMultiplier()
  {
    return comboMultipliers[ComboMultiplierIndex];
  }

  public void Hit()
  {
    currentComboTime = maxComboTime;
    currentComboUpgradeCount++;

    if (currentComboUpgradeCount >= maxComboUpgradeCount)
    {
      if (ComboMultiplierIndex == comboMultipliers.Length - 1)
      {
        // Combo multiplier is at its max, so maintain the max upgrade count
        currentComboUpgradeCount = maxComboUpgradeCount;
      }
      else
      {
        ComboMultiplierIndex++;
        //Audio
        if (ComboMultiplierIndex == 1)
        {
          AkSoundEngine.PostEvent("Play_AnnouncerGreat", gameObject);
        }
        else if (ComboMultiplierIndex == 2)
        {
          AkSoundEngine.PostEvent("Play_AnnouncerSuper", gameObject);
        }
        else if (ComboMultiplierIndex == 3)
        {
          AkSoundEngine.PostEvent("Play_AnnouncerOutstanding", gameObject);
        }
        else if (ComboMultiplierIndex == 4)
        {
          AkSoundEngine.PostEvent("Play_AnnouncerSensational", gameObject);
        }

        currentComboUpgradeCount = 0;
      }
    }

    Bulge();
    StartFill();
  }

  private void Bulge()
  {
    rectTransform.localScale = new Vector3(comboMaxSize, comboMaxSize);
    currentScaleTime = maxScaleTime;
  }

  private void Fill()
  {
    float t = Mathf.InverseLerp(maxFillTime, 0, currentFillTime);
    float fill = Mathf.Lerp(startFillAmount, (float) currentComboUpgradeCount / maxComboUpgradeCount, t);
    comboProgress.fillAmount = fill;
  }

  private void ReduceFill()
  {
    float t = Mathf.InverseLerp(0, maxFillTime, currentResetFillTime);
    float fill = Mathf.Lerp(0, startFillAmount, t);
    comboProgress.fillAmount = fill;
  }

  private void StartFill()
  {
    currentFillTime = maxFillTime;
    startFillAmount = comboProgress.fillAmount;
  }

  private void ResetFill()
  {
    currentResetFillTime = maxFillTime;
    startFillAmount = comboProgress.fillAmount;
  }

  private void ReduceSize()
  {
    float t = Mathf.InverseLerp(0, maxScaleTime, currentScaleTime);
    float size = Mathf.Lerp(1, comboMaxSize, t);
    rectTransform.localScale = new Vector3(size, size);
  }
}
