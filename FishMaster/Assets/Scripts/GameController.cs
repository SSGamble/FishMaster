using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // 单例模式
    private static GameController _instance;
    public static GameController Instance
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

    public Text oneShootCostTxt; // 一炮多少钱的 UI
    public Text goldTxt; // 金币
    public Text lvTxt; // 等级
    public Text lvNameTxt; // 等级名字
    public Text smallCountdownTxt; // 小的倒计时
    public Text bigCountdownTxt; // 大的倒计时
    public Button bigCountdownBtn; // 大的倒计时按钮
    public Button backBtn; // 返回按钮
    public Button settingBtn; // 设置按钮
    public Slider expSlider; // 经验条

    // 特效
    public GameObject LvUpImg;
    public GameObject fireEffect;
    public GameObject changeEffect;
    public GameObject lvEffect;
    public GameObject goldEffect;
    public GameObject seaWaveEffect;

    public Image bgImg; // 背景图片
    public int bgIndex = 0; // 背景图片数组的索引
    public Sprite[] bgSprites; // 背景图片数组
    public Transform bulletHolder; // 存放子弹的父物体
    public GameObject[] gunGos; // 炮数组
    public GameObject[] bullet1Gos; // 第一档炮，4 组子弹
    public GameObject[] bullet2Gos; // 第二档炮，4 组子弹
    public GameObject[] bullet3Gos; // 第三档炮，4 组子弹
    public GameObject[] bullet4Gos; // 第四档炮，4 组子弹
    public GameObject[] bullet5Gos; // 第五档炮，4 组子弹
    // 每一炮所需的金币数和造成的伤害值，炮的档数
    // 共有 5 档炮，每一档炮有 4 档子弹，即 2Gun(5-30) 3Gun(40-70) 4Gun(80-200) SGun(300-600) 1000Gun(700-1000)
    private int[] oneShootCosts = { 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
    private int costIndex = 0; // 使用的第几档的炮弹,oneShootCosts 的索引

    private float speed = 10f; // 子弹的飞行速度
    public int gold = 500; // 初始资金
    public int lv = 0; // 等级
    public int exp = 0; // 初始经验
    public const int bigCountdown = 240; // 大奖励倒计时
    public float bigTimer = bigCountdown;
    public const int smallCountdown = 60; // 小奖励倒计时
    public float smallTimer = smallCountdown;
    private string[] lvName = { "新手", "入门", "钢铁", "青铜", "白银", "黄金", "白金", "钻石", "大师", "宗师" };
    public Color goldColor; // 没钱金币闪烁

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        gold = PlayerPrefs.GetInt("gold", gold);
        lv = PlayerPrefs.GetInt("lv", lv);
        exp = PlayerPrefs.GetInt("exp", exp);
        smallTimer = PlayerPrefs.GetFloat("scd", smallCountdown);
        bigTimer = PlayerPrefs.GetFloat("bcd", bigCountdown);
        UpdateUI();
    }

    private void Update()
    {
        ChangeBulletCost();
        Fire();
        UpdateUI();
        ChangeBG();
    }

    /// <summary>
    /// 每 20 级，换一次背景，播一次特效
    /// </summary>
    private void ChangeBG()
    {
        if (bgIndex != lv / 20)
        {
            bgIndex = lv / 20;
            AudioManager.Instance.PlaySound(AudioManager.Instance.seaWaveClip);
            Instantiate(seaWaveEffect);
            // 等级过高就只显示最后一张背景图，防止越界
            if (bgIndex>=3)
            {
                bgImg.sprite = bgSprites[3];
            }
            else
            {
                bgImg.sprite = bgSprites[bgIndex];
            }
        }
    }

    /// <summary>
    /// 更新 UI
    /// </summary>
    private void UpdateUI()
    {
        // 两个奖励计时器
        bigTimer -= Time.deltaTime;
        smallTimer -= Time.deltaTime;
        // 小的时间奖励
        if (smallTimer <= 0)
        {
            smallTimer = smallCountdown;
            gold += 50;
        }
        // 大的时间奖励，需要点击按钮来获取奖励，类似于点宝箱
        if (bigTimer <= 0 && !bigCountdownBtn.gameObject.activeSelf)
        {
            bigCountdownTxt.gameObject.SetActive(false); // 隐藏倒计时
            bigCountdownBtn.gameObject.SetActive(true); // 显示按钮
        }
        // 经验等级换算公式：升级所需经验 = 1000 + 200 * 当前等级
        while (exp >= 1000 + 200 * lv)
        {
            exp = exp - (1000 + 200 * lv); // 经验
            lv++; // 升级
            // 升级文字提醒
            LvUpImg.SetActive(true);
            LvUpImg.transform.Find("Text").GetComponent<Text>().text = lv.ToString();
            StartCoroutine(LvUpImg.GetComponent<Ef_HideSelf>().HideSelf(0.6f));
            AudioManager.Instance.PlaySound(AudioManager.Instance.lvUpClip);
            Instantiate(lvEffect);
        }
        // 金钱
        goldTxt.text = "$" + gold;
        // 等级
        lvTxt.text = lv.ToString();
        // 称号
        if ((lv/10)<=9) // 等级 <= 99
        {
            lvNameTxt.text = lvName[lv / 10]; // 称号
        }
        else
        {
            lvNameTxt.text = lvName[9]; // 若超过 99 级，一直都是宗师
        }
        // 小的倒计时
        smallCountdownTxt.text = " " + (int)smallTimer/10 + "  " + (int)smallTimer%10;
        // 大的倒计时
        bigCountdownTxt.text = (int)bigTimer + "s";
        // 经验条
        expSlider.value = ((float)exp) / (1000 + 200 * lv);
    }

    /// <summary>
    /// 开火
    /// </summary>
    private void Fire()
    {
        GameObject[] useBullets = bullet1Gos; // 哪一组子弹
        int bulletIndex; // 这一组子弹中的哪一个
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // 没有点击到游戏物体上
        {
            // 钱够不够
            if (gold - oneShootCosts[costIndex] >= 0)
            {
                switch (costIndex / 4) // 哪一组子弹
                {
                    case 0: useBullets = bullet1Gos; break;
                    case 1: useBullets = bullet2Gos; break;
                    case 2: useBullets = bullet3Gos; break;
                    case 3: useBullets = bullet4Gos; break;
                    case 4: useBullets = bullet5Gos; break;

                }
                bulletIndex = (lv % 10 >= 9) ? 9 : lv % 10; // 根据等级判断使用哪一个子弹，每 10 级，换一个子弹的颜色
                gold -= oneShootCosts[costIndex]; // 消耗钱
                AudioManager.Instance.PlaySound(AudioManager.Instance.fireClip);
                Instantiate(fireEffect);
                // 实例化子弹
                GameObject bullet = Instantiate(useBullets[bulletIndex]);
                bullet.transform.SetParent(bulletHolder, false);
                bullet.transform.position = gunGos[costIndex / 4].transform.Find("FirePos").transform.position;
                bullet.transform.rotation = gunGos[costIndex / 4].transform.Find("FirePos").transform.rotation;
                bullet.GetComponent<BulletAttr>().damage = oneShootCosts[costIndex]; // 伤害/钱
                bullet.AddComponent<Ef_AutoMove>().dir = Vector3.up; // 方向
                bullet.GetComponent<Ef_AutoMove>().speed = bullet.GetComponent<BulletAttr>().speed; // 速度
            }
            else
            {
                StartCoroutine(GoldNotEnough()); // 钱不够
            }
        }
    }

    /// <summary>
    /// 钱不够时 UI 闪烁
    /// </summary>
    /// <returns></returns>
    IEnumerator GoldNotEnough()
    {
        goldTxt.color = goldColor;
        goldTxt.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        goldTxt.color = goldColor;
    }

    /// <summary>
    /// 滚轮控制炮的增强和减弱
    /// </summary>
    private void ChangeBulletCost()
    {
        // 上滚轮
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            OnButtonPDown(); // 增强炮
        }
        // 下滚轮
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            OnButtonMDown(); // 减弱炮
        }
    }

    /// <summary>
    /// 增强炮按钮，点击事件
    /// </summary>
    public void OnButtonPDown()
    {
        gunGos[costIndex / 4].SetActive(false); // 禁用当前使用的炮
        costIndex++; // 升一档
        AudioManager.Instance.PlaySound(AudioManager.Instance.changeClip);
        Instantiate(changeEffect);
        costIndex = (costIndex > oneShootCosts.Length - 1) ? 0 : costIndex; // 越界检查
        gunGos[costIndex / 4].SetActive(true); // 启用升档后的炮
        oneShootCostTxt.text = "$" + oneShootCosts[costIndex]; // 改变 UI
    }

    /// <summary>
    /// 减弱炮按钮，点击事件
    /// </summary>
    public void OnButtonMDown()
    {
        gunGos[costIndex / 4].SetActive(false); // 禁用当前使用的炮
        costIndex--; // 减一档
        AudioManager.Instance.PlaySound(AudioManager.Instance.changeClip);
        Instantiate(changeEffect);
        costIndex = (costIndex < 0) ? oneShootCosts.Length - 1 : costIndex; // 越界检查
        gunGos[costIndex / 4].SetActive(true); // 启用升档后的炮
        oneShootCostTxt.text = "$" + oneShootCosts[costIndex]; // 改变 UI
    }

    /// <summary>
    /// 领大奖金按钮，点击事件
    /// </summary>
    public void OnBigCountdownButtonDown()
    {
        gold += 500;
        AudioManager.Instance.PlaySound(AudioManager.Instance.rewardClip);
        Instantiate(goldEffect);
        bigCountdownBtn.gameObject.SetActive(false);
        bigCountdownTxt.gameObject.SetActive(true);
        bigTimer = bigCountdown;
    }
}
