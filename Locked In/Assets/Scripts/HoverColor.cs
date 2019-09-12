using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  public void OnPointerEnter(PointerEventData eventData) {
     GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.2878766f, 0, 1.0f);
  }

  public void OnPointerExit(PointerEventData eventData) {
     GetComponent<TextMeshProUGUI>().color = Color.white;
  }
}