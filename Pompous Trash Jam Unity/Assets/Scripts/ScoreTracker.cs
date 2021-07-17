using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
  [SerializeField] private int baseMultiplier = 100;
  [SerializeField] private Combo combo;
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private TextMeshProUGUI shadowScoreText;
  [SerializeField] private int bronzeScore;
  [SerializeField] private int silverScore;
  [SerializeField] private int goldScore;
  [SerializeField] private GameObject starPanel;
  [SerializeField] private GameObject star;
  [SerializeField] private TextMeshProUGUI highScoreText;
  [SerializeField] private GameObject pointsCanvas;
  [SerializeField] private GameObject pointsObject;
  [SerializeField] private Transform tubeEntry;

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
    // Update text
    scoreText.text = score.ToString();
    shadowScoreText.text = score.ToString();
    highScoreText.text = highScore.ToString();
  }

  public void Increase(int value)
  {
    combo.Hit();

    int points = value * baseMultiplier * combo.GetMultiplier();
    score += points;

    SpawnPoints(points);

    if (score > highScore)
    {
      highScore = score;

      // Use PlayerPrefs to save values
      PlayerPrefs.SetInt("highScore", highScore);
    }
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
