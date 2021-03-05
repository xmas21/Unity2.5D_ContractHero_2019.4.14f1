using UnityEngine;

public class Enemybirth : MonoBehaviour
{
    [Header("生成的怪物")]
    public GameObject enemyObject;
    [Header("生成位置")]
    public Transform[] spawnPoint = new Transform[4];

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
            for (int i = 0; i < 4; i++)
            {
                Instantiate<GameObject>(enemyObject, spawnPoint[i].position, spawnPoint[i].rotation);
            }
        }
    }
}
