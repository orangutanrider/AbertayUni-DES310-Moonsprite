using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpeedCheatScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Comma) == true)
        {
            Time.timeScale = Time.timeScale - 0.1f;
            Debug.Log("New timescale = " + Time.timeScale);
        }
        if(Input.GetKeyDown(KeyCode.Period) == true)
        {
            Time.timeScale = Time.timeScale + 0.1f;
            Debug.Log("New timescale = " + Time.timeScale);
        }
    }
}
