using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 金币移动到钱袋
///     挂载在金币上
/// </summary>
public class Ef_MoveTo : MonoBehaviour {

    private GameObject goldCollect; // 钱袋

    private void Start()
    {
        goldCollect = GameObject.Find("GoldCollect");
    }

    private void Update()
    {
        // 插值移动
        transform.position = Vector3.MoveTowards(transform.position, goldCollect.transform.position, 100 * Time.deltaTime);
    }
}
