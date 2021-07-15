using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  [SerializeField] private GameObject box;
  [SerializeField] private float spawnWidth = 12f;
  [SerializeField] private float spawnInterval = 0.5f;

  private bool isWaitingSpawn;

  // Update is called once per frame
  void Update()
  {
    if (GameManager.IsGameActive && !isWaitingSpawn)
    {
      StartCoroutine(SetSpawnInterval());
    }
  }

  private void Spawn()
  {
    float spawnHeight = transform.position.y;
    float min = transform.position.x - (spawnWidth / 2);
    float max = transform.position.x + (spawnWidth / 2);

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
