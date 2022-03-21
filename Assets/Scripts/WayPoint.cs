using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour //将正方体实际的trans坐标转换成上面的坐标
{
    public bool isExplored; //标记是否搜索过
    public WayPoint exploredFrom; //从哪个节点搜索来的，本结点的上一个结点
    public Vector2Int GetPosition()//实际的trans坐标÷1.5 = 正方体上方的坐标
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x/1.5f),
                              Mathf.RoundToInt(transform.position.z/1.5f));
    }
}
