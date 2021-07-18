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
  private static GameObject pauseMenu;

  private void Start()
  {
    IsLevelStarted = false;

    DeactivateGame();

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

  public static void StartLevel()
  {
    IsLevelStarted = true;
  }

  public static void EndLevel()
  {
    DeactivateGame();
    levelComplete.Invoke();
    isLevelComplete = true;
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
  }

  private static void Resume()
  {
    Time.timeScale = 1;
    pauseMenu.SetActive(false);
  }

  public void NextLevel()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  public void Quit()
  {
    SceneManager.LoadScene(0);
  }

  public void Restart()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
