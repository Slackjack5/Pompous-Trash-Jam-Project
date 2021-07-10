using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
  [SerializeField] private float maxTime = 100f;
  [SerializeField] private GameObject gameOverCanvas;
  [SerializeField] private TextMeshProUGUI countdownText;
  [SerializeField] private float maxFontSize = 72f;
  [SerializeField] private float minFontSize = 24f;

  private TextMeshProUGUI timerText;

  private float currentCountdownTime = 3f;
  private bool isCountingDown = true;
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

    if (isCountingDown)
    {
      DisplayCountdownTime();
      currentCountdownTime -= Time.deltaTime;

      if (currentCountdownTime <= 0)
      {
        // Countdown is over
        StartCoroutine(DisplayGo());

        isCountingDown = false;
        GameManager.IsGameActive = true;
      }
    }
    else
    {
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
  }

  private void DisplayCountdownTime()
  {
    countdownText.enabled = true;

    // Have the countdown get smaller and more transparent over time
    float interpolationValue = Mathf.InverseLerp(0, 1, currentCountdownTime % 1);

    countdownText.fontSize = Mathf.Lerp(minFontSize, maxFontSize, interpolationValue);

    Color color = countdownText.color;
    color.a = interpolationValue;
    countdownText.color = color;

    countdownText.text = string.Format("{0}", Mathf.CeilToInt(currentCountdownTime));
  }

  private string FormatTime(float time)
  {
    return Mathf.CeilToInt(time).ToString();
  }

  private IEnumerator DisplayGo()
  {
    countdownText.fontSize = maxFontSize;

    Color color = countdownText.color;
    color.a = 1;
    countdownText.color = color;

    countdownText.text = "GO";

    yield return new WaitForSeconds(1f);

    countdownText.enabled = false;
  }
}
