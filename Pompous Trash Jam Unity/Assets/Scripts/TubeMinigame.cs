using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TubeMinigame : ButtonMash
{
  [SerializeField] private float playInterval = 30f;

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
    if (IsReady && collision.gameObject.GetComponent<PlayerController>())
    {
      IsReady = false;
      triggered.Invoke();
    }
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
