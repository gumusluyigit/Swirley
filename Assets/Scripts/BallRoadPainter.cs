using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallRoadPainter : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private BallMovement ballMovement;
    [SerializeField] private MeshRenderer ballMeshRenderer;
    [SerializeField] private AudioClip win;

    [SerializeField] private GameObject winPanel;

    private AudioSource audioSource;

    public int paintedRoadTiles = 0;

    // Start is called before the first frame update
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
        //paint ball
        ballMeshRenderer.material.color = levelManager.paintColor;

        //paint default ball tile
        Paint(levelManager.defaultBallRoadTile, .5f, 0f);

        //paint ball road
        ballMovement.onMoveStart += OnBallMoveStartHandler;
    }

    private void OnBallMoveStartHandler(List<RoadTile> roadTiles, float totalDuration)
    {
        float stepDuration = totalDuration / roadTiles.Count;
        for(int i = 0; i < roadTiles.Count; i++)
        {
            RoadTile roadTile = roadTiles[i];
            if (!roadTile.isPainted)
            {
                float duration = totalDuration / 2f;
                float delay = i * (stepDuration / 2f);
                Paint(roadTile, duration, delay);

                //check for completion
                if(paintedRoadTiles == levelManager.roadTilesList.Count)
                {
                    
                    Debug.Log("Level Completed");
                    winPanel.SetActive(true);
                    SoundFXManager.instance.PlaySoundFXClip(win, transform, 1f);
                    //Load new level
                }
            }
        }
    }


    private void Paint (RoadTile roadTile, float duration, float delay)
    {
        roadTile.meshRenderer.material
            .DOColor(levelManager.paintColor, duration)
            .SetDelay(delay);

        roadTile.isPainted = true;
        paintedRoadTiles++;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
