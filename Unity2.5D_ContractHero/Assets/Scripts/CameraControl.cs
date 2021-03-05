using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform player;           // 玩家位置

    [Header("跟蹤速度"), Range(0, 10)]
    public float speed = 1.5f;
    [Header("上方限制")]
    public float top = -3;
    [Header("下方限制")]
    public float bottom = 5;
    [Header("左方限制")]
    public float left = -14;
    [Header("右方限制")]
    public float right = 27;

    private void Start()
    {
        player = GameObject.Find("主角2").transform;
    }

    /// <summary>
    /// 延後更新(在Update之後執行:攝影機推薦)
    /// </summary>
    private void LateUpdate()
    {
        Track();
    }

    /// <summary>
    /// 追蹤玩家
    /// </summary>
    private void Track()
    {
        Vector3 posplayer = player.position;
        Vector3 posCamera = transform.position;

        posplayer.x = Mathf.Clamp(posplayer.x, left, right);              // 設定左右限制
        posplayer.z = Mathf.Clamp(posplayer.z, top, bottom);              // 設定上下限制

        transform.position = Vector3.Lerp(posCamera, posplayer, speed);   // camera 往 player 前進 speed
    }

}

