using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBlurManager : MonoBehaviour
{
    public static ScreenBlurManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    static ScreenBlurManager instance = null;

    [Header("Required References")]
    public Material screenBlurMaterial = null;
    public SpriteRenderer spriteRenderer;

    Material runtimeBlurMaterial = null;

    public float BlurAmount
    {
        get
        {
            return blurAmount;
        }
        set
        {
            blurAmount = value;
            runtimeBlurMaterial.SetFloat(matBlurParameter, value);
        }
    }
    private float blurAmount = 0;

    const string matBlurParameter = "BlurAmount";

    // Start is called before the first frame update
    void Start()
    {
        runtimeBlurMaterial = new Material(screenBlurMaterial);
        spriteRenderer.material = runtimeBlurMaterial;
    }

    public IEnumerator LerpBlur(float duration, float targetValue)
    {
        float timer = 0;
        float startingValue = blurAmount;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float lerpValue = Mathf.Lerp(startingValue, targetValue, timer / duration);
            BlurAmount = lerpValue;
            yield return null;
        }
        BlurAmount = targetValue;

        yield break;
    }
}
