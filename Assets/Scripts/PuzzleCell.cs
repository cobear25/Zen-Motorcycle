using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleCell : MonoBehaviour
{
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>(); 
    }

    public void MarkInPlace() {
        image.color = Color.green;
        ResetColor();
    }

    void ResetColor() {
        StartCoroutine(FadeToWhite(1));
    }

    IEnumerator FadeToWhite(float duration)
    {
        float r = image.color.r;
        float g = image.color.g;
        float b = image.color.b;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            Color newColor = new Color(Mathf.Lerp(r, 1, t), Mathf.Lerp(g, 1, t), Mathf.Lerp(b, 1, t), 1);
            image.color = newColor;
            yield return null;
        }
    }
}
