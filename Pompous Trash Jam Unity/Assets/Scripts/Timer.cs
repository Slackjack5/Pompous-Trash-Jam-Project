using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
  [SerializeField] private float maxTime = 100f;
  [SerializeField] private GameObject gameOverCanvas;

  private TextMeshProUGUI timerText;

  private float currentTimeLeft;

  // Start is called before the first frame update
  void Start()
  {
    timerText = GetComponent<TextMeshProUGUI>();

    currentTimeLeft = maxTime;
    gameOverCanvas.SetActive(false);
    Time.timeScale = 1;
  }

  // Update is called once per frame
  void Update()
  {
    timerText.text = FormatTime(currentTimeLeft);

    if (currentTimeLeft <= 0)
    {
      currentTimeLeft = 0;
      gameOverCanvas.SetActive(true);
      Time.timeScale = 0;
    }
    else
    {
      currentTimeLeft -= Time.deltaTime;
    }
  }

  private string FormatTime(float time)
  {
    return Mathf.CeilToInt(time).ToString();
  }
}
