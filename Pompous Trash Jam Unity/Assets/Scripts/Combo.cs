using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Combo : MonoBehaviour
{
  [SerializeField] private float maxComboTime = 1f;
  [SerializeField] private int maxComboUpgradeCount = 10;
  [SerializeField] private int[] comboMultipliers;
  [SerializeField] private string[] exclamations;
  [SerializeField] private TextMeshProUGUI comboMultiplierText;
  [SerializeField] private TextMeshProUGUI shadowComboMultiplierText;
  [SerializeField] private TextMeshProUGUI exclamation;
  [SerializeField] private TextMeshProUGUI shadowExclamation;
  [SerializeField] private Image comboProgress;
  [SerializeField] private float maxScaleTime = 1f;
  [SerializeField] private float comboMaxSize = 1.2f;

  private RectTransform rectTransform;

  private float currentComboTime;
  private int currentComboUpgradeCount;
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

    comboProgress.fillAmount = (float) currentComboUpgradeCount / maxComboUpgradeCount;

    if (GameManager.IsGameActive)
    {
      if (currentScaleTime > 0)
      {
        currentScaleTime -= Time.deltaTime;
        ReduceSize();
      }

      if (currentComboTime > 0)
      {
        currentComboTime -= Time.deltaTime;
      }
      else
      {
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
        currentComboUpgradeCount = 0;
      }
    }

    Bulge();
  }

  private void Bulge()
  {
    rectTransform.localScale = new Vector3(comboMaxSize, comboMaxSize);
    currentScaleTime = maxScaleTime;
  }

  private void ReduceSize()
  {
    float t = Mathf.InverseLerp(0, maxScaleTime, currentScaleTime);
    float size = Mathf.Lerp(1, comboMaxSize, t);
    rectTransform.localScale = new Vector3(size, size);
  }
}
