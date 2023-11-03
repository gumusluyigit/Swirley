using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG.Infrastructure.Utils.Swipe;
using DG.Tweening;
using UnityEngine.Events;


public class BallMovement : MonoBehaviour
{
    [SerializeField] private SwipeListener swipeListener;
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private float stepDuration = 0.1f;
    [SerializeField] private LayerMask wallsAndRoadsLayer;
    private const float MAX_RAY_DISTANCE = 10f;

    public UnityAction<List<RoadTile>, float> onMoveStart;

    private Vector3 moveDirection;
    private bool canMove = true;

    private Vector3 moveDireciton;
    // Start is called before the first frame update
    void Start()
    {
        //change default ball position
        transform.position = levelManager.defaultBallRoadTile.position;
        swipeListener.OnSwipe.AddListener(swipe => { 
            switch(swipe)
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

    private void MoveBall()
    {
        if (canMove)
        {
            canMove = false;
            //add raycast in the swipe direction (from the ball)
            RaycastHit[] hits = Physics.RaycastAll(transform.position, moveDireciton, MAX_RAY_DISTANCE, wallsAndRoadsLayer.value);
            Debug.Log("Raycast koydum");

            Vector3 targetPosition = transform.position;

            int steps = 0;

            List<RoadTile> pathRoadTiles = new List<RoadTile>(); 
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.isTrigger)
                {
                    Debug.Log("roadtile listeye eklendi");
                    pathRoadTiles.Add(hits[i].transform.GetComponent<RoadTile>());
                }

                else
                {
                    if(i == 0)
                    {
                        Debug.Log("hareket edemiyom");
                        canMove = true;
                        return;
                    }
                    //else
                    steps = i;
                    Debug.Log("Steps:" + steps);
                    targetPosition = hits[i - 1].transform.position;
                    Debug.Log("Targer Pos:" + targetPosition);
                    break;
                }
            }

            float moveDuration = stepDuration * steps;
            transform
                .DOMove(targetPosition, moveDuration)
                .SetEase(Ease.OutExpo)
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
