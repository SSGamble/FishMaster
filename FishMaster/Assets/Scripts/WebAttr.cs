using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 网属性
/// </summary>
public class WebAttr : MonoBehaviour {

    public float disapperTime;
    public int damage;

    private void Start()
    {
        Destroy(gameObject, disapperTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fish")
        {
            collision.SendMessage("TakeDamage", damage);
        }
    }
}
