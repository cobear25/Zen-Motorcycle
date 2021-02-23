using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameController gameController;
    public SpriteRenderer spriteRenderer;
    public float scrollSpeed;
    public float endX;

    public bool shouldScroll = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldScroll) {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(endX, transform.position.y),
                scrollSpeed * Time.deltaTime);
            if (transform.position.x >= endX)
            {
                gameController.EndReached();
            }
        }
    }

    public void StartFadeIn() {
        StartCoroutine(FadeTo(1.0f, 8.0f));
    }

    public void StartFadeOut() {
        StartCoroutine(FadeTo(0.0f, 4.0f));
        Invoke("ResetPosition", 8.2f);
    }

    void ResetPosition() {
        transform.position = new Vector2(-6.5f, transform.position.y);
    }

    IEnumerator FadeTo(float newAlpha, float duration)
    {
        float alpha = spriteRenderer.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, newAlpha, t));
            spriteRenderer.color = newColor;
            yield return null;
        }
    }
}
