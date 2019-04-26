using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 浪花特效
/// </summary>
public class Ef_SeaWave : MonoBehaviour {

    public Vector3 temp; // 镜像翻转的位置

    private void Start()
    {
        temp = -transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, temp, 10 * Time.deltaTime);
    }
}
