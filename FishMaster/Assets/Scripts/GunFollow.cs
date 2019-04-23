using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 枪跟随鼠标指针转动
///     挂载在枪上
/// </summary>
public class GunFollow : MonoBehaviour
{
    public RectTransform canvas; // Order90Canvas
    public Camera mainCamera; // 主摄像机

    private void Update()
    {
        Vector3 mousePos; // 鼠标位置
        // RectTransformUtility.ScreenPointToWorldPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector3 worldPoint);
        // 将屏幕空间点转换为位于给定 RectTransform 平面上的世界空间中的位置
        // canvas，鼠标的位置，观察的摄像机，out 参数：把计算得到的值传递回来
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas, new Vector2(Input.mousePosition.x, Input.mousePosition.y), mainCamera, out mousePos);
        float z; // z轴的旋转
        if (mousePos.x > transform.position.x) // 鼠标位置在正前方的右边
        {
            //public static float Angle(Vector3 from, Vector3 to); // 两个向量的角度
            // Vector3.up：正前方，mousePos - transform.position：鼠标到枪的向量
            z = -Vector3.Angle(Vector3.up, mousePos - transform.position);
        }
        else // 鼠标位置在正前方的左边
        {
            z = Vector3.Angle(Vector3.up, mousePos - transform.position);
        }
        // 设置枪的目标位置
        transform.localRotation = Quaternion.Euler(0, 0, z);
    }
}
