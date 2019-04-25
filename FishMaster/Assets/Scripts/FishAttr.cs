using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鱼的属性
/// </summary>
public class FishAttr : MonoBehaviour {

    public int hp; // 生命值
    public int maxNum; // 最大生成数量
    public int maxSpeed; // 最大速度
    public GameObject diePre; // 死亡动画

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

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="value"></param>
    private void TakeDamage(int value)
    {
        hp -= value;
        if (hp<=0)
        {
            GameObject die = Instantiate(diePre);
            die.transform.SetParent(gameObject.transform.parent, false);
            die.transform.position = transform.position;
            die.transform.rotation = transform.rotation;
            Destroy(gameObject);
        }
    }

}
