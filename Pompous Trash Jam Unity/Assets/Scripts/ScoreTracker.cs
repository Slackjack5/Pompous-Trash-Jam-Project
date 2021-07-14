using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
  [SerializeField] private GameObject box;
  [SerializeField] private Transform spawnArea;
  [SerializeField] private float spawnWidth = 12f;
  [SerializeField] private float spawnInterval = 0.5f;
  [SerializeField] private int baseMultiplier = 100;
  [SerializeField] private float maxComboTime = 1f;
  [SerializeField] private int maxComboUpgradeCount = 10;
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private TextMeshProUGUI comboMultiplierText;
  [SerializeField] private TextMeshProUGUI comboCountText;

  private int comboMultiplier = 1;
  private float currentComboTime;
  private int currentComboUpgradeCount;
  private bool isWaitingSpawn;

  public int Score { get; private set; }

  // Update is called once per frame
  void Update()
  {
    scoreText.text = Score.ToString();
    comboMultiplierText.text = comboMultiplier + "x";
    comboCountText.text = currentComboUpgradeCount + " / " + maxComboUpgradeCount;

    if (GameManager.IsGameActive)
    {
      if (!isWaitingSpawn)
      {
        StartCoroutine(SetSpawnInterval());
      }
      
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

    Score += value * baseMultiplier * comboMultiplier;
  }

  private void Spawn()
  {
    float spawnHeight = spawnArea.position.y;
    float min = spawnArea.position.x - (spawnWidth / 2);
    float max = spawnArea.position.x + (spawnWidth / 2);

    Vector2 spawnPosition = new Vector2(Random.Range(min, max), spawnHeight);
    Instantiate(box, spawnPosition, Quaternion.identity);
  }

  private IEnumerator SetSpawnInterval()
  {
    isWaitingSpawn = true;
    yield return new WaitForSeconds(spawnInterval);
    isWaitingSpawn = false;

    Spawn();
  }
}
