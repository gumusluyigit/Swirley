using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Vector3 position;
    public bool isPainted;

    private void Awake()
    {
        position = transform.position;
        isPainted = false; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
