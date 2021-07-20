using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelComplete : MonoBehaviour
{
  [SerializeField] private float showDelay = 3f;
  [SerializeField] private TextMeshProUGUI countdownText;
  [SerializeField] private string endLevelText = "FINISHED";

  private Canvas canvas;

  private void Start()
  {
    canvas = GetComponent<Canvas>();
    canvas.enabled = false;

    GameManager.levelComplete.AddListener(() =>
    {
      StartCoroutine(Show());
    });
  }

  private IEnumerator Show()
  {
    countdownText.enabled = true;

    countdownText.text = endLevelText;

    yield return new WaitForSeconds(showDelay);

    countdownText.enabled = false;

    canvas.enabled = true;
    GameManager.FreezeTime();
  }
}
