using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowCursorOnEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	[SerializeField] private UIController uiController;

	public void OnPointerEnter (PointerEventData eventData) {
		uiController.ShowCursor();
	}

	public void OnPointerExit (PointerEventData eventData) {
		uiController.HideCursor();
	}
}