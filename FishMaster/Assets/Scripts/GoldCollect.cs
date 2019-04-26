using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 金币收集器
/// </summary>
public class GoldCollect : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Gold")
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.goldClip);
            Destroy(collision.gameObject);
        }
    }
}
