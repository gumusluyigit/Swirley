using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void OnLevelButtonClicked()
    {
        SceneManager.LoadScene(2);
        // Find the LevelManager object in the scene
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        // If LevelManager is found, call the LoadLevel method with the inferred level index
        if (levelManager != null)
        {
            levelManager.ClearLevel();

            // Get the sibling index of the button (assuming buttons are ordered in the hierarchy)
            int buttonIndex = transform.GetSiblingIndex();

            // Load the level based on the button index (add 1 to match the level numbering)
            levelManager.LoadLevel(buttonIndex + 1);
        }
        else
        {
            Debug.LogError("LevelManager not found in the scene. Make sure it exists and is active.");
        }
    }
}
