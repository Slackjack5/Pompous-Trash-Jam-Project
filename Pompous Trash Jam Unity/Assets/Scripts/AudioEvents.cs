using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEvents : MonoBehaviour
{
  public AK.Wwise.Event rhythmHeckinEvent;
  public UnityEvent OnLevelEnded;
  [HideInInspector] public static float secondsPerBeat;
  [HideInInspector] public static float BPM;
  public UnityEvent OnEveryGrid;
  public UnityEvent OnEveryBeat;
  public UnityEvent OnEveryBar;
  public UnityEvent OnR;
  public int GridCount = 0;
  public int gridCounter = 0;
  public bool startCounting = false;
  private bool playerReady = false;
  private bool musicReady = false;

  //id of the wwise event - using this to get the playback position
  static uint playingID;

  private void Start()
  {
    playingID = rhythmHeckinEvent.Post(gameObject, (uint)(AkCallbackType.AK_MusicSyncAll | AkCallbackType.AK_EnableGetMusicPlayPosition), MusicCallbackFunction);
  }
  public void EndAudio()
  {
    AkSoundEngine.PostEvent("Stop_VacuumLoop", gameObject);
    AkSoundEngine.PostEvent("Stop_Music", gameObject);
  }

  void MusicCallbackFunction(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
  {

    AkMusicSyncCallbackInfo _musicInfo = (AkMusicSyncCallbackInfo)in_info;

    switch (_musicInfo.musicSyncType)
    {
      case AkCallbackType.AK_MusicSyncUserCue:

        CustomCues(_musicInfo.userCueName, _musicInfo);

        secondsPerBeat = _musicInfo.segmentInfo_fBeatDuration;
        BPM = _musicInfo.segmentInfo_fBeatDuration * 60f;

        break;
      case AkCallbackType.AK_MusicSyncBeat:


        OnEveryBeat.Invoke();
        break;
      case AkCallbackType.AK_MusicSyncBar:
        //I want to make sure that the secondsPerBeat is defined on our first measure.
        secondsPerBeat = _musicInfo.segmentInfo_fBeatDuration;
        Debug.Log("Seconds Per Beat: " + secondsPerBeat);

        OnEveryBar.Invoke();
        break;

      case AkCallbackType.AK_MusicSyncGrid:
        OnEveryGrid.Invoke();
        break;
      default:
        break;

    }

  }


  public void CustomCues(string cueName, AkMusicSyncCallbackInfo _musicInfo)
  {
    switch (cueName)
    {
      case "R":
        if(playerReady==true)
        {
          AkSoundEngine.SetSwitch("GamePlay_Switch", "Active", gameObject);
          GameManager.StartLevel();
        }
        break;
      default:
        break;
    }
  }

  public void StartLevel()
  {
    playerReady = true;
  }
}
