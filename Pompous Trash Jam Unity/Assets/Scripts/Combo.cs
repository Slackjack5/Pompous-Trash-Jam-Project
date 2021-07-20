using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class Combo : MonoBehaviour
{
  [SerializeField] private float maxComboTime = 2f;
  [SerializeField] private int maxComboUpgradeCount = 10;
  [SerializeField] private int[] comboMultipliers;
  [SerializeField] private string[] exclamations;
  [SerializeField] private TextMeshProUGUI comboMultiplierText;
  [SerializeField] private TextMeshProUGUI exclamation;
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
  public ParticleSystem particles;
  public Volume volume;
  private Bloom bloom;
  private ChromaticAberration chromatic;
  private ColorAdjustments colorAdjustments;
  private void Start()
  {
    rectTransform = GetComponent<RectTransform>();
    volume.profile.TryGet(out bloom);
    volume.profile.TryGet(out chromatic);
    volume.profile.TryGet(out colorAdjustments);
  }

  private void Update()
  {
    // Update text
    comboMultiplierText.text = comboMultipliers[ComboMultiplierIndex] + "x";
    exclamation.text = exclamations[ComboMultiplierIndex];
    float comboRatio = (float)ComboMultiplierIndex / (float)(comboMultipliers.Length - 1);
    print(comboRatio);
    if (comboRatio >= 0.99)
    {
      if (bloom.intensity.value != 100)
      {
        // Place audio here
        Shader.SetGlobalFloat("_ShockTime", Time.time);
        Shader.SetGlobalVector("_FocalPoint", new Vector2(0.5f, 0.5f));
      }
      bloom.intensity.value = 100;
      chromatic.intensity.value = 1;
      colorAdjustments.contrast.value = 100;
      colorAdjustments.hueShift.value = 180;
      colorAdjustments.saturation.value = 100;
    }
    else
    {
      bloom.intensity.value = 5;
      chromatic.intensity.value = 0;
      colorAdjustments.contrast.value = 0;
      colorAdjustments.hueShift.value = 0;
      colorAdjustments.saturation.value = 0;
    }
    particles.startSpeed = comboRatio * 200f;
    particles.emissionRate = comboRatio * 200f;

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
    //Sound
    AkSoundEngine.PostEvent("Play_CollectStereo", gameObject);
    AkSoundEngine.SetRTPCValue("Combo", currentComboUpgradeCount, gameObject);

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
        AkSoundEngine.PostEvent("Play_NextStage", gameObject);
        AkSoundEngine.ResetRTPCValue("Combo");
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
    float fill = Mathf.Lerp(startFillAmount, (float)currentComboUpgradeCount / maxComboUpgradeCount, t);
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
