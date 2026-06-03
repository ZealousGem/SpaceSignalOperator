using System.Collections.Generic;
using UnityEngine;

public class RenderingManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private List<BaseObstacle> ListOfObjects = new List<BaseObstacle>(); 

    private Transform shipCoordinates;  

    private float timer;

    private void OnEnable()
    {
        EventBus.Subscribe<ClearObjectFromList>(RemoveObjectFromList);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ClearObjectFromList>(RemoveObjectFromList);
    }

    private void RemoveObjectFromList(ClearObjectFromList data)
    {
        for (int i = 0; i < ListOfObjects.Count; i++)
        {
            if (data.action == ListOfObjects[i])
            {
                ListOfObjects.Remove(ListOfObjects[i]);
                return;
            }
        }
    }

    private List<BaseObstacle> getGameObjectsInMap(string layername)
    {
        int layerid = LayerMask.NameToLayer(layername);
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        List<BaseObstacle> layerObjects = new List<BaseObstacle>();

        foreach (GameObject go in allObjects)
        {
            if (go.layer == layerid && go.TryGetComponent(out BaseObstacle component))
            {
                layerObjects.Add(component);
            }
        }

        return layerObjects;

    }

    private void Start()
    {
        shipCoordinates = GameObject.FindWithTag("Player").GetComponent<Transform>();
        ListOfObjects = getGameObjectsInMap("obstacles");
        EvokeIntialObject(ListOfObjects);
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.25f)
        {
            timer = 0f;
            EvokeObject(ListOfObjects);
        }
    } 
    
    private void EvokeObject(List<BaseObstacle> obstacles)
    {

        for (int i = 0; i < obstacles.Count; i++)
        {
             float dist = (obstacles[i].transform.position - shipCoordinates.transform.position).sqrMagnitude;
             obstacles[i].CheckDistance(dist);
        }
    }

    private void EvokeIntialObject(List<BaseObstacle> obstacles)
    {

        for (int i = 0; i < obstacles.Count; i++)
        {
             obstacles[i].InitialCheck();
        }
    }
}
