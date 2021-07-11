using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Minigame : MonoBehaviour
{
  public readonly UnityEvent complete = new UnityEvent();

  public virtual void OnFire() { }

  public virtual void OnUp() { }

  public virtual void OnDown() { }

  public virtual void OnLeft() { }

  public virtual void OnRight() { }

  public virtual void Restart() { }
}
