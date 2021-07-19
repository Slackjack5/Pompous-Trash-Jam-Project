using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  [SerializeField] private ButtonMash buttonMashMinigame;
  [SerializeField] private Accuracy accuracyMinigame;
  [SerializeField] private AlternatingButtonMash alternatingButtonMashMinigame;
  [SerializeField] private Sequence sequenceMinigame;
  [SerializeField] private TubeMinigame tubeMinigame;
  [SerializeField] private GameObject pauseMenuCanvas;

  public static readonly UnityEvent levelComplete = new UnityEvent();

  public static bool IsLevelStarted { get; private set; }

  public static bool IsGameActive { get; private set; }

  public static bool IsMinigameActive {
    get { return CurrentMinigame != null; }
  }

  public static Minigame CurrentMinigame { get; private set; }

  private static Vector2 defaultGravity = new Vector2(0, -9.8f);
  private static bool isLevelComplete = false;
  private static bool isPaused = false;
  private static Minigame[] minigames;
  private static GameObject pauseMenu;

  private void Start()
  {
    IsLevelStarted = false;

    DeactivateGame();

    minigames = new Minigame[] { buttonMashMinigame, accuracyMinigame, alternatingButtonMashMinigame, sequenceMinigame };
    tubeMinigame.triggered.AddListener(() => StartMinigame(tubeMinigame));

    isPaused = false;
    pauseMenu = pauseMenuCanvas;
    pauseMenu.SetActive(false);

    isLevelComplete = false;
  }

  public static void ActivateGame()
  {
    IsGameActive = true;
    Physics2D.gravity = defaultGravity;
  }

  public static void DeactivateGame()
  {
    IsGameActive = false;
    Physics2D.gravity = Vector2.zero;
  }

  private static void DeactivateMinigame()
  {
    CurrentMinigame = null;
  }

  public static void StartLevel()
  {
    IsLevelStarted = true;
  }

  public static void EndLevel()
  {
    DeactivateGame();
    DeactivateMinigame();

    levelComplete.Invoke();
    isLevelComplete = true;
    GameObject.Find("WwiseGlobal").GetComponent<AudioEvents>().FadeAudio();
  }

  public static void EndMinigame()
  {
    ActivateGame();

    CurrentMinigame = null;
  }

  public static void StartMinigame(Minigame minigame)
  {
    if (!IsMinigameActive)
    {
      DeactivateGame();

      minigame.complete.AddListener(() => EndMinigame());
      minigame.Restart();
      CurrentMinigame = minigame;
    }
  }

  public static void StartRandomMinigame()
  {
    StartMinigame(minigames[Random.Range(0, minigames.Length)]);
  }

  public static void TogglePause()
  {
    if (!isLevelComplete)
    {
      if (isPaused)
      {
        Resume();
        isPaused = false;
      }
      else
      {
        Pause();
        isPaused = true;
      }
    }
  }

  public static void FreezeTime()
  {
    Time.timeScale = 0;
  }

  private static void Pause()
  {
    FreezeTime();
    pauseMenu.SetActive(true);
    GameObject.Find("WwiseGlobal").GetComponent<AudioEvents>().PauseAudio();
  }

  private static void Resume()
  {
    Time.timeScale = 1;
    pauseMenu.SetActive(false);
    GameObject.Find("WwiseGlobal").GetComponent<AudioEvents>().ResumeAudio();
  }

  public void NextLevel()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    GameObject.Find("WwiseGlobal").GetComponent<AudioEvents>().EndAudio();
  }

  public void Quit()
  {
    SceneManager.LoadScene(0);
  }

  public void Restart()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    GameObject.Find("WwiseGlobal").GetComponent<AudioEvents>().EndAudio();
  }
}
