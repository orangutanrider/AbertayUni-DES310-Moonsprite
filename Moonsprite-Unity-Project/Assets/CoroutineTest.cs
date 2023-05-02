using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    void Start()
    {
        //Start the coroutine we define below named ExampleCoroutine.
        StartCoroutine(ExampleCoroutine( 2, 5));
    }

    IEnumerator ExampleCoroutine(int x, int y)
    {

        string[] test = Regex.Split("AEIOU", string.Empty);

        foreach (string s in test)
        {
            Debug.LogWarning("current Sound: " + s);
        }

        string testingString = "this drill";
        int letter = 3;
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);

        Debug.Log("Numbers are : " + x + " and " + y);

        Debug.Log("letter number " + letter + " is " + testingString[letter]);

        //FindObjectOfType<AudioManager>().Play("123");
        FindObjectOfType<AudioManagerManager>().PlaySpecificSound("clack", "123");

        yield return new WaitForSeconds(2);

        //FindObjectOfType<AudioManager>().Play("123");
        FindObjectOfType<AudioManagerManager>().PlaySpecificSound("click", "5");

        yield return new WaitForSeconds(2);

        FindObjectOfType<AudioManagerManager>().playDialogue("AN SSA  NSAASS", 0.1f, 0.01f, -0.5f);
    }
}
