using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motorcycle : MonoBehaviour
{
    public float hSpeed;
    public float vSpeed;
    public float maxY;
    public float minY;
    public float maxX;
    public float minX;
    public GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        // MoveWithMouse();
        var vertical = Input.GetAxis("Vertical"); 
        if (vertical > 0) {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x, maxY),
                vSpeed * vertical * Time.deltaTime); 
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -8 * vertical));
        } else if (vertical < 0) {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x, minY),
                vSpeed * -vertical * Time.deltaTime); 
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 8 * -vertical));
        } else {
            transform.rotation = Quaternion.identity;
        }
        var horizontal = Input.GetAxis("Horizontal"); 
        if (horizontal > 0) {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(maxX, transform.position.y),
                hSpeed * horizontal * Time.deltaTime); 
        } else if (horizontal < 0) {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(minX, transform.position.y),
                hSpeed * -horizontal * Time.deltaTime); 
        }

        if (transform.position.y > maxY) 
        {
            transform.position = new Vector2(transform.position.x, maxY - 0.01f);
        }
        if (transform.position.y < minY) 
        {
            transform.position = new Vector2(transform.position.x, minY + 0.01f);
        }
    }

    void MoveWithMouse() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (transform.position.y < maxY && transform.position.y > minY)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x, mousePos.y),
                vSpeed * Time.deltaTime);
        }
        if (transform.position.y > maxY) 
        {
            transform.position = new Vector2(transform.position.x, maxY - 0.01f);
        }
        if (transform.position.y < minY) 
        {
            transform.position = new Vector2(transform.position.x, minY + 0.01f);
        }
        
        float yDist = Mathf.Abs(transform.position.y - mousePos.y);
        if (mousePos.y < transform.position.y) {
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 8)), 60 * yDist * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 8));
        }
        if (mousePos.y > transform.position.y) {
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, -8)), 60 * yDist * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -8));
        }
        if (yDist < 1) {
            //     transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), 20 * yDist * Time.deltaTime);
            transform.rotation = Quaternion.identity;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");    
    }

    void OnTriggerEnter2D(Collider2D col) {
        col.GetComponent<SpriteRenderer>().enabled = false;
        col.enabled = false;
        col.gameObject.GetComponent<ParticleSystem>().Stop();
        gameController.WrenchPickedUp();
    }
}
