using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  [SerializeField] private BoxSpawner[] boxes;
  [SerializeField] private GameObject defaultBox;
  [SerializeField] private float spawnWidth = 12f;
  [SerializeField] private float spawnInterval = 1.5f;

  [System.Serializable]
  private struct BoxSpawner
  {
    [SerializeField] internal GameObject box;
    [SerializeField, Range(0, 1f)] internal float spawnRate;
  }

  private struct CumulativeBoxSpawner
  {
    internal GameObject box;
    internal float cumulativeSpawnRate;
  }

  private bool isWaitingSpawn;
  private List<CumulativeBoxSpawner> spawnOptions;
  private float totalSpawnRate;

  private void Start()
  {
    spawnOptions = new List<CumulativeBoxSpawner>();
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
    // Rather than normalize all spawn rates to add up to 1, total the spawn rates
    // and use that total as the maximum of the range to pick a random number from
    float currentTotal = 0;
    spawnOptions.Clear();
    foreach (BoxSpawner boxSpawner in boxes)
    {
      GameObject box = boxSpawner.box;
      MinigameBox minigameBox = box.GetComponent<MinigameBox>();
      UpgradeBox upgradeBox = box.GetComponent<UpgradeBox>();
      if ((!minigameBox && !upgradeBox) || (minigameBox && !MinigameBoxExists()) || (upgradeBox && !UpgradeBoxExists()))
      {
        currentTotal += boxSpawner.spawnRate;
        spawnOptions.Add(new CumulativeBoxSpawner
        {
          box = box,
          cumulativeSpawnRate = currentTotal
        });
      }
    }

    totalSpawnRate = currentTotal;
  }

  private void Spawn()
  {
    float spawnHeight = transform.position.y;
    float min = transform.position.x - (spawnWidth / 2);
    float max = transform.position.x + (spawnWidth / 2);
    
    GameObject box = SelectBox();
    Vector2 spawnPosition = new Vector2(Random.Range(min, max), spawnHeight);
    Instantiate(box, spawnPosition, Quaternion.identity);
  }

  private GameObject SelectBox()
  {
    float r = Random.Range(0, totalSpawnRate);
    foreach (CumulativeBoxSpawner cumulativeBoxSpawner in spawnOptions)
    {
      if (r <= cumulativeBoxSpawner.cumulativeSpawnRate)
      {
        return cumulativeBoxSpawner.box;
      }
    }

    return defaultBox;
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
