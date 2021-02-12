using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool shouldScroll = false;
    public float scrollSpeed;
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
                new Vector2(12, transform.position.y),
                scrollSpeed * Time.deltaTime);
        }
        float distFromTop = Mathf.Abs(1 - transform.position.y);
        float scale = (distFromTop / 4.5f * 0.15f) + 0.15f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
