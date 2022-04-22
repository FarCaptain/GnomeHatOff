using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreboardCanvasUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] float waitTimeForEachLetter;
    [SerializeField] Animator scoreboardCanvasAnimator;
    [SerializeField] GameObject lockInPlatforms;
    public Dictionary<int, int> playerScores;

    [SerializeField] GameObject hatPrefab;
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    [SerializeField] float zOffset;
    Vector3 spawnLocation;
    int zAdjustCouter = 0;
    int yAdjustCounter = 0;
    Color hatColor;
    bool scoreBoardSetup = false;

    [SerializeField] Transform[] hatPileSpawnLocation;
    [SerializeField] TextMeshProUGUI[] scoreTexts;
    int hatPileSpawnIndex;
    int hatPileCounter = 0;


    void OnEnable()
    {
        StartCoroutine(TypeWriteText(titleText.text));
    }

    void Update()
    {
        if(IsAnimationStateOver(scoreboardCanvasAnimator, "ScoreSpritesRise",1f) && !scoreBoardSetup)
		{
            StartCoroutine(SpawnHatPile(playerScores[0],0));
            scoreBoardSetup = true;
        }
    }

    private IEnumerator TypeWriteText(string textToFill)
    {
        titleText.text = "";
        foreach (char letter in textToFill)
        {
            titleText.text += letter;
            yield return new WaitForSecondsRealtime(waitTimeForEachLetter);
        }
        scoreboardCanvasAnimator.SetTrigger("scoreBoardUIActive");
        lockInPlatforms.SetActive(true);
    }
 
    private bool IsAnimationStateOver(Animator animator, string animationStateName, float time)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName)
               && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= time;
    }

    private IEnumerator SpawnHatPile(int numberOfHats, int gnomeIndex)
    {
        switch(gnomeIndex)
		{
            case 0:
                ColorUtility.TryParseHtmlString("#3768A7", out hatColor);
                hatPileSpawnIndex = 0;
                break;

            case 1:
                ColorUtility.TryParseHtmlString("#FFFFFF", out hatColor);
                hatPileSpawnIndex = 1;
                break;

            case 2:
                ColorUtility.TryParseHtmlString("#7637A7", out hatColor);
                hatPileSpawnIndex = 2;
                break;

            case 3:
                ColorUtility.TryParseHtmlString("#A7A037", out hatColor);
                hatPileSpawnIndex = 3;
                break;
        }

        spawnLocation = hatPileSpawnLocation[hatPileSpawnIndex].position;
        zAdjustCouter = 0;
        yAdjustCounter = 0;
        for (int i = 1; i < numberOfHats + 1; i++)
        {
            print(hatPileCounter);
            GameObject hat = Instantiate(hatPrefab, spawnLocation, Quaternion.identity);
            hat.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = hatColor;
            hat.transform.parent = hatPileSpawnLocation[hatPileSpawnIndex];
            if (i >= 3 && i % 3 == 0)
            {
                zAdjustCouter++;
                spawnLocation = new Vector3(hatPileSpawnLocation[hatPileSpawnIndex].position.x, hatPileSpawnLocation[hatPileSpawnIndex].position.y + (yOffset * yAdjustCounter), hatPileSpawnLocation[hatPileSpawnIndex].position.z - (zOffset * zAdjustCouter));
                if (i % 9 == 0 && i >= 9)
                {
                    zAdjustCouter = 0;
                    yAdjustCounter++;
                    spawnLocation = new Vector3(hatPileSpawnLocation[hatPileSpawnIndex].position.x, hatPileSpawnLocation[hatPileSpawnIndex].position.y + (yOffset * yAdjustCounter), hatPileSpawnLocation[hatPileSpawnIndex].position.z);
                }
            }
            else
            {
                spawnLocation = new Vector3(spawnLocation.x + xOffset, spawnLocation.y, spawnLocation.z);
            }
            scoreTexts[hatPileCounter].text = i.ToString();
            yield return new WaitForSecondsRealtime(0.1f);
        }
        hatPileCounter++;
        if (hatPileCounter < 4)
        {
            StartCoroutine(SpawnHatPile(playerScores[hatPileCounter], hatPileCounter));
        }
        else
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
