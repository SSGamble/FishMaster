using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鱼的属性
/// </summary>
public class FishAttr : MonoBehaviour {

    public int hp; // 生命值
    public int exp; // 经验
    public int gold; // 金币
    public int maxNum; // 最大生成数量
    public int maxSpeed; // 最大速度
    public GameObject diePre; // 死亡动画
    public GameObject goldPre; // 金币

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
            GameController.Instance.gold += gold;
            GameController.Instance.exp += exp;
            // 死亡特效
            GameObject die = Instantiate(diePre);
            die.transform.SetParent(gameObject.transform.parent, false);
            die.transform.position = transform.position;
            die.transform.rotation = transform.rotation;
            Destroy(gameObject);
            // 掉落金币
            GameObject goldGo = Instantiate(goldPre);
            goldGo.transform.SetParent(gameObject.transform.parent, false);
            goldGo.transform.position = transform.position;
            goldGo.transform.rotation = transform.rotation;

            // 播放特效
            if (gameObject.GetComponent<Ef_PlayEffect>() != null)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.rewardClip);
                gameObject.GetComponent<Ef_PlayEffect>().PlayEffect();
            }

            Destroy(gameObject);
        }
    }

}
