using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自动移动
/// </summary>
public class Ef_AutoMove : MonoBehaviour {

    public float speed = 1f;
    public Vector3 dir = Vector3.right;

	void Update () {
        transform.Translate(dir * speed * Time.deltaTime);
	}
}
