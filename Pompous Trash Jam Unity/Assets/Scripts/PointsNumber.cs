using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsNumber : MonoBehaviour
{
  [SerializeField] private float maxStayTime = 0.3f;
  [SerializeField] private float maxFadeTime = 1f;

  private RectTransform rectTransform;
  private TextMeshProUGUI text;

  private float currentFadeTime;
  private float currentStayTime;

  private void Start()
  {
    rectTransform = GetComponent<RectTransform>();
    text = GetComponent<TextMeshProUGUI>();

    currentFadeTime = maxFadeTime;
    currentStayTime = maxStayTime;
  }

  private void Update()
  {
    if (currentStayTime > 0)
    {
      currentStayTime -= Time.deltaTime;
    }
    else
    {
      if (currentFadeTime > 0)
      {
        float t = Mathf.InverseLerp(0, maxFadeTime, currentFadeTime);
        Color color = text.color;
        color.a = Mathf.Lerp(0, 1, t);
        text.color = color;

        currentFadeTime -= Time.deltaTime;
      }
      else
      {
        Destroy(gameObject);
      }
    }
  }

  private void FixedUpdate()
  {
    Vector2 anchoredPosition = rectTransform.anchoredPosition;
    anchoredPosition.y++;
    rectTransform.anchoredPosition = anchoredPosition;
  }
}
