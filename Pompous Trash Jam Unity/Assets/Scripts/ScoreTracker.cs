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
  [SerializeField] private TextMeshProUGUI comboMultiplierText;
  [SerializeField] private TextMeshProUGUI comboCountText;
  [SerializeField] private int bronzeScore;
  [SerializeField] private int silverScore;
  [SerializeField] private int goldScore;
  [SerializeField] private GameObject starPanel;
  [SerializeField] private GameObject star;
  [SerializeField] private TextMeshProUGUI highScoreText;

  private int comboMultiplier = 1;
  private float currentComboTime;
  private int currentComboUpgradeCount;
  private int highScore;
  private int score;

  private void Start()
  {
    highScore = PlayerPrefs.GetInt("highScore", 0);

    GameManager.levelComplete.AddListener(() =>
    {
      EvaluateScore();
    });
  }

  private void Update()
  {
    scoreText.text = score.ToString();
    comboMultiplierText.text = comboMultiplier + "x";
    comboCountText.text = currentComboUpgradeCount + " / " + maxComboUpgradeCount;
    highScoreText.text = highScore.ToString();

    if (GameManager.IsGameActive)
    {
      if (currentComboTime > 0)
      {
        currentComboTime -= Time.deltaTime;
      }
      else
      {
        comboMultiplier = 1;
        currentComboUpgradeCount = 0;
      }
    }
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(10, 10, 400, 30), "currentComboTime: " + currentComboTime);
  }

  public void Increase(int value)
  {
    currentComboTime = maxComboTime;
    currentComboUpgradeCount++;

    if (currentComboUpgradeCount >= maxComboUpgradeCount)
    {
      comboMultiplier++;
      currentComboUpgradeCount = 0;
    }

    score += value * baseMultiplier * comboMultiplier;

    if (score > highScore)
    {
      highScore = score;

      // Use PlayerPrefs to save values
      PlayerPrefs.SetInt("highScore", highScore);
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
