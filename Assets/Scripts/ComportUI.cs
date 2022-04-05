using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComportUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject Canvas;
    public MainGameController MG;
    public TMP_InputField comport1;
    public TMP_InputField comport2;
    public TMP_InputField comport3;
    public void setcomports()
    {
        //MG.COM = new string[3];
        MG.COM.Clear();
        MG.COM.Add(comport1.text);
        MG.COM.Add(comport2.text);
        MG.COM.Add(comport3.text);

        Canvas.SetActive(false);
        MG.gameObject.SetActive(true);
        FindObjectOfType<Timer>().timerPaused = false;
    }
}
