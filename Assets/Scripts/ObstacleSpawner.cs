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


    public float radius;
    public List<Obstacle> obstacles;
    public LayerMask mask;

    private Vector3 TopLeftBorder = new Vector3(300, 0, 96);

    private Vector3 TopRightBorder = new Vector3(-300, 0, 96);

    private Vector3 BottomRightBorder= new Vector3(-300, 0, -200); 

    private Vector3 BottomLeftBorder = new Vector3(300, 0, -200);

    private Collider[] colliders;


    void Start() => SpawnObjectType();
    
    private void SpawnObjectType()
    {
        if(obstacles.Count == 0) return;

        for (int i = 0; i < obstacles.Count; i++)
        {
            float counter = 0f;

            while (counter < obstacles[i].frequency)
            {
                SpawnObject(obstacles[i]);
                counter++;
            }
        }

        
    }

    private void SpawnObject(Obstacle obj)
    {
        bool canSpawn = false;
        Vector3 SpawnPos = new Vector3();
        int safetyNet = 0;

        while (!canSpawn)
        {
             float SpawnPointX = Random.Range(TopRightBorder.x, BottomRightBorder.x);
             float SpawnPointZ = Random.Range(BottomLeftBorder.z, TopLeftBorder.z);
             
             SpawnPos = new Vector3(SpawnPointX, 0, SpawnPointZ);
             canSpawn = PreventOverlap(SpawnPos);

             safetyNet++;

             if (safetyNet > 50)
             {
              Debug.Log("could not find suitable spaw point");    
              break; 
             }
        }

        GameObject newObstacle = Instantiate(obj.obstacle, SpawnPos, Quaternion.identity);
    }

    private bool PreventOverlap(Vector3 SpawnPos)
    {
        colliders = Physics.OverlapSphere(transform.position, radius, mask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 CentrePoint = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x;
            float heigth = colliders[i].bounds.extents.z;

            float leftExtent = CentrePoint.x - width;
            float rightExtent = CentrePoint.x + width;
            float lowerExtent = CentrePoint.z - heigth;
            float upperExtent = CentrePoint.z + heigth;

            if(SpawnPos.x >= leftExtent && SpawnPos.x <= rightExtent)
            {
                if (SpawnPos.z >= lowerExtent && SpawnPos.z >= upperExtent)
                {
                    return false;
                }
            }

        }

         return true;

    }
}
