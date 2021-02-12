using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool shouldScroll = false;
    public float scrollSpeed;
    public Sprite[] vehicleSprites;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = vehicleSprites[Random.Range(0, vehicleSprites.Length - 1)];
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldScroll) {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(12, transform.position.y),
                scrollSpeed * Time.deltaTime);
        }
        float distFromTop = Mathf.Abs(1 - transform.position.y);
        float scale = (distFromTop / 4.5f * 0.15f) + 0.7f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
