using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform shipCoordinates;
   

    void Example(List<BaseObstacle> obstacles)
{
    Vector3 playerPos = shipCoordinates.position;

    for (int i = 0; i < obstacles.Count; i++)
    {
        // 1. Get the raw offset vector
        Vector3 offset = obstacles[i].transform.position - playerPos;
        
        // 2. Get the squared distance (Incredibly fast math)
        float sqrDist = offset.sqrMagnitude; 
        
        // 3. Pass the squared distance down
       // obstacles[i].CheckDistance(sqrDist);
    }
}

    // Update is called once per fram
}
