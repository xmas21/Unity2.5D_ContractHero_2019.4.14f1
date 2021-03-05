using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpMpManager : MonoBehaviour
{
    private RectTransform rtValue;  // 血量位置
    private Text textValue;         // 血量數值

    private void Start()
    {
        rtValue = transform.GetChild(0).GetComponent<RectTransform>();
        textValue = transform.GetChild(0).GetComponent<Text>();
    }

    private void Update()
    {
        FixAngle();
    }

    /// <summary>
    /// 固定血條螢幕角度
    /// </summary>
    private void FixAngle()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// 顯示數值
    /// </summary>
    /// <param name="value">傷害量</param>
    /// <param name="size">文字尺寸</param>
    /// <param name="valueColor">文字顏色</param>
    /// <returns></returns>
    public IEnumerator ShowValue(float value, Vector3 size, Color valueColor)
    {
        textValue.text =  value.ToString("f0");  // 數值
        valueColor.a = 0;                        // 顏色.透明度 = 0
        textValue.color = valueColor;            // 更新文字.顏色
        rtValue.localScale = size;               // 更新文字.尺寸

        for (int i = 0; i < 10; i++)
        {
            textValue.color += new Color(0, 0, 0, 1f);
            rtValue.anchoredPosition += Vector2.up * 0.1f;
            yield return new WaitForSeconds(0.01f);
        }

        rtValue.anchoredPosition = new Vector2(0, 0);
        textValue.color = new Color(0, 0, 0, 0);
    }

}
