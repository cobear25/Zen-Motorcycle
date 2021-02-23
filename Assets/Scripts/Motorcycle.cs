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
    public ParticleSystem trackParticleSystem;
    public ParticleSystem smokeParticleSystem;

    // helps determine how much the motorcycle will scale up and down as it moves along the y-axis
    public float scaleRangeMultiplier;
    // the min scale for the motorcycle
    public float bottomScaleBuffer;

    SpriteRenderer spriteRenderer;
    public SpriteRenderer manSpriteRenderer;

    float yRange;
    bool canMove = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        yRange = Mathf.Abs(maxY - minY);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!canMove) return;
        // vertical movement
        var vertical = Input.GetAxis("Vertical"); 
        if (vertical > 0) {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x, maxY),
                vSpeed * vertical * Time.deltaTime); 
        } else if (vertical < 0) {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x, minY),
                vSpeed * -vertical * Time.deltaTime); 
        }

        // horizontal movement
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

        var emission = trackParticleSystem.emission;
        emission.rateOverTime = 20 * (-horizontal + 1.1f);

        // handle rotation
        float h = horizontal;
        // ensure that there's always rotation even if horizontal is zero
        if (h == 0) h = -1f;
        if (vertical > 0 && horizontal <= 0) {
            // front up
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -8 * vertical * -h));
        } else if (vertical > 0 && horizontal > 0) {
            // front down
           transform.rotation = Quaternion.Euler(new Vector3(0, 0, 8 * vertical * h));
        } else if (vertical < 0 && horizontal <= 0) {
            // front down
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -8 * vertical * -h));
        } else if (vertical < 0 && horizontal > 0) {
            // front up
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 8 * vertical * h));
        } else {
            // no vertical movement, stay straight.
            transform.rotation = Quaternion.identity;
        }

        if (transform.position.y > maxY) 
        {
            transform.position = new Vector2(transform.position.x, maxY - 0.01f);
        }
        if (transform.position.y < minY) 
        {
            transform.position = new Vector2(transform.position.x, minY + 0.01f);
        }

        float distFromTop = Mathf.Abs(maxY - transform.position.y);
        float scale = (distFromTop / yRange * scaleRangeMultiplier) + bottomScaleBuffer;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void LateUpdate() {
        spriteRenderer.sortingOrder = (int)-transform.position.y;
        manSpriteRenderer.sortingOrder = (int)-transform.position.y;
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

    public void BrokenDown() {
        gameController.ObstacleHit();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        gameController.ObstacleHit();
    }

    void OnTriggerEnter2D(Collider2D col) {
        col.GetComponent<SpriteRenderer>().enabled = false;
        col.enabled = false;
        col.gameObject.GetComponent<ParticleSystem>().Stop();
        var pickup = col.GetComponent<Pickup>();
        pickup.PickedUp();
        gameController.PickupPickedUp(pickup);
    }

    public void StopMoving() {
        canMove = false;
        trackParticleSystem.Stop();
        var shape = smokeParticleSystem.shape;
        shape.rotation = new Vector3(0, 0, -transform.rotation.eulerAngles.z);
        smokeParticleSystem.Play();
    }

    public void Repare() {
        canMove = true;
        trackParticleSystem.Play();
        smokeParticleSystem.Stop();
    }

    public void EnableMovement() {
        canMove = true;
    }
}
