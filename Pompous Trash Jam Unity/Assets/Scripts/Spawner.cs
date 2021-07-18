using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  [SerializeField] private GameObject[] boxes;
  [SerializeField] private float spawnWidth = 12f;
  [SerializeField] private float spawnInterval = 1.5f;

  private bool isWaitingSpawn;
  private List<GameObject> spawnOptions;

  private void Start()
  {
    spawnOptions = new List<GameObject>();
  }

  // Update is called once per frame
  void Update()
  {
    if (GameManager.IsGameActive && !isWaitingSpawn)
    {
      StartCoroutine(SetSpawnInterval());
    }
  }

  private void EvaluateOptions()
  {
    foreach (GameObject box in boxes)
    {
      MinigameBox minigameBox = box.GetComponent<MinigameBox>();
      UpgradeBox upgradeBox = box.GetComponent<UpgradeBox>();
      if ((!minigameBox && !upgradeBox) || (minigameBox && !MinigameBoxExists()) || (upgradeBox && !UpgradeBoxExists()))
      {
        spawnOptions.Add(box);
      }
    }
  }

  private void Spawn()
  {
    float spawnHeight = transform.position.y;
    float min = transform.position.x - (spawnWidth / 2);
    float max = transform.position.x + (spawnWidth / 2);

    GameObject box = spawnOptions[Random.Range(0, spawnOptions.Count)];
    Vector2 spawnPosition = new Vector2(Random.Range(min, max), spawnHeight);
    Instantiate(box, spawnPosition, Quaternion.identity);
  }

  private IEnumerator SetSpawnInterval()
  {
    isWaitingSpawn = true;
    yield return new WaitForSeconds(spawnInterval);
    isWaitingSpawn = false;

    EvaluateOptions();
    Spawn();
  }

  private bool MinigameBoxExists()
  {
    return FindObjectOfType<MinigameBox>();
  }

  private bool UpgradeBoxExists()
  {
    return FindObjectOfType<UpgradeBox>();
  }
}
