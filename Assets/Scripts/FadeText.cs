using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeText : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>(); 
    }

    public void FadeIn() {
        StartCoroutine(FadeTo(1.0f, 8.0f));
    }

    public void FadeOut() {
        StartCoroutine(FadeTo(0.0f, 8.0f));
    }

    IEnumerator FadeTo(float newAlpha, float duration)
    {
        float alpha = text.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, newAlpha, t));
            text.color = newColor;
            yield return null;
        }
    }
}
