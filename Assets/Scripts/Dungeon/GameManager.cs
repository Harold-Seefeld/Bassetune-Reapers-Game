using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public DungeonMap dungeonPrefab;

    private DungeonMap dungeonInstance;

    private void Start()
    {
        BeginGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    private void BeginGame()
    {
        dungeonInstance = Instantiate(dungeonPrefab) as DungeonMap;
        StartCoroutine(dungeonInstance.Generate());
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(dungeonInstance.gameObject);
        BeginGame();
    }
}
