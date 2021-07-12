using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  [SerializeField] private InputActionAsset actions;
  [SerializeField] private ButtonMash buttonMashMinigame;
  [SerializeField] private Accuracy accuracyMinigame;
  [SerializeField] private AlternatingButtonMash alternatingButtonMashMinigame;
  [SerializeField] private Sequence sequenceMinigame;

  private static InputActionMap playerActions;
  private static InputActionMap minigameActions;

  public static bool IsGameActive { get; private set; }

  public static bool IsMinigameActive {
    get { return CurrentMinigame != null; }
  }

  public static Minigame CurrentMinigame { get; private set; }

  private static Vector2 defaultGravity = new Vector2(0, -9.8f);

  private void Start()
  {
    DeactivateGame();

    playerActions = actions.FindActionMap("Player");
    minigameActions = actions.FindActionMap("Minigame");

    playerActions.Enable();
    minigameActions.Disable();
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

    playerActions.Enable();
    minigameActions.Disable();

    CurrentMinigame = null;
  }

  public static void StartMinigame(Minigame minigame)
  {
    if (!IsMinigameActive)
    {
      DeactivateGame();

      minigameActions.Enable();
      playerActions.Disable();

      minigame.complete.AddListener(() => EndMinigame());
      minigame.Restart();
      CurrentMinigame = minigame;
    }
  }

  public void Restart()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
