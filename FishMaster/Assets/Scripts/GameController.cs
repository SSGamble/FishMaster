using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int lv = 0; // 等级
    private float speed = 10f; // 子弹的飞行速度

    public Text oneShootCostTxt; // 一炮多少钱的 UI

    public Transform bulletHolder; // 存放子弹的父物体
    public GameObject[] gunGos; // 炮数组
    public GameObject[] bullet1Gos; // 第一档炮，4 组子弹
    public GameObject[] bullet2Gos; // 第二档炮，4 组子弹
    public GameObject[] bullet3Gos; // 第三档炮，4 组子弹
    public GameObject[] bullet4Gos; // 第四档炮，4 组子弹
    public GameObject[] bullet5Gos; // 第五档炮，4 组子弹

    // 每一炮所需的金币数和造成的伤害值，炮的档数
    // 共有 5 档炮，每一档炮有 4 档子弹，即 2Gun(5-30) 3Gun(40-70) 4Gun(80-200) SGun(300-600) 1000Gun(700-1000)
    private int[] oneShootCosts = { 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
    private int costIndex = 0; // 使用的第几档的炮弹,oneShootCosts 的索引

    private void Update()
    {
        Fire();
        ChangeBulletCost();
    }

    /// <summary>
    /// 开火
    /// </summary>
    private void Fire()
    {
        GameObject[] useBullets = bullet1Gos; // 哪一组子弹
        int bulletIndex; // 这一组子弹中的哪一个
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // 没有点击到游戏物体上
        {
            switch (costIndex / 4) // 哪一组子弹
            {
                case 0: useBullets = bullet1Gos; break;
                case 1: useBullets = bullet2Gos; break;
                case 2: useBullets = bullet3Gos; break;
                case 3: useBullets = bullet4Gos; break;
                case 4: useBullets = bullet5Gos; break;

            }
            bulletIndex = (lv % 10 >= 9) ? 9 : lv % 10; // 根据等级判断使用哪一个子弹，每 10 级，换一个子弹的颜色
            // 实例化子弹
            GameObject bullet = Instantiate(useBullets[bulletIndex]);
            bullet.transform.SetParent(bulletHolder, false);
            bullet.transform.position = gunGos[costIndex / 4].transform.Find("FirePos").transform.position;
            bullet.transform.rotation = gunGos[costIndex / 4].transform.Find("FirePos").transform.rotation;
            bullet.GetComponent<BulletAttr>().damage = oneShootCosts[costIndex]; // 伤害/钱
            bullet.AddComponent<Ef_AutoMove>().dir = Vector3.up; // 方向
            bullet.GetComponent<Ef_AutoMove>().speed = bullet.GetComponent<BulletAttr>().speed; // 速度
        }
    }

    /// <summary>
    /// 滚轮控制炮的增强和减弱
    /// </summary>
    private void ChangeBulletCost()
    {
        // 上滚轮
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        { 
            OnButtonPDown(); // 增强炮
        }
        // 下滚轮
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            OnButtonMDown(); // 减弱炮
        }
    }

    /// <summary>
    /// 增强炮按钮，点击事件
    /// </summary>
    public void OnButtonPDown()
    {
        gunGos[costIndex / 4].SetActive(false); // 禁用当前使用的炮
        costIndex++; // 升一档
        costIndex = (costIndex > oneShootCosts.Length - 1) ? 0 : costIndex; // 越界检查
        gunGos[costIndex / 4].SetActive(true); // 启用升档后的炮
        oneShootCostTxt.text = "$" + oneShootCosts[costIndex]; // 改变 UI
    }

    /// <summary>
    /// 减弱炮按钮，点击事件
    /// </summary>
    public void OnButtonMDown()
    {
        gunGos[costIndex / 4].SetActive(false); // 禁用当前使用的炮
        costIndex--; // 减一档
        costIndex = (costIndex < 0) ? oneShootCosts.Length - 1 : costIndex; // 越界检查
        gunGos[costIndex / 4].SetActive(true); // 启用升档后的炮
        oneShootCostTxt.text = "$" + oneShootCosts[costIndex]; // 改变 UI
    }
}
