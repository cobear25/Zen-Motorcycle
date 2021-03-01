using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeText : MonoBehaviour
{
    public bool shouldFadeOnStart = false;
    TextMeshProUGUI textMesh;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>(); 
        if (shouldFadeOnStart) {
            FastFadeOut();
        }
    }

    public void SetText(string text) {
        textMesh.SetText(text);
    }

    public void FadeIn() {
        textMesh.color = new Color(0, 0, 0, 0);
        if (textMesh.isActiveAndEnabled) {
            StartCoroutine(FadeTo(1.0f, 8.0f));
        }
    }

    public void FadeOut() {
        textMesh.color = new Color(0, 0, 0, 1);
        if (textMesh.isActiveAndEnabled)
        {
            StartCoroutine(FadeTo(0.0f, 8.0f));
        }
    }

    public void FastFadeOut() {
        textMesh.color = new Color(1, 1, 1, 1);
        if (textMesh.isActiveAndEnabled)
        {
            StartCoroutine(FadeAwayWhite(0.0f, 5.0f));
        }
    }

    IEnumerator FadeAwayWhite(float newAlpha, float duration)
    {
        float alpha = textMesh.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, newAlpha, t));
            textMesh.color = newColor;
            yield return null;
        }
    }

    IEnumerator FadeTo(float newAlpha, float duration)
    {
        float alpha = textMesh.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, newAlpha, t));
            textMesh.color = newColor;
            yield return null;
        }
    }
}
