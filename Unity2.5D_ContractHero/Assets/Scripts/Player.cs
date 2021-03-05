using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("角色資料")]
    public HeroData data;
    [Header("劍氣")]
    public GameObject sword;
    [Header("發射速度")]
    public float power;
    [Header("玩家血量")]
    public float hp;
    [Header("玩家魔量")]
    public float mp;
    [Header("每秒回血量")]
    public float restoreHp;
    [Header("每秒回魔量")]
    public float restoreMp;
    [Header("小怪")]
    public Enemy enemy;

    private Rigidbody rig;
    private Animator ani;
    private Joystick joystick;
    private BoxCollider box;     // 角色碰撞器

    private Button normalAttack; // 普攻按鈕
    private Button skill_1;      // 技能1按鈕
    private Button skill_2;      // 技能2按鈕
    private Button skill_3;      // 技能3按鈕

    private Image hpBar;         // 生命圖片
    private Image mpBar;         // 魔力圖片
    private Text lvText;         // 等級文字
    private Text hpText;         // 血量文字
    private Text mpText;         // 魔量文字

    private float timer;         // 計時器
    private float interval = 1;  // 普攻間隔時間
    private int count;           // 普攻狀態

    private float[] skillTimer = new float[3];  // 技能的計時器
    private bool[] skillStart = new bool[3];    // 技能的是否冷卻
    private Image[] imgSkills = new Image[3];   // 技能圖片
    private Image[] imgSkillsCD = new Image[3]; // 技能冷卻圖片
    private Text[] textSkills = new Text[3];    // 技能CD時間

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();

        joystick = GameObject.Find("虛擬搖桿").GetComponent<Joystick>();
        normalAttack = GameObject.Find("普攻按鈕").GetComponent<Button>();
        hpBar = GameObject.Find("血量上條").GetComponent<Image>();
        mpBar = GameObject.Find("魔力上條").GetComponent<Image>();
        lvText = GameObject.Find("等級文字").GetComponent<Text>();
        hpText = GameObject.Find("血量文字").GetComponent<Text>();
        mpText = GameObject.Find("魔量文字").GetComponent<Text>();

        hp = data.hp;
        mp = data.mp;

        normalAttack.onClick.AddListener(Attack);
        SetSkillUI();
    }

    private void Update()
    {
        Move();
        UpdateHpBar();
        AttackCount();
        TimerControl();
        UpdateSkillCD();
        Restore();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        float v = joystick.Vertical;    // 垂直
        float h = joystick.Horizontal;  // 水平

        rig.velocity = new Vector3(h * data.speed, rig.velocity.y, v * data.speed);

        ani.SetBool("跑步開關", v != 0 || h != 0);

        if (h < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (h > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    /// <summary>
    /// 普攻
    /// </summary>
    private void Attack()
    {
        if (count < 3)
        {
            timer = 0;
            count++;
            ani.SetTrigger("攻擊" + count);
        }
        else
        {
            count = 0;
        }
    }

    /// <summary>
    /// 受傷
    /// </summary>
    private void Hit()
    {
        hp -= enemy.data.attack;
        ani.SetTrigger("被攻擊開關");
        if (hp <= 0) Dead();
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        ani.SetBool("死亡開關", true);
        hpBar.fillAmount = 0;
        box.enabled = false;
        rig.useGravity = false;
        enabled = false;
    }

    /// <summary>
    /// 更新角色資訊
    /// </summary>
    private void UpdateHpBar()
    {
        hpBar.fillAmount = hp / data.hp;
        mpBar.fillAmount = mp / data.mp;
        lvText.text = "Lv." + data.exp;
        hpText.text = "[ " + hp.ToString("f0") + " / " + data.hp.ToString("f0") + " ]";
        mpText.text = "[ " + mp.ToString("f0") + " / " + data.mp.ToString("f0") + " ]";
    }

    /// <summary>
    /// 自動回血回魔
    /// </summary>
    private void Restore()
    {
        hp += restoreHp * Time.deltaTime;
        mp += restoreMp * Time.deltaTime;
        hp = Mathf.Clamp(hp, 0, data.hp);
        mp = Mathf.Clamp(mp, 0, data.mp);
        hpBar.fillAmount = hp / data.hp;
        mpBar.fillAmount = mp / data.mp;
    }

    /// <summary>
    /// 技能按鈕抓取 & 監聽
    /// </summary>
    private void SetSkillUI()
    {
        skill_1 = GameObject.Find("技能1").GetComponent<Button>();
        skill_2 = GameObject.Find("技能2").GetComponent<Button>();
        skill_3 = GameObject.Find("技能3").GetComponent<Button>();

        skill_1.onClick.AddListener(Skill_1);
        skill_2.onClick.AddListener(Skill_2);
        skill_3.onClick.AddListener(Skill_3);

        for (int i = 0; i < 3; i++)
        {
            imgSkills[i] = GameObject.Find("技能圖片 " + (i + 1)).GetComponent<Image>();
            imgSkillsCD[i] = GameObject.Find("技能冷卻圖片 " + (i + 1)).GetComponent<Image>();
            textSkills[i] = GameObject.Find("技能冷卻 " + (i + 1)).GetComponent<Text>();

            imgSkills[i].sprite = data.skills[i].image;
            textSkills[i].text = "";
        }
    }

    /// <summary>
    /// 技能CD時間
    /// </summary>
    private void UpdateSkillCD()
    {
        for (int i = 0; i < 3; i++)
        {
            if (skillStart[i])
            {
                textSkills[i].text = (data.skills[i].cd - skillTimer[i]).ToString("F0");

                imgSkillsCD[i].fillAmount = (data.skills[i].cd - skillTimer[i]) / data.skills[i].cd;
            }
            else
            {
                textSkills[i].text = "";
                imgSkillsCD[i].fillAmount = 0;
            }
        }
    }

    /// <summary>
    /// 普攻計時 + 計次
    /// </summary>
    private void AttackCount()
    {
        if (count >= 0)
        {
            if (timer <= interval)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                count = 0;
            }
        }
    }

    /// <summary>
    /// 技能計時
    /// </summary>
    private void TimerControl()
    {
        for (int i = 0; i < 3; i++)
        {
            if (skillStart[i])
            {
                skillTimer[i] += Time.deltaTime;

                // 如果 計時器 >= 冷卻時間 就 歸零並且設定為 尚未開始
                if (skillTimer[i] >= data.skills[i].cd)
                {
                    skillTimer[i] = 0;
                    skillStart[i] = false;
                }
            }
        }
    }

    /// <summary>
    /// 技能1
    /// </summary>
    public void Skill_1()
    {
        if (skillStart[0] || mp < data.skills[0].cost) return;
        mp -= data.skills[0].cost;
        ani.SetTrigger("技能1");
        skillStart[0] = true;
    }

    /// <summary>
    /// 技能2
    /// </summary>
    public void Skill_2()
    {
        if (skillStart[1] || mp < data.skills[1].cost) return;
        mp -= data.skills[1].cost;
        ani.SetTrigger("技能2");
        GameObject temp = Instantiate(sword, transform.position, transform.rotation);
        temp.GetComponent<Rigidbody>().AddForce(-transform.right * power);
        Destroy(temp, 2);
        skillStart[1] = true;
    }

    /// <summary>
    /// 技能3
    /// </summary>
    public void Skill_3()
    {
        if (skillStart[2] || mp < data.skills[2].cost) return;
        mp -= data.skills[2].cost;
        ani.SetTrigger("技能3");
        skillStart[2] = true;
    }

    /// <summary>
    /// 碰撞
    /// </summary>
    /// <param name="other">碰撞的物件</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "攻擊範圍")
        {
            Hit();
        }
    }
}
