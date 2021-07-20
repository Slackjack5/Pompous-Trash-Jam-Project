using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
  [SerializeField] private float maxTime = 100f;
  [SerializeField] private TextMeshProUGUI timerText;
  [SerializeField] private TextMeshProUGUI countdownText;
  [SerializeField] private string startLevelText = "Press Space to start";
  [SerializeField] private float maxFontSize = 72f;
  [SerializeField] private float minFontSize = 24f;
  [SerializeField] private float fadeTime = 1f;

  private float currentFadeTime;
  private int currentCountdownTime = 5;
  private bool isCountingDown = true;
  private bool isTimeDone = false;
  private float currentTimeLeft;
  private bool isTutorialSetup = false;

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

    if (GameManager.IsLevelStarted)
    {
      if (GameManager.IsTutorial)
      {
        if (!isTutorialSetup)
        {
          isTutorialSetup = true;

          countdownText.enabled = false;
          RemoveBackground();
          GameManager.ActivateGame();
        }
      }
      else
      {
        if (isCountingDown)
        {
          if (currentCountdownTime < 4)
          {
            DisplayCountdownTime();
            Fade();
          }
          else if (currentCountdownTime == 4)
          {
            countdownText.text = "Get Ready";
            Fade();
          }
          else
          {
            countdownText.text = "Preparing Room";
          }

          if (currentCountdownTime <= 0)
          {
            // Countdown is over
            StartCoroutine(DisplayGo());

            isCountingDown = false;

          }

          if (currentFadeTime > 0)
          {
            currentFadeTime -= Time.deltaTime;
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
              //Audio
              AkSoundEngine.PostEvent("Play_AnnouncerFinish", gameObject);
              AkSoundEngine.PostEvent("Stop_Music", gameObject);
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
    }
    else
    {
      countdownText.text = startLevelText;
    }
  }

  private void Fade()
  {
    // Have the countdown get smaller and more transparent over time
    float t = Mathf.InverseLerp(0, fadeTime, currentFadeTime);

    countdownText.fontSize = Mathf.Lerp(minFontSize, maxFontSize, t);

    Color color = countdownText.color;
    color.a = t;
    countdownText.color = color;
  }
  public void SubtractTime()
  {
    currentCountdownTime -= 1; //1f;/////Time.deltaTime;
    currentFadeTime = fadeTime;
  }
  private void DisplayCountdownTime()
  {
    countdownText.text = string.Format("{0}",(currentCountdownTime));

    RemoveBackground();
  }

  private void RemoveBackground()
  {
    // Make background color transparent
    Image image = countdownText.gameObject.GetComponentInParent<Image>();
    Color color = image.color;
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

    Color color = countdownText.color;
    color.a = 1;
    countdownText.color = color;

    countdownText.text = "GO";

    yield return new WaitForSeconds(.5f);
    GameManager.ActivateGame();
    countdownText.enabled = false;
  }
}
