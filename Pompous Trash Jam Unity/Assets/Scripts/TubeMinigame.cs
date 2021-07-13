using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TubeMinigame : ButtonMash
{
  [SerializeField] private float playInterval = 30f;
  [SerializeField] private GameObject player;

  private bool isReady = false;

  public readonly UnityEvent triggered = new UnityEvent();

  protected override void Start()
  {
    base.Start();

    StartCoroutine(Ready());
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (isReady && collision.name == player.name)
    {
      isReady = false;
      triggered.Invoke();
    }
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(10, 110, 400, 30), "tubeMinigame.isReady: " + isReady);
  }

  private IEnumerator Ready()
  {
    yield return new WaitForSeconds(playInterval);
    isReady = true;
  }

  public override void Restart()
  {
    base.Restart();

    StartCoroutine(Ready());
  }
}
