using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("UI")]
    public RectTransform repairMeter;
    public float maxMeterWidth;

    [Header("Gameplay")]
    public GameObject obstaclePrefab;
    public GameObject pickupPrefab;
    public Background background;
    public float obstacleStartX;
    public float obstacleMaxY;
    public float obstacleMinY;
    public float obstacleSpawnRate;
    private bool shouldScroll;
    public bool ShouldScroll {
        set {
            shouldScroll = value;
            background.shouldScroll = value;
            foreach (var item in obstacles)
            {
                item.GetComponent<Obstacle>().shouldScroll = value; 
            }
            foreach (var item in pickups)
            {
                item.GetComponent<Pickup>().shouldScroll = value; 
            }
        }
        get {
            return shouldScroll;
        }
    }

    List<Obstacle> obstacles = new List<Obstacle>();
    List<Pickup> pickups = new List<Pickup>();
    // Start is called before the first frame update
    void Start()
    {
        ShouldScroll = true;
        StartSpawningObstacles();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldScroll && repairMeter.sizeDelta.x > 0) {
            repairMeter.sizeDelta = new Vector2(repairMeter.sizeDelta.x - 0.9f, repairMeter.sizeDelta.y);
            if (repairMeter.sizeDelta.x <= 0) {
                ShouldScroll = false;
            }
        }
    }

    public void StartSpawningObstacles() {
        SpawnObstacle();
    }

    private bool shouldSwitch = true;
    void SpawnObstacle() {
        if (shouldScroll)
        {
            if (shouldSwitch) {
                Obstacle obstacle = Instantiate(obstaclePrefab).GetComponent<Obstacle>();
                obstacle.transform.position = new Vector2(obstacleStartX, Random.Range(obstacleMinY, obstacleMaxY));
                obstacle.shouldScroll = true;
                obstacle.scrollSpeed = Random.Range(2.0f, 3.0f);
                obstacles.Add(obstacle);
                Invoke("SpawnObstacle", obstacleSpawnRate / 2);
            } else {
                Pickup pickup = Instantiate(pickupPrefab).GetComponent<Pickup>();
                pickup.transform.position = new Vector2(obstacleStartX, Random.Range(obstacleMinY, obstacleMaxY));
                pickup.shouldScroll = true;
                pickup.scrollSpeed = 5;
                pickups.Add(pickup);
                Invoke("SpawnObstacle", obstacleSpawnRate);
            }
            shouldSwitch = !shouldSwitch;
        }
    }

    public void ObstacleHit() {

    }

    public void WrenchPickedUp() {
        repairMeter.sizeDelta = new Vector2(maxMeterWidth, repairMeter.sizeDelta.y);
    }

    public void EndReached() {
        ShouldScroll = false;
    }
}
