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
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private int baseMultiplier = 100;

  private bool isWaitingSpawn;

  public int Score { get; private set; }

  // Update is called once per frame
  void Update()
  {
    scoreText.text = Score.ToString();

    if (GameManager.IsGameActive && !isWaitingSpawn)
    {
      StartCoroutine(SetSpawnInterval());
    }
  }

  public void Increase(int value)
  {
    Score += value * baseMultiplier;
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
