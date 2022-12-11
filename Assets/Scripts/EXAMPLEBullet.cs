using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXAMPLEGUN:  MonoBehaviour
{
    // 1. La posicion de spawn debe ser relativa al player.
    // 2. No deben spawnear enemigos encima del player

    public Transform player;
    public float safeRadius;
    public Vector2 spawnRect;
    [Space]
    public GameObject orcPrefab;
    public int enemyLimit;
    public float spawnTime; //3
    
    private float spawnCounter;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(player.position, new Vector3(spawnRect.x, spawnRect.y, 0.1f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, safeRadius);

    }

    void Update()
    {
        spawnCounter += Time.deltaTime;
        if(spawnCounter > spawnTime)
        {
            spawnCounter = 0f;
            Vector2 spawnPosition;
            do
            {
                float spawnX = Random.Range(player.position.x - spawnRect.x / 2f, player.position.x + spawnRect.x / 2f );
                float spawnY = Random.Range(player.position.y - spawnRect.y / 2f, player.position.y + spawnRect.y / 2f );
                spawnPosition = new Vector2(spawnX, spawnY);
            }while(Vector2.Distance(player.position, spawnPosition) <= safeRadius);

            Instantiate(orcPrefab, spawnPosition, Quaternion.identity, null);
        }
    }

}
