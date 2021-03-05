using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("關卡資料")]
    public LevelCount levelCount;
    [Header("當前關卡小怪數量")]
    public int allCount = 0;
    [Header("已經殺怪數量")]
    public int taskCount;
    [Header("設定畫面")]
    public GameObject panelSet;
    [Header("死亡畫面")]
    public GameObject panelDead;
    [Header("勝利畫面")]
    public GameObject panelWin;
    [Header("繼續遊戲按鈕")]
    public Button resumeGame;
    [Header("回到主選單按鈕")]
    public Button backtoMenu;
    [Header("離開遊戲按鈕")]
    public Button quitGame;
    [Header("回到主選單按鈕")]
    public Button backtoMenu_D;
    [Header("離開遊戲按鈕")]
    public Button quitGame_D;
    [Header("回到主選單按鈕")]
    public Button backtoMenu_W;
    [Header("離開遊戲按鈕")]
    public Button quitGame_W;

    private Player player;
    private Button setButton;    // 設定按鈕
    private Text taskText;       // 任務文字

    private void Start()
    {
        setButton = GameObject.Find("設定按鈕").GetComponent<Button>();
        player = GameObject.Find("主角2").GetComponent<Player>();
        taskText = GameObject.Find("任務數量").GetComponent<Text>();

        allCount = levelCount.count;
        taskCount = levelCount.taskCount;

        setButton.onClick.AddListener(ShowPanelSet);
        resumeGame.onClick.AddListener(ResumeGame);

        backtoMenu.onClick.AddListener(BackToMenu);
        backtoMenu_D.onClick.AddListener(BackToMenu);
        backtoMenu_W.onClick.AddListener(BackToMenu);

        quitGame.onClick.AddListener(QuitGame);
        quitGame_D.onClick.AddListener(QuitGame);
        quitGame_W.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        ShowPanelDead();
        Victory();
        UpdateTask();
    }

    /// <summary>
    /// 顯示設定畫面
    /// </summary>
    private void ShowPanelSet()
    {
        Time.timeScale = 0;
        panelSet.SetActive(true);
    }

    /// <summary>
    /// 顯示死亡畫面
    /// </summary>
    private void ShowPanelDead()
    {
        if (player.hp <= 0)
        {
            Invoke("Dead", 0.5f);
        }
    }

    /// <summary>
    /// 顯示勝利畫面
    /// </summary>
    private void ShowPanelWin()
    {
        panelWin.SetActive(true);
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    private void Dead()
    {
        Time.timeScale = 0;
        panelDead.SetActive(true);
    }

    /// <summary>
    /// 繼續遊戲
    /// </summary>
    private void ResumeGame()
    {
        Time.timeScale = 1;
        panelSet.SetActive(false);
    }

    /// <summary>
    /// 回到主選單
    /// </summary>
    private void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    private void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 遊戲獲勝
    /// </summary>
    private void Victory()
    {
        if (allCount <= 0)
        {
            Invoke("ShowPanelWin", 1.5f);
        }
    }

    /// <summary>
    /// 更新任務條
    /// </summary>
    private void UpdateTask()
    {
        taskText.text = taskCount + " / " + levelCount.count;
    }


}
