using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameController gameController;
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
}
