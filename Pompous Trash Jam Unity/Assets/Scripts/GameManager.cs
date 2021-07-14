using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  [SerializeField] private ButtonMash buttonMashMinigame;
  [SerializeField] private Accuracy accuracyMinigame;
  [SerializeField] private AlternatingButtonMash alternatingButtonMashMinigame;
  [SerializeField] private Sequence sequenceMinigame;
  [SerializeField] private TubeMinigame tubeMinigame;
  [SerializeField] private GameObject pauseMenuCanvas;

  public static bool IsGameActive { get; private set; }

  public static bool IsMinigameActive {
    get { return CurrentMinigame != null; }
  }

  public static Minigame CurrentMinigame { get; private set; }

  private static Vector2 defaultGravity = new Vector2(0, -9.8f);
  private static bool isPaused = false;
  private static GameObject pauseMenu;

  private void Start()
  {
    DeactivateGame();

    tubeMinigame.triggered.AddListener(() => StartMinigame(tubeMinigame));

    isPaused = false;
    pauseMenu = pauseMenuCanvas;
    pauseMenu.SetActive(false);
  }

  private void OnGUI()
  {
    if (GUI.Button(new Rect(25, 25, 120, 40), "Start Button Mash"))
    {
      StartMinigame(buttonMashMinigame);
    }

    if (GUI.Button(new Rect(145, 25, 120, 40), "Start Accuracy"))
    {
      StartMinigame(accuracyMinigame);
    }

    if (GUI.Button(new Rect(265, 25, 120, 40), "Start Alternating Button Mash"))
    {
      StartMinigame(alternatingButtonMashMinigame);
    }

    if (GUI.Button(new Rect(385, 25, 120, 40), "Start Sequence"))
    {
      StartMinigame(sequenceMinigame);
    }

    GUI.Label(new Rect(10, 70, 400, 40), "IsGameActive: " + IsGameActive);
    GUI.Label(new Rect(10, 90, 400, 40), "IsMinigameActive: " + IsMinigameActive);
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

  public static void FreezeGame()
  {
    Time.timeScale = 0;
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

  private static void Pause()
  {
    FreezeGame();
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
