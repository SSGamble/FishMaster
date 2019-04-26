using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 播放特效
/// </summary>
public class Ef_PlayEffect : MonoBehaviour {

    public GameObject[] effectPres;

    public void PlayEffect()
    {
        foreach(GameObject effect in effectPres)
        {
            Instantiate(effect);
        }
    }

}
