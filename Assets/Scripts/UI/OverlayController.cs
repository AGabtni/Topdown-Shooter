using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayController : MonoBehaviour
{
    [SerializeField] private Image overlayImage;

    void Start()
    {
        overlayImage.color = new Color(0, 0, 0, 1);
        overlayImage.fillAmount = 1;
        SetImageSetting();
    }

    public void SetImageSetting(Image.FillMethod fillMethod = Image.FillMethod.Horizontal, int fillOrigin = (int)Image.OriginHorizontal.Left)
    {

        overlayImage.fillMethod = fillMethod;
        overlayImage.fillOrigin = fillOrigin;


    }
    public IEnumerator FadeColor(float duration, Color targetColor)
    {
        Color currentColor = overlayImage.color;
        
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            overlayImage.color = Color.Lerp(currentColor, targetColor, normalizedTime);
            yield return null;
        }
        overlayImage.color = targetColor;

    }

    public IEnumerator FadeValue(float duration, float targetValue)
    {

        float currentFill = overlayImage.fillAmount;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            overlayImage.fillAmount = Mathf.Lerp(currentFill, targetValue, normalizedTime);
            yield return null;
        }
        overlayImage.fillAmount = targetValue;


    }
}
