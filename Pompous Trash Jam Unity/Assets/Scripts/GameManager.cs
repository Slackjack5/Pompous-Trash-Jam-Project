using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  [SerializeField] private ButtonMash buttonMashMinigame;

  public static bool IsGameActive { get; set; }

  public static bool IsMinigameActive {
    get { return CurrentMinigame != null; }
  }

  public static Minigame CurrentMinigame { get; private set; }

  const float defaultGravity = -9.8f;

  private void Start()
  {
    IsGameActive = false;
  }

  private void OnGUI()
  {
    if (GUI.Button(new Rect(25, 25, 120, 40), "Start Button Mash"))
    {
      StartMinigame(buttonMashMinigame);
    }
  }

  public static void FreezeGame()
  {
    Time.timeScale = 0;
  }

  public static void EndMinigame()
  {
    IsGameActive = true;
    Physics2D.gravity = new Vector2(0, defaultGravity);

    CurrentMinigame = null;
  }

  public static void StartMinigame(Minigame minigame)
  {
    IsGameActive = false;
    Physics2D.gravity = Vector2.zero;

    minigame.complete.AddListener(() => EndMinigame());
    minigame.Restart();
    CurrentMinigame = minigame;
  }

  public void Restart()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
