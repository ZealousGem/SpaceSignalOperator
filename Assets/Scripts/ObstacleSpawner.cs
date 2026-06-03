using System.Collections.Generic;
using UnityEngine;

public struct Obstacle
{
    public GameObject obstacle;

    public float frequency;

    public Obstacle(GameObject _obstacle, float _frequncy)
    {
        obstacle = _obstacle;
        frequency = _frequncy;
    }
}

public class ObstacleSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform shipCoordinates;

    private Vector3 TopLeftBorder = new Vector3(300, 0, 96);

    private Vector3 TopRightBorder = new Vector3(-300, 0, 96);

    private Vector3 BottomRightBorder= new Vector3(-300, 0, -200); 

    private Vector3 BottomLeftBorder = new Vector3(300, 0, -200);

    private void Awake()
    {
         shipCoordinates = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    //private Ien


    // Update is called once per fram
}
