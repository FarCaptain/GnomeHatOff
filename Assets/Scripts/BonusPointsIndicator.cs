using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonusPointsIndicator : MonoBehaviour
{
	[Header("Bonus Points Indicator Movement Parameters")]
	[SerializeField] float lifeTime = 0.6f;
	[SerializeField] float distance = 2f;
	[SerializeField] public Vector3 initialPos;

	[Header("Bonus Points Indicator Color Options")]
	[SerializeField] Color player1BonusColor;
	[SerializeField] Color player2BonusColor;

	[Header("Bonus Points Indicator Cached Components")]
	[SerializeField] Transform desiredRotationsTransform;
 	[SerializeField] TextMeshProUGUI bonusPoints;

	[HideInInspector]
	public float startTime;

	private Vector3 targetPos;
    private float halfOfLifeTime;
    private NewTimer popupTimer;
  
    void Start()
	{
		startTime = Time.time;
		halfOfLifeTime = lifeTime / 2f;
		InstantiatePopupTimer();
		SetupRandomTargotPos();
	}

	private void SetupRandomTargotPos()
	{
		transform.rotation=desiredRotationsTransform.rotation;
		targetPos = initialPos +  new Vector3(0f, distance, 0f);
		targetPos = new Vector3(targetPos.x, Mathf.Abs(targetPos.y), targetPos.z);
		transform.localScale = Vector3.zero;
	}

	private void InstantiatePopupTimer()
	{
		popupTimer = gameObject.AddComponent<NewTimer>();
		popupTimer.MaxTime = lifeTime;
		popupTimer.TimerStart = true;
	}

	// Update is called once per frame
	void Update()
	{
		FadeOutAndDestroy();

		transform.localPosition = Vector3.Lerp(initialPos, targetPos, popupTimer.TimerCompletionRate);
		transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, popupTimer.TimerCompletionRate);
	}

	private void FadeOutAndDestroy()
	{
		if (popupTimer.TimerStart == false)
		{
			Destroy(gameObject);
		}
		else if (popupTimer.CurrentTime > halfOfLifeTime)
		{
			bonusPoints.color = Color.Lerp(bonusPoints.color, Color.clear, (popupTimer.CurrentTime - halfOfLifeTime) / halfOfLifeTime);
		}
	}

	public void SetPointsText(int points, int playerIndex)
	{
        bonusPoints.text = "+" + points.ToString() +" BONUS";
		if(playerIndex==0)
		{
			bonusPoints.color = player1BonusColor;
		}
		else if(playerIndex==1)
		{
			bonusPoints.color = player2BonusColor;
		}
	}
}
