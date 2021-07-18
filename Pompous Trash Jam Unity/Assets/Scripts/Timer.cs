using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
  [SerializeField] private float maxTime = 100f;
  [SerializeField] private TextMeshProUGUI timerText;
  [SerializeField] private TextMeshProUGUI shadowTimerText;
  [SerializeField] private TextMeshProUGUI countdownText;
  [SerializeField] private TextMeshProUGUI shadowCountdownText;
  [SerializeField] private string startLevelText = "Press Space to start";
  [SerializeField] private float maxFontSize = 72f;
  [SerializeField] private float minFontSize = 24f;

  private float currentCountdownTime = 3f;
  private bool isCountingDown = true;
  private bool isTimeDone = false;
  private float currentTimeLeft;

  // Start is called before the first frame update
  void Start()
  {
    currentTimeLeft = maxTime;
    
    Time.timeScale = 1;
  }

  // Update is called once per frame
  void Update()
  {
    timerText.text = FormatTime(currentTimeLeft);
    shadowTimerText.text = FormatTime(currentTimeLeft);

    if (GameManager.IsLevelStarted)
    {
      if (isCountingDown)
      {
        DisplayCountdownTime();
        currentCountdownTime -= Time.deltaTime;

        if (currentCountdownTime <= 0)
        {
          // Countdown is over
          StartCoroutine(DisplayGo());

          isCountingDown = false;
          GameManager.ActivateGame();
        }
      }
      else
      {
        if (currentTimeLeft <= 0)
        {
          if (!isTimeDone)
          {
            // Player ran out of time
            isTimeDone = true;
            currentTimeLeft = 0;

            GameManager.EndLevel();
          }
          else
          {
            currentTimeLeft = 0;
          }
        }
        else
        {
          currentTimeLeft -= Time.deltaTime;
        }
      }
    }
    else
    {
      countdownText.text = startLevelText;
      shadowCountdownText.text = startLevelText;
    }
  }

  private void DisplayCountdownTime()
  {
    // Have the countdown get smaller and more transparent over time
    float t = Mathf.InverseLerp(0, 1, currentCountdownTime % 1);

    countdownText.fontSize = Mathf.Lerp(minFontSize, maxFontSize, t);
    shadowCountdownText.fontSize = countdownText.fontSize;

    Color color = countdownText.color;
    color.a = t;
    countdownText.color = color;

    color = shadowCountdownText.color;
    color.a = t;
    shadowCountdownText.color = color;

    countdownText.text = string.Format("{0}", Mathf.CeilToInt(currentCountdownTime));
    shadowCountdownText.text = countdownText.text;

    // Make background color transparent
    Image image = countdownText.gameObject.GetComponentInParent<Image>();
    color = image.color;
    color.a = 0;
    image.color = color;
  }

  private string FormatTime(float time)
  {
    return Mathf.CeilToInt(time).ToString();
  }

  private IEnumerator DisplayGo()
  {
    countdownText.fontSize = maxFontSize;
    shadowCountdownText.fontSize = maxFontSize;

    Color color = countdownText.color;
    color.a = 1;
    countdownText.color = color;

    color = shadowCountdownText.color;
    color.a = 1;
    shadowCountdownText.color = color;

    countdownText.text = "GO";
    shadowCountdownText.text = "GO";

    yield return new WaitForSeconds(1f);

    countdownText.enabled = false;
    shadowCountdownText.enabled = false;
  }
}
