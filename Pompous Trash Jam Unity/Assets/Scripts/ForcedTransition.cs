using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForcedTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public void Transition()
  {
    if(GameManager.PressedButton==1)
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    else if(GameManager.PressedButton == 2)
    {
      SceneManager.LoadScene(0);
    }
    else if(GameManager.PressedButton == 3)
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

  }
}
