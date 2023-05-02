using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrossfadeTestScript : MonoBehaviour
{
    public TMP_Text minuteText;
    public TMP_Text secodndsText;

    float timer = 0;

    // Update is called once per frame
    void Update()
    {
        timer = timer + Time.deltaTime;

        int minuteNumber = Mathf.FloorToInt(timer / 60f);
        minuteText.text = minuteNumber.ToString();
        int secondsNumber = Mathf.FloorToInt(timer - (minuteNumber * 60));

        secodndsText.text = secondsNumber.ToString();
    }
}
