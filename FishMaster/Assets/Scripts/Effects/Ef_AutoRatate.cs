using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自动旋转
/// </summary>
public class Ef_AutoRatate : MonoBehaviour {

    public float speed = 10f; // 旋转的速度
	
	void Update () {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime); // 绕 z 轴旋转
	}
}
