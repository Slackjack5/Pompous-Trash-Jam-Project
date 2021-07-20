using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
  public static PowerupManager Instance { get; private set; }

  private static PlayerController player;
  private static Powerup currentPowerup;

  private static float timeLeft;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
    }
    else
    {
      Instance = this;
    }
  }

  private void Start()
  {
    player = GetComponent<PlayerController>();
  }

  private void Update()
  {
    if (currentPowerup != null)
    {
      if (timeLeft > 0)
      {
        timeLeft -= Time.deltaTime;
      }
      else
      {
        currentPowerup = null;
      }
    }
  }

  public void Activate(Powerup powerup)
  {
    if (currentPowerup != null)
    {
      currentPowerup = null;
    }

    currentPowerup = powerup;
    timeLeft = powerup.Duration;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (currentPowerup != null)
    {
      if (currentPowerup.Type == Powerup.PowerupType.SUPER_STRENGTH)
      {
        BoxDestruction box = collision.gameObject.GetComponent<BoxDestruction>();
        if (box)
        {
          box.Hit(player.IsFacingRight, (currentPowerup as SuperStrength).HitForce);
        }
      }
    }
  }
}
