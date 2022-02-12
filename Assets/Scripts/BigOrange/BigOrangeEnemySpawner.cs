using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOrangeEnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;

    static class SpawnOffset
    {
        static float m = 0.72f;
        public static Vector2 left   = new Vector2(-m,0);
        public static Vector2 right  = new Vector2(m, 0);
        public static Vector2 top    = new Vector2(0, m);
        public static Vector2 bottom = new Vector2(0,-m);
        public static Vector2 none   = Vector2.zero;
    }

    public void SpawnEnemiesLeft()
    {
        CreateWithAdjacentHorizontal<EnemyMovement>(enemyPrefabs[0], SpawnPoint.TopLeft);
        CreateWithAdjacentVertical<EnemyMovementTwo>(enemyPrefabs[4], SpawnPoint.BottomLeft);
    }

    public void SpawnEnemiesRight()
    {
        CreateWithAdjacentHorizontal<EnemyMovement>(enemyPrefabs[1], SpawnPoint.TopRight);
        CreateWithAdjacentVertical<EnemyMovementTwo>(enemyPrefabs[5], SpawnPoint.BottomRight);
    }

    public void SpawnEnemiesTop()
    {
        CreateEnemy<EnemyMovementThreeVariant>(enemyPrefabs[2], SpawnPoint.TLWhite, SpawnOffset.none);
        CreateEnemy<EnemyMovementThreeVariant>(enemyPrefabs[3], SpawnPoint.BRWhite, SpawnOffset.none);
        CreateEnemy<EnemyMovementThreeVariant>(enemyPrefabs[6], SpawnPoint.BLWhite, SpawnOffset.none);
        CreateEnemy<EnemyMovementThreeVariant>(enemyPrefabs[7], SpawnPoint.TRWhite, SpawnOffset.none);
    }

    void CreateWithAdjacentHorizontal<T>(GameObject prefab, SpawnPoint point)
    {
        CreateEnemy<T>(prefab, point, SpawnOffset.none);
        CreateEnemy<T>(prefab, point, SpawnOffset.left);
        CreateEnemy<T>(prefab, point, SpawnOffset.right);
    }

    void CreateWithAdjacentVertical<T>(GameObject prefab, SpawnPoint point)
    {
        CreateEnemy<T>(prefab, point, SpawnOffset.none);
        CreateEnemy<T>(prefab, point, SpawnOffset.top);
        CreateEnemy<T>(prefab, point, SpawnOffset.bottom);
    }

    void CreateEnemy<T>(GameObject prefab, SpawnPoint point, Vector2 offset)
    {
        GameObject newEnemy = Instantiate(prefab);
        PauseControl.TryAddPausable(newEnemy);
        T moveComponent = newEnemy.GetComponent<T>();
        newEnemy.transform.position = spawnPoints[(int)point].position + (Vector3)offset;
        
        if(moveComponent is EnemyMovement moveOne)
            moveOne.multiplier = 3;
        if(moveComponent is EnemyMovementTwo moveTwo)
            moveTwo.multiplier = 3;
    }
    
    enum SpawnPoint
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        TLWhite,
        BRWhite,
        BLWhite,
        TRWhite,
    }
}
