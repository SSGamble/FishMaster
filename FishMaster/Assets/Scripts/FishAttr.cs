using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鱼的属性
/// </summary>
public class FishAttr : MonoBehaviour {

    public int maxNum; // 最大生成数量
    public int maxSpeed; // 最大速度

    /// <summary>
    /// 超过边界就销毁
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            Destroy(gameObject);
        }
    }

}
