using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMaker : MonoBehaviour
{
    public Transform fishHolder; // 存放生成鱼的父物体
    public Transform[] genPositions; // 鱼的生成位置
    public GameObject[] fishPrefabs; // 所有鱼的预制体
    private float fishGenWaitTime = 0.5f; // 每条鱼生成的时间间隔，否则所有鱼会重叠在一起
    private float waveGenWaitTime = 0.3f; // 每波鱼生成的时间间隔

    void Start()
    {
        InvokeRepeating("MakeFish", 0, waveGenWaitTime);
    }

    /// <summary>
    /// 鱼的生成和移动
    /// </summary>
    private void MakeFish()
    {
        int genPosIndex = Random.Range(0, genPositions.Length); // 随机位置
        int fishPreIndex = Random.Range(0, fishPrefabs.Length); // 随机种类
        int maxNum = fishPrefabs[fishPreIndex].GetComponent<FishAttr>().maxNum; // 获取最大数量
        int maxSpeed = fishPrefabs[fishPreIndex].GetComponent<FishAttr>().maxSpeed; // 获取最大速度
        int num = Random.Range((maxNum / 2 + 1), maxNum); // 随机数量
        int speed = Random.Range(maxSpeed / 2, maxSpeed); // 随机速度
        int moveType = Random.Range(0, 2); // 鱼的移动模式,0直走，1转弯
        int angOffset; // 仅直走生效，直走的倾斜角
        int angSpeed; // 仅转弯生效，转弯的角速度

        if (moveType == 0)
        {
            angOffset = Random.Range(-22, 22);
            StartCoroutine(GenStraightFish(genPosIndex, fishPreIndex, num, speed, angOffset));
        }
        else
        {
            if (Random.Range(0, 1) == 0) // 1/2，是否取负的角速度
            {
                angSpeed = Random.Range(-15, -9);
            }
            else
            {
                angSpeed = Random.Range(9, 15);
            }
            StartCoroutine(GenTrunFish(genPosIndex, fishPreIndex, num, speed, angSpeed));
        }
    }

    /// <summary>
    /// 直行鱼的生成
    /// </summary>
    /// <param name="genPosIndex"></param>
    /// <param name="fishPreIndex"></param>
    /// <param name="num"></param>
    /// <param name="speed"></param>
    /// <param name="angOffset"></param>
    /// <returns></returns>
    IEnumerator GenStraightFish(int genPosIndex, int fishPreIndex, int num, int speed, int angOffset)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject fish = Instantiate(fishPrefabs[fishPreIndex]); // 生成鱼
            fish.transform.SetParent(fishHolder, false);
            fish.transform.localPosition = genPositions[genPosIndex].localPosition;
            fish.transform.localRotation = genPositions[genPosIndex].localRotation;
            fish.transform.Rotate(0, 0, angOffset); // 加上一个随机的倾斜角
            fish.GetComponent<SpriteRenderer>().sortingOrder += i; // 每生成一个鱼，就让他的渲染层级 +1，防止大的鱼 fishGenWaitTime 不够，导致头尾相接后，渲染的闪烁问题
            fish.AddComponent<Ef_AutoMove>().speed = speed; // 速度
            yield return new WaitForSeconds(fishGenWaitTime);
        }
    }

    /// <summary>
    /// 转弯鱼的生成
    /// </summary>
    /// <param name="genPosIndex"></param>
    /// <param name="fishPreIndex"></param>
    /// <param name="num"></param>
    /// <param name="speed"></param>
    /// <param name="angSpeed"></param>
    /// <returns></returns>
    IEnumerator GenTrunFish(int genPosIndex, int fishPreIndex, int num, int speed, int angSpeed)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject fish = Instantiate(fishPrefabs[fishPreIndex]); // 生成鱼
            fish.transform.SetParent(fishHolder, false);
            fish.transform.localPosition = genPositions[genPosIndex].localPosition;
            fish.transform.localRotation = genPositions[genPosIndex].localRotation;
            fish.GetComponent<SpriteRenderer>().sortingOrder += i; // 每生成一个鱼，就让他的渲染层级 +1，防止大的鱼 fishGenWaitTime 不够，导致头尾相接后，渲染的闪烁问题
            fish.AddComponent<Ef_AutoMove>().speed = speed; // 移动速度
            fish.AddComponent<Ef_AutoRatate>().speed = angSpeed; // 旋转速度
            yield return new WaitForSeconds(fishGenWaitTime);
        }
    }
}
