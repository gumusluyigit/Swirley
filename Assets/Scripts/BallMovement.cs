using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG.Infrastructure.Utils.Swipe;
using DG.Tweening;
using UnityEngine.Events;
using System.Linq;

public class BallMovement : MonoBehaviour
{
    private SwipeListener swipeListener;

    [SerializeField] private float stepDuration = 0.1f;
    [SerializeField] private LayerMask wallsAndRoadsLayer;
    private const float MAX_RAY_DISTANCE = 100f;

    public UnityAction<List<RoadTile>, float> onMoveStart;

    [SerializeField] private AudioClip hitTheWall;

    public AnimationCurve moveCurve;

    private Vector3 moveDireciton;
    private bool canMove = true;

    
    // Start is called before the first frame update
    void Start()
    {
        if (SwipeListener.Instance != null)
        {
            swipeListener = SwipeListener.Instance;
            //change default ball position
            transform.position = LevelManager.Instance.defaultBallRoadTile.position;

            swipeListener.OnSwipe.AddListener(swipe => {
                switch (swipe)
                {
                    case "Right":
                        moveDireciton = Vector3.right;
                        break;

                    case "Left":
                        moveDireciton = Vector3.left;
                        break;

                    case "Up":
                        moveDireciton = Vector3.forward;
                        break;

                    case "Down":
                        moveDireciton = Vector3.back;
                        break;
                }
                MoveBall();
            });
        }
        else
        {
            Debug.LogError("SwipeListener instance is null. Make sure it's properly set up as a singleton.");
        }
    }

    private void MoveBall()
    {
        if (canMove)
        {
            canMove = false;
            //add raycast in the swipe direction (from the ball)
            RaycastHit[] hits = Physics.RaycastAll(transform.position, moveDireciton, MAX_RAY_DISTANCE, wallsAndRoadsLayer.value);
            // Sort the hits by distance from the starting point (closest first).
            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
           

            Vector3 targetPosition = transform.position;

            int steps = 0;

            List<RoadTile> pathRoadTiles = new List<RoadTile>(); 

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.isTrigger)
                {
                    pathRoadTiles.Add(hits[i].transform.GetComponent<RoadTile>());
                }

                else
                {
                    if(i == 0)
                    {
                        canMove = true;
                        return;
                    }
                    SoundFXManager.instance.PlaySoundFXClip(hitTheWall, transform, 1f);
                    //else
                    steps = i;
                    targetPosition = hits[i - 1].transform.position;
                    break;
                }
            }

            float moveDuration = stepDuration * steps;
            transform
                .DOMove(targetPosition, moveDuration)
                .SetEase(moveCurve)
                .OnComplete(() => canMove = true);

            if (onMoveStart != null)
                onMoveStart.Invoke(pathRoadTiles, moveDuration);          
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
