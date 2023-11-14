using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string name;
    public int width;
    public int height;
    public List<List<int>> tiles;
}

public class LevelManager : MonoBehaviour
{

    [Header("Tiles Prefabs")]
    [SerializeField] private GameObject prefabWallTile;
    [SerializeField] private GameObject prefabRoadTile;

    [Header("Ball and Road paint color")]
    public Color paintColor;

    [HideInInspector] public List<RoadTile>  roadTilesList = new List<RoadTile>();
    [HideInInspector] public RoadTile defaultBallRoadTile;

    private Color colorWall = Color.white;
    private Color colorRoad = Color.black;

    private float unitPerPixel;

    private List<LevelData> levels;
    private int currentLevelIndex = 0;

    private void Awake()
    {
        LoadLevels();
        LoadLevel(currentLevelIndex);
        // Check if roadTilesList is not empty before accessing elements
        if (roadTilesList.Count > 0)
        {
            defaultBallRoadTile = roadTilesList[0];
        }
        else
        {
            // Handle the case where roadTilesList is empty
            Debug.LogError("No road tiles found. Make sure your level contains road tiles.");
        }
    }

    private void LoadLevels()
    {
        // Load your levels from JSON files and populate the 'levels' list
        levels = new List<LevelData>();

        LevelData level1 = LoadLevelDataFromJSON("Level1.json");
        if (level1 != null)
        {
            levels.Add(level1);
            Debug.Log("Level 1 loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load Level 1.");
        }

        LevelData level2 = LoadLevelDataFromJSON("Level2.json");
        if (level2 != null)
        {
            levels.Add(level2);
            Debug.Log("Level 2 loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load Level 2.");
        }

        // Add more levels as needed
    }


    private LevelData LoadLevelDataFromJSON(string jsonFileName)
    {
        string jsonFilePath = Application.dataPath + "/" + jsonFileName;

        if (System.IO.File.Exists(jsonFilePath))
        {
            string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(jsonContent);

            return levelData;
        }
        else
        {
            Debug.LogError("JSON file not found: " + jsonFilePath);
            return null;
        }
    }


    private void LoadLevel(int levelIndex)
    {
        // Clear previous level
        ClearLevel();

        // Load and generate the new level
        LevelData levelData = levels[levelIndex];
        unitPerPixel = prefabWallTile.transform.lossyScale.x;
        float halfUnitPerPixel = unitPerPixel / 2f;

        // ... (rest of the level loading logic)

        // You can also perform any specific actions or setup for the current level here
    }

    private void ClearLevel()
    {
        // Destroy existing level objects or perform any cleanup
        // ...

        // Clear the roadTilesList
        roadTilesList.Clear();
    }

    private void Update()
    {
        // Example: Check for a condition to move to the next level (e.g., player reaching the end of the current level)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Move to the next level if available
            if (currentLevelIndex < levels.Count - 1)
            {
                currentLevelIndex++;
                LoadLevel(currentLevelIndex);
            }
            else
            {
                // Handle game completion or loop back to the first level
                // ...
            }
        }
    }

    private void Spawn(GameObject prefabTile, Vector3 position)
    {
        position.y = prefabTile.transform.position.y;

        GameObject obj = Instantiate(prefabTile, position, Quaternion.identity, transform);

        if (prefabTile == prefabRoadTile)
            roadTilesList.Add(obj.GetComponent<RoadTile>());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
