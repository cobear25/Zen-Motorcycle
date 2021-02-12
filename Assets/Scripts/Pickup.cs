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
    }
}
