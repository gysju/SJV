using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    //[System.Serializable]
    //public struct EnemyWave
    //{
    //    [System.Serializable]
    //    public struct Spawn
    //    {
    //        public BaseEnemy Enemy;
    //        public Vector3 SpawnPosition;
    //        public Vector3 SpawnRotation;
    //        public Vector3 AttackPosition;
    //    }

    //    public float timeBeforeNextWave;
    //    public bool nextWaveWait;
    //    public List<Spawn> Spawns;
    //}

    //public bool m_showSpawnsInEditor;
    public Transform m_player;

    public List<WaveScriptableObject> m_enemiesWaves;

    public float m_timeBeforeFirstWave = 5f;

    protected List<BaseEnemy> m_enemiesCurrentWave;

    void Start()
    {
        StartCoroutine(ManageWaves());
    }

    private bool IsCurrentWaveDestroyed()
    {
        bool allDestroyed = true;

        foreach (BaseEnemy enemy in m_enemiesCurrentWave)
        {
            allDestroyed = enemy.IsDestroyed();
            if (!allDestroyed) break;
        }

        return allDestroyed;
    }

    protected IEnumerator ManageWaves()
    {
        int currentWaveID = 0;
        WaveScriptableObject currentWave = m_enemiesWaves[currentWaveID];

        yield return new WaitForSeconds(m_timeBeforeFirstWave);
        while (currentWave)
        {
            foreach (WaveScriptableObject.Spawn spawn in currentWave.Spawns)
            {
                BaseEnemy newEnemy = Instantiate(spawn.Unit, spawn.SpawnPosition, Quaternion.Euler(spawn.SpawnRotation)).GetComponent<BaseEnemy>();
                newEnemy.ResetUnit(spawn.SpawnPosition, spawn.AttackPosition, m_player);
                if (currentWave.nextWaveWait)
                {
                    m_enemiesCurrentWave.Add(newEnemy);
                }
            }

            if (currentWave.nextWaveWait)
            {
                yield return IsCurrentWaveDestroyed();
                m_enemiesCurrentWave.Clear();
            }
            yield return new WaitForSeconds(currentWave.timeBeforeNextWave);
            currentWave = m_enemiesWaves[++currentWaveID];
        }
    }

    void Update()
    {

    }

    //void OnDrawGizmos()
    //{
    //    if(m_showSpawnsInEditor)
    //    {
    //        foreach (EnemyWave enemyWave in m_enemiesWaves)
    //        {
    //            foreach (EnemyWave.Spawn spawn in enemyWave.Spawns)
    //            {
    //                Mesh mesh;
    //                if(spawn.Enemy)
    //                {
    //                    mesh = spawn.Enemy.GetComponentsInChildren<MeshFilter>()[0].sharedMesh;
    //                    Gizmos.color = Color.green;
    //                    Gizmos.DrawWireMesh(mesh, spawn.SpawnPosition, Quaternion.Euler(spawn.SpawnRotation));
    //                    Gizmos.color = Color.red;
    //                    Gizmos.DrawWireMesh(mesh, spawn.AttackPosition, Quaternion.Euler(spawn.SpawnRotation));
    //                }
    //                else
    //                {
    //                    Gizmos.color = Color.green;
    //                    Gizmos.DrawWireSphere(spawn.SpawnPosition, 1f);
    //                    Gizmos.color = Color.red;
    //                    Gizmos.DrawWireSphere(spawn.AttackPosition, 1f);
    //                }
    //            }
    //        }
    //    }
    //}
}