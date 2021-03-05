using UnityEngine;

[CreateAssetMenu(fileName = "第關資料", menuName = "關卡/關卡資料")]
public class LevelCount : ScriptableObject
{
    [Header("關卡敵人數量"), Range(0, 8000)]
    public int count;
    [Header("目前殺敵數量"), Range(0, 8000)]
    public int taskCount;
}
