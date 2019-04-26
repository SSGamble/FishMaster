using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声音管理
/// </summary>
public class AudioManager : MonoBehaviour {

    // 单例
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    public AudioSource bgmAudioSource;
    public AudioClip seaWaveClip;
    public AudioClip goldClip;
    public AudioClip rewardClip;
    public AudioClip fireClip;
    public AudioClip changeClip;
    public AudioClip lvUpClip;

    private bool isMute = false; // 是否静音
    public bool IsMute
    {
        get
        {
            return isMute;
        }
    }

    private void Awake()
    {
        _instance = this;
        isMute = (PlayerPrefs.GetInt("mute", 0) == 0) ? false : true;
        DoMute();
    }

    /// <summary>
    /// 是否静音
    /// </summary>
    public void SwitchMute(bool isOn)
    {
        isMute = !isOn;
        print("audiomanager" + isOn + "/" + isMute);
        DoMute();
    }

    /// <summary>
    /// 声音开关
    /// </summary>
    private void DoMute(){
        if (isMute)
        {
            bgmAudioSource.Pause();
        }
        else
        {
            bgmAudioSource.Play();
        }
    }

    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound(AudioClip clip)
    {
        if (!isMute)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }
    }
}
