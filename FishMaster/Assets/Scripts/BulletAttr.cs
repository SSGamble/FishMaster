using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹属性
/// </summary>
public class BulletAttr : MonoBehaviour
{

    public int speed; // 速度
    public int damage; // 伤害值
    public GameObject webPre; // 网

    /// <summary>
    /// 触发检测
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            Destroy(gameObject);
        }
        if (collision.tag == "Fish")
        {
            GameObject web = Instantiate(webPre);
            web.transform.SetParent(gameObject.transform.parent, false);
            web.transform.position = gameObject.transform.position;
            web.GetComponent<WebAttr>().damage = damage;
            Destroy(gameObject);
        }
    }
}
