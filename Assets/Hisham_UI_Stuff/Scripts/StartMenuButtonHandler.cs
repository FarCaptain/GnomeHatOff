using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenuButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float buttonFinalXPos;
    [SerializeField] float bobOffset;
    [SerializeField] float bobSpeedMultiplier;
    [SerializeField] bool buttonSlide;

    private Vector2 buttonInitialPos;
    private bool buttonHoveredOver = false;
    private float currentBobTime;
    private void Start()
    {
        buttonInitialPos = GetComponent<RectTransform>().anchoredPosition;
    }
    private void Update()
    {
        if (buttonHoveredOver == true)
        {
            BobButton();
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(buttonSlide)
		{
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(buttonFinalXPos, buttonInitialPos.y);
        }
        buttonHoveredOver = true;
        currentBobTime = 0f;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        buttonHoveredOver = false;
        gameObject.GetComponent<RectTransform>().anchoredPosition = buttonInitialPos;

    }


	private void BobButton()
	{
        currentBobTime += Time.deltaTime*bobSpeedMultiplier;
        if ((int)(currentBobTime%2f)==0)
		{
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(buttonFinalXPos, (gameObject.GetComponent<RectTransform>().anchoredPosition.y+bobOffset));
		}
        else if ((int)(currentBobTime % 2f) == 1)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(buttonFinalXPos, (gameObject.GetComponent<RectTransform>().anchoredPosition.y - bobOffset));
        }
    }
}