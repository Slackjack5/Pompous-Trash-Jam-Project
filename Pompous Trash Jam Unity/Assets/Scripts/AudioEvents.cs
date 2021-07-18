using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvents : MonoBehaviour
{
  public void EndAudio()
  {
    AkSoundEngine.PostEvent("Stop_VacuumLoop", gameObject);
    AkSoundEngine.PostEvent("Stop_Music", gameObject);
  }
}
