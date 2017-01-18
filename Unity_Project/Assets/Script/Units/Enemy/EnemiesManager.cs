using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public bool m_showSpawnsInEditor;
    public BaseMecha m_player;

    public Vector3 m_poolPosition = Vector3.zero;
    protected List<GroundEnemy> m_groundPool = new List<GroundEnemy>();
    protected List<AirEnemy> m_airPool = new List<AirEnemy>();

    public List<WaveObject> m_enemiesWaves;
    protected List<EnemiesWave> m_wavesSpawned = new List<EnemiesWave>();

    public float m_timeBeforeFirstWave = 5f;

    void Start()
    {
        StartCoroutine(ManageWaves());
    }

    #region Pools
    public bool IsThereUnusedGroundEnemy()
    {
        return (m_groundPool.Count > 0);
    }

    public bool IsThereUnusedAirEnemy()
    {
        return (m_airPool.Count > 0);
    }

    public GroundEnemy GetUnusedGroundEnemy(Transform parent)
    {
        GroundEnemy groundEnemy = m_groundPool[0];
        m_groundPool.RemoveAt(0);
        groundEnemy.transform.parent = parent;

        return groundEnemy;
    }

    public AirEnemy GetUnusedAirEnemy(Transform parent)
    {
        AirEnemy airEnemy = m_airPool[0];
        m_airPool.RemoveAt(0);
        airEnemy.transform.parent = parent;

        return airEnemy;
    }

    public void PoolUnit(BaseEnemy enemyToPool)
    {
        enemyToPool.transform.position = m_poolPosition;
        if (enemyToPool is AirEnemy)
        {
            m_airPool.Add((AirEnemy)enemyToPool);
        }
        else if (enemyToPool is GroundEnemy)
        {
            m_groundPool.Add((GroundEnemy)enemyToPool);
        }
        else
        {
            Destroy(enemyToPool.gameObject);
        }
    }
    #endregion

    private bool IsPreviousWavesDestroyed()
    {
        bool allDestroyed = true;

        foreach (EnemiesWave wave in m_wavesSpawned)
        {
            allDestroyed = wave.IsWaveDestroyed();
            if (!allDestroyed) break;
        }

        return allDestroyed;
    }



    protected IEnumerator ManageWaves()
    {
        int currentWaveID = 0;
        WaveObject currentWaveObject = m_enemiesWaves[currentWaveID];

        yield return new WaitForSeconds(m_timeBeforeFirstWave);
        while (currentWaveObject)
        {
            Transform currentWaveTransform = new GameObject("Wave" + currentWaveID).transform;
            EnemiesWave currentWave = currentWaveTransform.gameObject.AddComponent<EnemiesWave>();
            m_wavesSpawned.Add(currentWave);

            bool waveSurvey = currentWaveObject.nextWaveWait || currentWaveObject == m_enemiesWaves[m_enemiesWaves.Count - 1];

            foreach (SpawnObject spawn in currentWaveObject.Spawns)
            {
                BaseEnemy newEnemy;

                if (spawn.Unit is GroundEnemy)
                {
                    newEnemy = (IsThereUnusedGroundEnemy()) ? GetUnusedGroundEnemy(currentWaveTransform) : Instantiate(spawn.Unit, spawn.SpawnPosition, Quaternion.Euler(spawn.SpawnRotation), currentWaveTransform);
                }
                else if (spawn.Unit is AirEnemy)
                {
                    newEnemy = (IsThereUnusedAirEnemy()) ? GetUnusedAirEnemy(currentWaveTransform) : Instantiate(spawn.Unit, spawn.SpawnPosition, Quaternion.Euler(spawn.SpawnRotation), currentWaveTransform);
                }
                else
                {
                    newEnemy = Instantiate(spawn.Unit, spawn.SpawnPosition, Quaternion.Euler(spawn.SpawnRotation), currentWaveTransform);
                }

                currentWave.AddEnemy(newEnemy);
                newEnemy.ResetUnit(spawn.SpawnPosition, spawn.AttackPosition, m_player.m_targetPoint);
            }

            if (waveSurvey)
            {
                while (!IsPreviousWavesDestroyed())
                {
                    yield return null;
                }
            }

            yield return new WaitForSeconds(currentWaveObject.timeBeforeNextWave);
            currentWaveObject = m_enemiesWaves[++currentWaveID];
        }


    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        if (m_showSpawnsInEditor)
        {
            foreach (WaveObject enemyWave in m_enemiesWaves)
            {
                foreach (SpawnObject spawn in enemyWave.Spawns)
                {
                    Mesh mesh;
                    if (spawn.Unit)
                    {
                        mesh = spawn.Unit.GetComponentsInChildren<MeshFilter>()[0].sharedMesh;
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireMesh(mesh, spawn.SpawnPosition, Quaternion.Euler(spawn.SpawnRotation));
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireMesh(mesh, spawn.AttackPosition, Quaternion.Euler(spawn.SpawnRotation));
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireSphere(spawn.SpawnPosition, 1f);
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireSphere(spawn.AttackPosition, 1f);
                    }
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(spawn.SpawnPosition + new Vector3(0.0f, 1.0f, 0.0f), spawn.AttackPosition + new Vector3(0.0f, 1.0f, 0.0f));
                }
            }
        }
    }
}