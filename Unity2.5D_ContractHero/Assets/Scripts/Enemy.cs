using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("敵人資料")]
    public HeroData data;

    public float rangeAttack; // 攻擊距離
    public float rangeTrack;  // 追蹤距離
    public float hp;          // 怪物血量

    private Rigidbody rig;
    private Animator ani;
    private NavMeshAgent nav;
    private Player player;
    private Transform target; // 玩家位置
    private LevelManager levelManager;
    private HpMpManager HpMpManager;

    private float timer; // 計時器
    private float dis;   // 怪跟人的距離

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        nav.speed = data.speed;
        hp = data.hp;

        target = GameObject.Find("主角2").transform;
        player = GameObject.Find("主角2").GetComponent<Player>();

        levelManager = FindObjectOfType<LevelManager>();
        HpMpManager = FindObjectOfType<HpMpManager>();

        SetCollision();
    }

    private void Update()
    {
        Move();
    }


    /// <summary>
    /// 等待
    /// </summary>
    private void Wait()
    {
        ani.SetBool("移動開關", false);
        timer += Time.deltaTime;

        if (timer >= data.attackCD)
        {
            Attack();
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        dis = Vector3.Distance(target.position, transform.position);

        if (ani.GetBool("死亡開關") || player.hp <= 0) return;

        if (target.position.x < transform.position.x)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (dis < rangeAttack)
        {
            Wait();
        }
        else if (dis < rangeTrack)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, data.speed * Time.deltaTime);

            ani.SetBool("移動開關", true);
        }
        else if (dis > rangeTrack)
        {
            ani.SetBool("移動開關", false);
        }
    }

    /// <summary>
    /// 普攻
    /// </summary>
    private void Attack()
    {
        timer = 0;
        ani.SetTrigger("攻擊開關");
    }

    /// <summary>
    /// 受傷
    /// </summary>
    private void Hit(float damage)
    {
        hp -= damage;
        StartCoroutine(HpMpManager.ShowValue(damage, (Vector3.one) / 15, Color.white));
        ani.SetTrigger("被攻擊開關");
        if (hp <= 0) Dead();
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        levelManager.allCount--;
        levelManager.taskCount++;
        ani.SetBool("死亡開關", true);
        enabled = false;

        Destroy(gameObject, 2.5f);
    }

    /// <summary>
    /// 避免碰撞
    /// </summary>
    private void SetCollision()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("怪物"), LayerMask.NameToLayer("怪物"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("玩家"), LayerMask.NameToLayer("怪物"));
    }

    /// <summary>
    /// 碰撞
    /// </summary>
    /// <param name="col">碰撞的物品</param>
    private void OnTriggerEnter(Collider col)
    {
        if (hp >= 0)
        {
            if (col.gameObject.tag == "玩家武器")
            {
                Hit(player.data.attack);
            }
            else if (col.gameObject.tag == "技能1")
            {
                Hit(player.data.skills[0].attack);
            }
            else if (col.gameObject.tag == "技能2")
            {
                Hit(player.data.skills[1].attack);
            }
            else if (col.gameObject.tag == "技能3")
            {
                Hit(player.data.skills[2].attack);
            }
        }
    }
}
