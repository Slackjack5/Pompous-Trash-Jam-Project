using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
  [SerializeField] private int baseMultiplier = 100;
  [SerializeField] private float maxComboTime = 1f;
  [SerializeField] private int maxComboUpgradeCount = 10;
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private TextMeshProUGUI shadowScoreText;
  [SerializeField] private TextMeshProUGUI comboMultiplierText;
  [SerializeField] private TextMeshProUGUI shadowComboMultiplierText;
  [SerializeField] private TextMeshProUGUI exclamation;
  [SerializeField] private TextMeshProUGUI shadowExclamation;
  [SerializeField] private int bronzeScore;
  [SerializeField] private int silverScore;
  [SerializeField] private int goldScore;
  [SerializeField] private GameObject starPanel;
  [SerializeField] private GameObject star;
  [SerializeField] private TextMeshProUGUI highScoreText;
  [SerializeField] private int[] comboMultipliers;
  [SerializeField] private string[] exclamations;
  [SerializeField] private Image comboProgress;
  [SerializeField] private GameObject comboPanel;
  [SerializeField] private float comboMaxSize = 1.2f;
  [SerializeField] private GameObject pointsCanvas;
  [SerializeField] private GameObject pointsObject;
  [SerializeField] private Transform tubeEntry;
  [SerializeField] private float pointsDuration = 1f;

  private RectTransform comboRectTransform;

  private int comboMultiplierIndex;
  private float currentComboTime;
  private int currentComboUpgradeCount;
  private float currentScaleTime;
  private int highScore;
  private int score;

  private void Start()
  {
    comboRectTransform = comboPanel.GetComponent<RectTransform>();

    highScore = PlayerPrefs.GetInt("highScore", 0);

    GameManager.levelComplete.AddListener(() =>
    {
      EvaluateScore();
    });
  }

  private void Update()
  {
    // Update text
    scoreText.text = score.ToString();
    shadowScoreText.text = score.ToString();
    comboMultiplierText.text = comboMultipliers[comboMultiplierIndex] + "x";
    shadowComboMultiplierText.text = comboMultipliers[comboMultiplierIndex] + "x";
    highScoreText.text = highScore.ToString();
    exclamation.text = exclamations[comboMultiplierIndex];
    shadowExclamation.text = exclamations[comboMultiplierIndex];

    comboProgress.fillAmount = (float) currentComboUpgradeCount / maxComboUpgradeCount;

    if (GameManager.IsGameActive)
    {
      if (currentScaleTime > 0)
      {
        currentScaleTime -= Time.deltaTime;
        RestoreSize();
      }

      if (currentComboTime > 0)
      {
        currentComboTime -= Time.deltaTime;
      }
      else
      {
        comboMultiplierIndex = 0;
        currentComboUpgradeCount = 0;
      }
    }
  }

  public void Increase(int value)
  {
    currentComboTime = maxComboTime;
    currentComboUpgradeCount++;
    Bulge();

    if (currentComboUpgradeCount >= maxComboUpgradeCount)
    {
      if (comboMultiplierIndex == comboMultipliers.Length - 1)
      {
        currentComboUpgradeCount = maxComboUpgradeCount;
      }
      else
      {
        comboMultiplierIndex++;
        currentComboUpgradeCount = 0;
      }
    }

    int points = value * baseMultiplier * comboMultipliers[comboMultiplierIndex];
    score += points;

    SpawnPoints(points);

    if (score > highScore)
    {
      highScore = score;

      // Use PlayerPrefs to save values
      PlayerPrefs.SetInt("highScore", highScore);
    }
  }

  private void Bulge()
  {
    comboRectTransform.localScale = new Vector3(comboMaxSize, comboMaxSize);
    currentScaleTime = 1f;
  }

  private void RestoreSize()
  {
    float size = Mathf.Lerp(1, comboMaxSize, currentScaleTime);
    comboRectTransform.localScale = new Vector3(size, size);
  }

  private void SpawnPoints(int points)
  {
    GameObject newPointsObject = Instantiate(pointsObject, pointsCanvas.transform);

    RectTransform rectTransform = newPointsObject.GetComponent<RectTransform>();
    Vector2 viewportPoint = Camera.main.WorldToViewportPoint(tubeEntry.position);
    rectTransform.anchorMin = viewportPoint;
    rectTransform.anchorMax = viewportPoint;

    TextMeshProUGUI[] texts = newPointsObject.GetComponentsInChildren<TextMeshProUGUI>();
    foreach (TextMeshProUGUI text in texts)
    {
      text.text = "+" + points;
    }
  }

  private void EvaluateScore()
  {
    int numStars;
    if (score >= goldScore)
    {
      numStars = 3;
    }
    else if (score >= silverScore)
    {
      numStars = 2;
    }
    else if (score >= bronzeScore)
    {
      numStars = 1;
    }
    else
    {
      numStars = 0;
    }

    for (int i = 0; i < numStars; i++)
    {
      GameObject newStar = Instantiate(star);
      RectTransform rectTransform = newStar.GetComponent<RectTransform>();
      rectTransform.SetParent(starPanel.transform);
    }
  }
}
