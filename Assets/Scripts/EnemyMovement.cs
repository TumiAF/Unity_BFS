using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //[SerializeField]
    //private List<GameObject> pathWayPoints = new List<GameObject>(); //要走的路径点

    IEnumerator FindWayPoint(List<WayPoint> pathWayPoints)
    {
        foreach(var waypoint in pathWayPoints) //根据路径点进行移动
        {
            transform.position = waypoint.transform.position + new Vector3(0,1,0);
            yield return new WaitForSeconds(0.5f); //等待0.5s再次遍历
        }
    }
    private void Start() 
    {
        PathFinding pf = FindObjectOfType<PathFinding>();
        StartCoroutine(FindWayPoint(pf.GetPath())); //开启协程
    }
}
