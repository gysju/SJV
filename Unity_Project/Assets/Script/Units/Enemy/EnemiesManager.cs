using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public bool m_showSpawnsInEditor;
    [Range(0, 100)]
    public int m_waveToShow;
    public BaseMecha m_player;

    public Vector3 m_poolPosition = Vector3.zero;
    protected List<GroundEnemy> m_groundPool = new List<GroundEnemy>();
    protected List<AirEnemy> m_airPool = new List<AirEnemy>();
    protected List<MobileMineEnemy> m_minePool = new List<MobileMineEnemy>();

    public List<WaveObject> m_enemiesWaves;
    protected List<EnemiesWave> m_wavesSpawned = new List<EnemiesWave>();

    public float m_timeBeforeFirstWave = 5f;
    public float m_timeBeforeEndZA = 5f;

    protected ZAManager m_zaManager;

    public List<BaseEnemy> m_activeEnemies = new List<BaseEnemy>();

    public void StartWaves()
    {
        m_zaManager = FindObjectOfType<ZAManager>();
        m_player = FindObjectOfType<BaseMecha>();
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

    public bool IsThereUnusedMobileMineEnemy()
    {
        return (m_minePool.Count > 0);
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

    public MobileMineEnemy GetUnusedMobileMine(Transform parent)
    {
        MobileMineEnemy mobileMineEnemy = m_minePool[0];
        m_minePool.RemoveAt(0);
        mobileMineEnemy.transform.parent = parent;
        return mobileMineEnemy;
    }

    public void PoolUnit(BaseEnemy enemyToPool)
    {
        m_activeEnemies.Remove(enemyToPool);
        enemyToPool.transform.position = m_poolPosition;
        if (enemyToPool is AirEnemy)
        {
            m_airPool.Add((AirEnemy)enemyToPool);
        }
        else if (enemyToPool is MobileMineEnemy)
        {
            m_minePool.Add((MobileMineEnemy)enemyToPool);
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

        yield return new WaitForSeconds(m_timeBeforeFirstWave);
        while (currentWaveID < m_enemiesWaves.Count)
        {
            WaveObject currentWaveObject = m_enemiesWaves[currentWaveID];
            Transform currentWaveTransform = new GameObject("Wave" + currentWaveID).transform;
            EnemiesWave currentWave = currentWaveTransform.gameObject.AddComponent<EnemiesWave>();
            m_wavesSpawned.Add(currentWave);

            bool waveSurvey = currentWaveObject.nextWaveWait || currentWaveObject == m_enemiesWaves[m_enemiesWaves.Count - 1];

            foreach (SpawnObject spawn in currentWaveObject.Spawns)
            {
                BaseEnemy newEnemy;

                if (spawn.Unit is MobileMineEnemy)
                {
                    newEnemy = (IsThereUnusedMobileMineEnemy()) ? GetUnusedMobileMine(currentWaveTransform) : Instantiate(spawn.Unit, spawn.SpawnPosition, Quaternion.Euler(spawn.SpawnRotation), currentWaveTransform);
                }
                else if (spawn.Unit is GroundEnemy)
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

                m_activeEnemies.Add(newEnemy);
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
            currentWaveID++;
        }

        m_zaManager.MissionAccomplished();
        yield return new WaitForSeconds(m_timeBeforeEndZA);
        m_zaManager.BackToMainMenu();
    }

    void OnDrawGizmos()
    {
        if (m_showSpawnsInEditor)
        {
            if (m_waveToShow == 0)
            {
                foreach (WaveObject enemyWave in m_enemiesWaves)
                {
                    foreach (SpawnObject spawn in enemyWave.Spawns)
                    {
                        Mesh mesh;
                        if (spawn.Unit)
                        {
                            SkinnedMeshRenderer skinnedMesh = spawn.Unit.GetComponentInChildren<SkinnedMeshRenderer>();
                            if (skinnedMesh == null)
                                mesh = spawn.Unit.GetComponentInChildren<MeshFilter>().sharedMesh;
                            else
                                mesh = skinnedMesh.sharedMesh;
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
            else
            {
                if (m_waveToShow <= m_enemiesWaves.Count)
                foreach (SpawnObject spawn in m_enemiesWaves[m_waveToShow - 1].Spawns)
                {
                    Mesh mesh;
                    if (spawn.Unit)
                    {
                        SkinnedMeshRenderer skinnedMesh = spawn.Unit.GetComponentInChildren<SkinnedMeshRenderer>();
                        if (skinnedMesh == null)
                            mesh = spawn.Unit.GetComponentInChildren<MeshFilter>().sharedMesh;
                        else
                            mesh = skinnedMesh.sharedMesh;
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