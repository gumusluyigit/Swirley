using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string name;
    public int width;
    public int height;
    public int[,] tiles;
}

public class LevelManager : MonoBehaviour
{

    [Header("Tiles Prefabs")]
    [SerializeField] private GameObject prefabWallTile;
    [SerializeField] private GameObject prefabRoadTile;

    [SerializeField] private Vector2 offset;

    [Header("Ball and Road paint color")]
    public Color paintColor;

    [HideInInspector] public List<RoadTile>  roadTilesList = new List<RoadTile>();
    [HideInInspector] public RoadTile defaultBallRoadTile;

    [HideInInspector] public List<GameObject> wallTilesList = new List<GameObject>();


    private Color colorWall = Color.white;
    private Color colorRoad = Color.black;

    private float unitPerPixel;

    private List<LevelData> levels;
    public int currentLevelIndex = 0;


    private void Awake()
    {
        DontDestroyOnLoad(this);
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
        levels = new List<LevelData>();

        LevelData level1 = LoadLevelDataFromJSON("Level1.json");
        if (level1 != null)
        {
            levels.Add(level1);

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

        LevelData level3 = LoadLevelDataFromJSON("Level3.json");
        if (level3 != null)
        {
            levels.Add(level3);
            Debug.Log("Level 3 loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load Level 3.");
        }
        // Add more levels as needed
    }


    private LevelData LoadLevelDataFromJSON(string jsonFileName)
    {
        string jsonFilePath = Application.dataPath + "/Resources/Levels/" + jsonFileName;

        if (System.IO.File.Exists(jsonFilePath))
        {
            string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(jsonContent);
           
            if (levelData != null)
            {
                Debug.Log("Successfully loaded LevelData from JSON.");
                return levelData;
            }
            else
            {
                Debug.LogError("Failed to deserialize LevelData from JSON.");
                return null;
            }
        }
        else
        {
            Debug.LogError("JSON file not found: " + jsonFilePath);
            return null;
        }
    }



    public void LoadLevel(int levelIndex)
    {
        // Clear previous level
        ClearLevel();

        // Load and generate the new level
        LevelData levelData = levels[levelIndex];
        Debug.Log($"Loaded level: {levelData.name}, width: {levelData.width}, height: {levelData.height}");

        unitPerPixel = prefabWallTile.transform.lossyScale.x;
        float halfUnitPerPixel = unitPerPixel / 2f;

        // Calculate the offset based on the level index
        // float xOffset = levelIndex * levelData.width * unitPerPixel;

        for (int row = 0; row < levelData.height; row++)
        {
            for (int col = 0; col < levelData.width; col++)
            {
                if (levelData != null && levelData.tiles != null)
                {
                    Debug.Log($"Checking tile at row: {row}, col: {col}, value: {levelData.tiles[row, col]}");
                    int tileValue = levelData.tiles[row, col];
                    // Calculate the automatic offset to center the level
                    var autoOffsetX = -((levelData.width - 1) * unitPerPixel + halfUnitPerPixel) / 2f;
                    var autoOffsetY = -((levelData.height - 1) * unitPerPixel + halfUnitPerPixel) / 2f;

                    // Combine the serialized offset and automatic offset
                    Vector3 position = new Vector3((col * unitPerPixel - halfUnitPerPixel) + offset.x + autoOffsetX, 0f, row * unitPerPixel - halfUnitPerPixel + offset.y + autoOffsetY);
                    Debug.Log($"Tile Position: {position}, Tile Value: {tileValue}, AutoOffsetX: {autoOffsetX}, AutoOffsetY: {autoOffsetY}");

                    if (tileValue == 1) // Assuming 1 represents a wall tile
                    {
                        Spawn(prefabWallTile, position);
                    }
                    else if (tileValue == 0) // Assuming 0 represents a road tile
                    {
                        Spawn(prefabRoadTile, position);
                    }
                    // Add more conditions if you have other tile types

                    // ... (rest of the logic, if needed)
                }
                else
                {
                    Debug.LogError("LevelData or tiles list is null.");
                }
            }
        }
            // You can also perform any specific actions or setup for the current level here
        }

    public void ClearLevel()
    {
        // Destroy existing road tiles
        foreach (RoadTile roadTile in roadTilesList)
        {
            Destroy(roadTile.gameObject);
        }

        // Clear the roadTilesList
        roadTilesList.Clear();

        // Destroy existing wall tiles
        foreach (GameObject wallTile in wallTilesList)
        {
            Destroy(wallTile);
        }

        wallTilesList.Clear();
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
        {
            RoadTile roadTileComponent = obj.GetComponent<RoadTile>();
            if (roadTileComponent != null)
            {
                roadTilesList.Add(roadTileComponent);
                Debug.Log("Road tile added to roadTilesList.");
            }
            else
            {
                Debug.LogError("Road tile prefab is missing RoadTile component.");
            }
        }
        else if (prefabTile == prefabWallTile)
        {
            wallTilesList.Add(obj);  // Add the wall tile GameObject to the list
            Debug.Log("Wall tile GameObject added to wallTilesList.");
        }
        else
        {
            Debug.LogError("Prefab type not recognized.");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }
}
