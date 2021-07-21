using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
  public Animator transition;

  public void Play()
  {
    StartCoroutine(LoadLevel(1));
  }

  IEnumerator LoadLevel(int levelIndex)
  {
    transition.SetTrigger("Start");

    yield return new WaitForSeconds(1);

    SceneManager.LoadScene(levelIndex);
  }

  public void Quit()
  {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }
}
