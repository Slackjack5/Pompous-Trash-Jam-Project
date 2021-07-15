using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
  [SerializeField] private TextMeshProUGUI starText;
  [SerializeField] private TextMeshProUGUI highScoreText;

  private int comboMultiplier = 1;
  private float currentComboTime;
  private int currentComboUpgradeCount;
  private int highScore;
  private int score;

  private void Start()
  {
    highScore = PlayerPrefs.GetInt("highScore", 0);
  }

  private void Update()
  {
    scoreText.text = score.ToString();
    comboMultiplierText.text = comboMultiplier + "x";
    comboCountText.text = currentComboUpgradeCount + " / " + maxComboUpgradeCount;
    starText.text = EvaluateScore();
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

  private string EvaluateScore()
  {
    if (score >= goldScore)
    {
      return "3 Stars";
    }
    else if (score >= silverScore)
    {
      return "2 Stars";
    }
    else if (score >= bronzeScore)
    {
      return "1 Star";
    }
    else
    {
      return "0 Stars";
    }
  }
}
