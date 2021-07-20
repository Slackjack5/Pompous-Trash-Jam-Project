using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TubeMinigame : ButtonMash
{
  [SerializeField] private float playInterval = 30f;

  public bool IsReady { get; private set; }

  public readonly UnityEvent triggered = new UnityEvent();

  //UI
  [SerializeField] public GameObject FixMe;

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
      AkSoundEngine.PostEvent("Stop_VacuumBroken", gameObject);
      FixMe.SetActive(false);
    }
  }

  private IEnumerator Ready()
  {
    yield return new WaitForSeconds(playInterval);
    IsReady = true;
    AkSoundEngine.PostEvent("Play_VacuumBroken", gameObject);
    FixMe.SetActive(true);
  }

  public override void Restart()
  {
    base.Restart();

    StartCoroutine(Ready());
  }
}
