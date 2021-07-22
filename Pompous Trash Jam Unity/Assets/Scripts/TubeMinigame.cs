using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TubeMinigame : ButtonMash
{
  [SerializeField] private float playInterval = 30f;
  [SerializeField] private bool isTutorialStage = false;

  public bool IsReady { get; private set; }

  public readonly UnityEvent triggered = new UnityEvent();

  //UI
  [SerializeField] public GameObject FixMe;

  private bool hasStartedTicking = false;

  protected override void Start()
  {
    base.Start();

    hasStartedTicking = false;

    if (GameManager.IsTutorial)
    {
      if (isTutorialStage)
      {
        Activate();
      }
    }
    else
    {
      IsReady = false;
    }
  }

  protected override void DoUpdate()
  {
    base.DoUpdate();

    if (GameManager.IsGameActive && !hasStartedTicking)
    {
      hasStartedTicking = true;
      StartCoroutine(Ready());
    }
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
    if (!IsReady)
    {
      Activate();
    }
  }

  private void Activate()
  {
    IsReady = true;
    AkSoundEngine.PostEvent("Play_VacuumBroken", gameObject);
    FixMe.SetActive(true);
  }

  public override void Restart()
  {
    base.Restart();

    if (!isTutorialStage)
    {
      StartCoroutine(Ready());
    }
  }
}
