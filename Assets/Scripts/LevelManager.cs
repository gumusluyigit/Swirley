using System.Collections;
using System.Collections.Generic;
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

        // Add more levels as needed
    }


    private LevelData LoadLevelDataFromJSON(string jsonFileName)
    {
        string jsonFilePath = Application.dataPath + "/Resources/Levels/" + jsonFileName;

        if (System.IO.File.Exists(jsonFilePath))
        {
            string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(jsonContent);

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



    private void LoadLevel(int levelIndex)
    {
        // Clear previous level
        ClearLevel();

        // Load and generate the new level
        LevelData levelData = levels[levelIndex];
        unitPerPixel = prefabWallTile.transform.lossyScale.x;
        float halfUnitPerPixel = unitPerPixel / 2f;

        for (int row = 0; row < levelData.height; row++)
        {
            for (int col = 0; col < levelData.width; col++)
            {
                if (levelData != null && levelData.tiles != null)
                {
                    int tileValue = levelData.tiles[row, col];
                    Vector3 position = new Vector3(col * unitPerPixel - halfUnitPerPixel, 0f, row * unitPerPixel - halfUnitPerPixel);

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
        else
            Debug.LogError("prefabtitle ve prefabroadtitle ayný deðil");
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }
}
