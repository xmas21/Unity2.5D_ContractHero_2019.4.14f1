using UnityEngine;

public class BossBirth : MonoBehaviour
{
    [Header("魔王")]
    public GameObject boss;
    [Header("生成位置")]
    public Transform spawnPoint;

    private BoxCollider box;

    private void Start()
    {
        box = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "主角2")
        {
            box.enabled = false;
            Instantiate<GameObject>(boss, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
