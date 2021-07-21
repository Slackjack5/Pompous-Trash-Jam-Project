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
  [SerializeField] private bool isTutorial = false;

  public static readonly UnityEvent levelComplete = new UnityEvent();

  public static bool IsLevelStarted { get; private set; }

  public static bool IsGameActive { get; private set; }

  public static bool IsMinigameActive {
    get { return CurrentMinigame != null; }
  }

  public static Minigame CurrentMinigame { get; private set; }

  public static TubeMinigame TubeMinigame { get; private set; }

  public static bool IsTutorial { get; private set; }

  public static bool IsTimerPaused;

  private static Vector2 defaultGravity = new Vector2(0, -9.8f);
  private static bool isGameStarted = false;
  private static bool isLevelComplete = false;
  private static bool isPaused = false;
  private static Minigame[] minigames;
  private static GameObject pauseMenu;
  
  private float timeFrozen = 0;

  private void Awake()
  {
    IsLevelStarted = false;

    DeactivateGame();

    minigames = new Minigame[] { buttonMashMinigame, accuracyMinigame, alternatingButtonMashMinigame, sequenceMinigame };
    TubeMinigame = tubeMinigame;
    TubeMinigame.triggered.AddListener(() => StartMinigame(TubeMinigame));

    isPaused = false;
    pauseMenu = pauseMenuCanvas;
    pauseMenu.SetActive(false);

    isLevelComplete = false;

    IsTutorial = isTutorial;
  }

  private void Update()
  {
    if (isGameStarted && !IsGameActive)
    {
      timeFrozen += Time.deltaTime;
      if (timeFrozen >= 1f)
      {
        print("[GameManager] Fail-safe initialized: activating game");
        ActivateGame();
      }
    }
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

  // Called when countdown is finished
  public static void StartGame()
  {
    ActivateGame();
    isGameStarted = true;
  }

  // Called when Space is pressed at the beginning
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
    // GameObject.Find("WwiseGlobal").GetComponent<AudioEvents>().FadeAudio();
  }

  public static void EndMinigame()
  {
    if (IsTutorial)
    {
      EndLevel();
    }
    else
    {
      ActivateGame();

      CurrentMinigame = null;
    }
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
    GameObject.Find("WwiseGlobal").GetComponent<AudioEvents>().EndAudio();
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  public void Quit()
  {
    GameObject.Find("WwiseGlobal").GetComponent<AudioEvents>().EndAudio();
    SceneManager.LoadScene(0);
  }

  public void Restart()
  {
    GameObject.Find("WwiseGlobal").GetComponent<AudioEvents>().EndAudio();
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
