using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour//为List集合添加最佳路径点
{
    //通过字典，存储坐标和对应的waypoint，
    public Dictionary<Vector2Int,WayPoint> wayPointDic = new Dictionary<Vector2Int, WayPoint>();

    //搜索的方向是上、右、下、左；一个搜索方向数组，方便搜索
    private Vector2Int[] directions = {Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};

    private Queue<WayPoint> queue = new Queue<WayPoint>();
    [SerializeField] private GameObject startPoint,endPoint;
    [SerializeField]private bool isRunning = true; //当前算法是否在执行

    private WayPoint searchCenter; //搜索的当前结点

    [SerializeField]private List<WayPoint> path = new List<WayPoint>(); //存储搜索路径

    // private void Awake() { //初始化 起始点 和 终点 的颜色
    //     startPoint.GetComponent<MeshRenderer>().material.color = Color.blue;
    //     endPoint.GetComponent<MeshRenderer>().material.color = Color.red;
    // }

    // private void Start() {

    //     LoadAllWayPoints(); //将立方体存入字典
    //     //ExploreAround(); 将会在BFS中调用
    //     BFS();
    //     CreatePath();
    // }

    public List<WayPoint> GetPath() //为其它脚本/对象 提供 获取路径的这个方法；
    {
        startPoint.GetComponent<MeshRenderer>().material.color = Color.blue;
        endPoint.GetComponent<MeshRenderer>().material.color = Color.red;

        LoadAllWayPoints(); //将立方体存入字典
        //ExploreAround(); 将会在BFS中调用
        BFS();
        CreatePath();
        return path;
    }
    
    private void LoadAllWayPoints()//将所有的立方体存入字典中
    {
        var wayPoints = FindObjectsOfType<WayPoint>();//获取所有的立方体
        foreach (var wayPoint in wayPoints)
        {
            var tempWayPoint = wayPoint.GetPosition();//转换坐标
            if(!wayPointDic.ContainsKey(tempWayPoint))
            {
                wayPointDic.Add(tempWayPoint,wayPoint);
            }
        }
    }
    private void BFS()
    {
        queue.Enqueue(startPoint.GetComponent<WayPoint>());
        while(queue.Count>0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            StopIfSearchEnd(); //判断是否到达end终点

            ExploreAround(); //查询当前结点的四周结点
            searchCenter.isExplored = true; //该结点已经被搜索过，防止重复搜索
        }
    }

    private void StopIfSearchEnd()
    {
        if(searchCenter == endPoint.GetComponent<WayPoint>())
        {
            //已经到达最后的end结点，算法终止，不进行查找了
            isRunning = false;
            Debug.Log("SToooooooooooooooooooooooooop");
        }
    }

    private void ExploreAround() //寻找周围结点
    {
        if(isRunning==false)return; //isRunning==false,算法结束，不再搜搜周围结点
        foreach (var direction in directions)
        {
            var exploreArounds = searchCenter.GetPosition() + direction;
            //Debug.Log("查找的点是："+exploreArounds);

            //TODO:四周的点高亮
            //做一个边缘检测（也可以用dic.Contains()）
            try
            {
                var neighbour = wayPointDic[exploreArounds];
                if(neighbour.isExplored || queue.Contains(neighbour))
                {
                    //该结点已经被遍历过了
                    //啥也不干
                }
                else
                {
                    //neighbour.GetComponent<MeshRenderer>().material.color = Color.green;
                    queue.Enqueue(neighbour); //当前结点入队
                    Debug.Log("exploreArounds:"+exploreArounds);

                    neighbour.exploredFrom = searchCenter; //相邻结点的exploreFrom就是当前的searchCenter
                                                           //即：这个neighbor是从这个searchCenter来的。
                }

            }
            catch
            {

            }
            
        }
    }

    private void CreatePath()
    {
        path.Add(endPoint.GetComponent<WayPoint>()); //终点信息
        WayPoint prePoint = endPoint.GetComponent<WayPoint>().exploredFrom; //终点的上一个点
        while(prePoint!=startPoint.GetComponent<WayPoint>()) //如果prePoint 不是初始点，那就一直往前找
        {
            prePoint.GetComponent<MeshRenderer>().material.color = Color.yellow; //黄色标记路径
            path.Add(prePoint);
            prePoint = prePoint.exploredFrom;
        }
        path.Add(startPoint.GetComponent<WayPoint>()); //添加初始点

        //反转整个列表
        path.Reverse();
    }

}
