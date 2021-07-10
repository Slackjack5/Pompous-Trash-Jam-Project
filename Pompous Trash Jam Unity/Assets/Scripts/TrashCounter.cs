using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : MonoBehaviour
{
  [SerializeField] private int maxCount = 100;

  private int currentCount;

  // Start is called before the first frame update
  void Start()
  {
    currentCount = maxCount;
  }

  // Update is called once per frame
  void Update()
  {
    if (currentCount <= 0)
    {
      // Player wins when there are zero trash left
      print("You win!");
    }
  }

  public void Decrease(int value)
  {
    currentCount -= value;
  }
}
