using UnityEngine;

public class EnemiSpawner : MonoBehaviour
{
   public GameObject EnemiPrefab;
    public float spawnInterval = 5f;
    private float timer;
    public Transform[] spawnPoints;
    public int maxEnemis = 10;
    private int currentEnemiCount = 0;
    public Transform Flag;
    public GameObject Parent;
    public WinLooseScript WinLooseScript;

    private void Start()
    {
        timer = 0f;
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }

    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval  && currentEnemiCount < maxEnemis)
        {
            SpawnEnemi();
            currentEnemiCount++;
            timer = 0f;
        }
    }

    private void SpawnEnemi()
    {
        if (spawnPoints.Length == 0) return;
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];
        GameObject Enemi = Instantiate(EnemiPrefab, spawnPoint.position, spawnPoint.rotation);
        var ai = Enemi.GetComponent<EnemiAi>();
        ai.Flag = Flag;
        Enemi.transform.parent = Parent.transform;
        WinLooseScript.Enemies.Add(Enemi);
    }

}
