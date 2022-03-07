using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenuButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] float buttonInitialXPos;
    [SerializeField] float buttonFinalXPos;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(buttonFinalXPos, gameObject.GetComponent<RectTransform>().anchoredPosition.y);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(buttonInitialXPos, gameObject.GetComponent<RectTransform>().anchoredPosition.y);
    }
}