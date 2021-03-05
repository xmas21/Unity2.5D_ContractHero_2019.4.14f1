using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject panelLoading;  // 載入畫面
    public Text textLoading;         // 進度文字
    public Image imgLoading;         // 進度條
    public GameObject tip;           // 提示文字

    private Button startButton;      // 開始按鈕
    private Button QuitButton;       // 離開按鈕

    private void Start()
    {
        startButton = GameObject.Find("開始遊戲").GetComponent<Button>();
        QuitButton = GameObject.Find("退出遊戲").GetComponent<Button>();

        startButton.onClick.AddListener(LoadScean);
        QuitButton.onClick.AddListener(Quit);
    }

    /// <summary>
    /// 載入場景
    /// </summary>
    private void LoadScean()
    {
        StartCoroutine(Loading());
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    private void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 載入場景 + 載入動畫
    /// </summary>
    /// <returns></returns>
    private IEnumerator Loading()
    {
        panelLoading.SetActive(true);                               // 顯示載入畫面
        AsyncOperation ao = SceneManager.LoadSceneAsync(1);         // 同步載入場景(場景名稱)
        ao.allowSceneActivation = false;                            // 不要自動載入場景

        // 當 尚未載入場景
        while (!ao.isDone)
        {
            textLoading.text = (ao.progress / 0.9f * 100).ToString("F0") + "%";   // 更新文字
            imgLoading.fillAmount = ao.progress / 0.9f;                           // 更新進度條
            yield return null;                                                    // 延遲一個影格時間

            if (ao.progress == 0.9f)
            {
                tip.SetActive(true);

                if (Input.anyKeyDown) ao.allowSceneActivation = true;
            }
        }
    }

}
