using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 利用切换材质贴图是实现的水波纹特效
/// </summary>
public class Ef_WaterWave : MonoBehaviour
{

    public Texture[] textureArr; // 贴图数组
    private int index = 0; // 贴图数组的索引
    private Material material; // 材质

    void Start()
    {
        material = GetComponent<MeshRenderer>().material; // 获取该物体上的材质
        InvokeRepeating("ChangeTexture", 0, 0.1f); // 持续切换贴图，达到动画效果
    }

    void ChangeTexture()
    {
        material.mainTexture = textureArr[index]; // 设置当前贴图
        index = (index + 1) % textureArr.Length; // 循环切换索引，用于切换下一张贴图
    }
}
