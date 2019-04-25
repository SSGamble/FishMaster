using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 销毁自身
/// </summary>
public class Ef_DestroySelf : MonoBehaviour {

    public float delay = 1f;

    private void Start()
    {
        Destroy(gameObject, delay);
    }
}
