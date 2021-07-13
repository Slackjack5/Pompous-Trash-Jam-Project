using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TubeMinigame : ButtonMash
{
  [SerializeField] private float playInterval = 30f;
  [SerializeField] private GameObject player;

  public bool IsReady { get; private set; }

  public readonly UnityEvent triggered = new UnityEvent();

  protected override void Start()
  {
    base.Start();

    IsReady = false;
    StartCoroutine(Ready());
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (IsReady && collision.name == player.name)
    {
      IsReady = false;
      triggered.Invoke();
    }
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(10, 110, 400, 30), "tubeMinigame.isReady: " + IsReady);
  }

  private IEnumerator Ready()
  {
    yield return new WaitForSeconds(playInterval);
    IsReady = true;
  }

  public override void Restart()
  {
    base.Restart();

    StartCoroutine(Ready());
  }
}
