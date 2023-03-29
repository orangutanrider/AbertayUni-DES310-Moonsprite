using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustCloudScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fadeTime;
    float fadeTimer;

    // Start is called before the first frame update
    void Start()
    {
        fadeTimer = fadeTime;
    }

    void Update()
    {
        fadeTimer = fadeTimer - Time.deltaTime;
        float newAlpha = Mathf.Lerp(0, 1, fadeTimer / fadeTime);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);

        if(fadeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
