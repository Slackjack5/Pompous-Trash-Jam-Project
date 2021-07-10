using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashCounter : MonoBehaviour
{
  [SerializeField] private int maxCount = 100;
  [SerializeField] private GameObject box;
  [SerializeField] private Transform spawnArea;
  [SerializeField] private float spawnWidth;
  [SerializeField] private float spawnInterval = 0.5f;

  // Amount of trash remaining to collect
  private int currentCount;

  // Amount of trash remaining to spawn
  private int remainingSpawnCount;

  private bool isWaitingSpawn;

  // Start is called before the first frame update
  void Start()
  {
    currentCount = maxCount;
    remainingSpawnCount = maxCount;
  }

  // Update is called once per frame
  void Update()
  {
    if (currentCount <= 0)
    {
      // Player wins when there are zero trash left
      SceneManager.LoadScene("YouWin");
    }

    if (remainingSpawnCount > 0 && !isWaitingSpawn)
    {
      StartCoroutine(SetSpawnInterval());
    }
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(10, 10, 400, 30), "remainingSpawnCount: " + remainingSpawnCount);
  }

  public void Decrease(int value)
  {
    currentCount -= value;
  }

  private void Spawn(int value)
  {
    remainingSpawnCount -= value;

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

    Spawn(1);
  }
}
