using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Minigame : MonoBehaviour
{
  public readonly UnityEvent complete = new UnityEvent();

  public abstract void OnFire();

  public abstract void Restart();
}
