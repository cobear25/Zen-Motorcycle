using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("UI")]
    public RectTransform canvasRectTransform;
    public RectTransform repairMeter;
    public float maxMeterWidth;
    public GridLayoutGroup puzzleGrid;
    public GameObject puzzlePiecePrefab;

    public GameObject repairIcon;
    public GameObject repairMeterWrapper;

    [Header("Gameplay")]
    public Motorcycle motorcycle;
    public GameObject obstaclePrefab;
    public GameObject pickupPrefab;
    public Background background;
    public float obstacleStartX;
    public float obstacleMaxY;
    public float obstacleMinY;
    public float obstacleSpawnRate;
    private bool shouldScroll;

    int puzzleRows = 3;
    int puzzleCols = 4;

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
            if (!value) motorcycle.StopMoving();
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
                motorcycle.BrokenDown();
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
        ShouldScroll = false;
        Invoke("GetScreenshot", 1);
    }

    public void WrenchPickedUp() {
        repairMeter.sizeDelta = new Vector2(maxMeterWidth, repairMeter.sizeDelta.y);
    }

    public void EndReached() {
        ShouldScroll = false;
    }

    void GetScreenshot() {
        repairMeterWrapper.SetActive(false);
        repairIcon.SetActive(false);
        System.IO.File.Delete("screenshot.png");
        ScreenCapture.CaptureScreenshot("screenshot.png");
        Invoke("LoadScreenshot", 0.5f);
    }

    void LoadScreenshot() {
        repairMeterWrapper.SetActive(true);
        repairIcon.SetActive(true);

        int imageHeight = Screen.height;
        int imageWidth = Screen.width;

        byte[] fileData = System.IO.File.ReadAllBytes("screenshot.png");
        Texture2D tex2d = new Texture2D(imageWidth, imageHeight);
        tex2d.LoadImage(fileData);

        puzzleGrid.cellSize = new Vector2(canvasRectTransform.rect.width / puzzleCols, canvasRectTransform.rect.height / puzzleRows);
        float cutoutWidth = imageWidth / puzzleCols;
        float cutoutHeight = imageHeight / puzzleRows;
        List<Sprite> spritePieces = new List<Sprite>();
        for (int row = 0; row < puzzleRows; row++)
        {
            for (int col = 0; col < puzzleCols; col++)
            {
                Sprite newSprite = Sprite.Create(tex2d, new Rect(col * cutoutWidth, imageHeight - (row * cutoutHeight) - cutoutHeight, cutoutWidth, cutoutHeight), Vector2.zero);
                // Sprite newSprite = Sprite.Create(tex2d, new Rect(0, 0, cutoutWidth, cutoutHeight), Vector2.zero);
                spritePieces.Add(newSprite);
            }
        }
        foreach (var sprite in spritePieces)
        {
            var piece = Instantiate(puzzlePiecePrefab);
            piece.transform.parent = puzzleGrid.transform;
            piece.GetComponent<Image>().sprite = sprite;
            piece.GetComponent<Tag>().value = spritePieces.IndexOf(sprite);
            piece.GetComponent<Button>().onClick.AddListener(delegate { CellClicked(piece); });
        }
        puzzleGrid.gameObject.SetActive(true);
        Invoke("ShufflePuzzle", 1f);
    }

    GameObject clickedCell; 
    void CellClicked(GameObject cell)
    {
        cell.GetComponent<Image>().color = Color.yellow;
        if (clickedCell != null) {
            // swap this cell and clickedCell
            var newIndex = cell.transform.GetSiblingIndex();
            var oldIndex = clickedCell.transform.GetSiblingIndex();
            clickedCell.transform.SetSiblingIndex(newIndex);
            cell.transform.SetSiblingIndex(oldIndex);
            cell.GetComponent<Image>().color = Color.white;
            clickedCell.GetComponent<Image>().color = Color.white;
            clickedCell = null;
            // check for a win
            CheckForPuzzleWin();
        } else {
            // set this cell as the one to swap with
            clickedCell = cell;
        }
    }

    void ShufflePuzzle() {
        int count = puzzleGrid.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            // var temp = puzzleGrid.transform.GetChild(i).GetComponent<Image>().sprite;
            // var rand = Random.Range(0, count - 1);
            // var other = puzzleGrid.transform.GetChild(rand).GetComponent<Image>().sprite;
            // puzzleGrid.transform.GetChild(i).GetComponent<Image>().sprite = other;
            // puzzleGrid.transform.GetChild(rand).GetComponent<Image>().sprite = temp;
            var temp = puzzleGrid.transform.GetChild(i);
            var rand = Random.Range(0, count - 1);
            puzzleGrid.transform.GetChild(rand).SetSiblingIndex(i);
            if (i < count - 1) {
                puzzleGrid.transform.GetChild(i + 1).SetSiblingIndex(rand);
            }
        }
    }

    void CheckForPuzzleWin() {
        bool allInPlace = true;
        foreach (Transform cell in puzzleGrid.transform)
        {
            if (cell.GetComponent<Tag>().value != cell.transform.GetSiblingIndex()) {
                allInPlace = false;
            } 
        }
        if (allInPlace) {
            foreach (Transform cell in puzzleGrid.transform)
            {
                Destroy(cell.gameObject);   
            }
            puzzleGrid.gameObject.SetActive(false);
            foreach (var item in obstacles)
            {
                Destroy(item.gameObject); 
            }
            obstacles.Clear();
            motorcycle.Repare();
            ShouldScroll = true;
            repairMeter.sizeDelta = new Vector2(maxMeterWidth, repairMeter.sizeDelta.y);
            StartSpawningObstacles();
        }
    }
}