using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HatDropIndicator : MonoBehaviour
{
    [SerializeField] float lifeTime = 0.6f;
    [SerializeField] float minDist = 2f;
    [SerializeField] float maxDist = 3f;
    [SerializeField] TextMeshProUGUI pointsText;

    private Vector3 initialPos;
    private Vector3 targetPos;
    private float halfOfLifeTime;
    private NewTimer popupTimer;
  
    void Start()
	{
		initialPos = transform.position;
		halfOfLifeTime = lifeTime / 2f;
		InstantiatePopupTimer();
		SetupRandomTargotPos();
	}

	private void SetupRandomTargotPos()
	{
		float direction = Random.rotation.eulerAngles.z;
		float distance = Random.Range(minDist, maxDist);
		transform.LookAt(2 * transform.position - Camera.main.transform.position);
		targetPos = initialPos + (Quaternion.Euler(0, 0, direction) * new Vector3(distance, distance, 0f));
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
			pointsText.color = Color.Lerp(pointsText.color, Color.clear, (popupTimer.CurrentTime - halfOfLifeTime) / halfOfLifeTime);
		}
	}

	public void SetPoints(int points)
	{
        pointsText.text = points.ToString();
	}
}
