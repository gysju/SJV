using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyWave
    {
        [System.Serializable]
        public struct Spawn
        {
            public BaseEnemy Enemy;
            public Vector3 SpawnPosition;
            public Vector3 SpawnRotation;
            public Vector3 AttackPosition;
        }

        public float timeBeforeNextWave;
        public bool waitPreviousWave;
        public List<Spawn> Spawns;
    }

    public bool m_showSpawnsInEditor;
    public List <EnemyWave> m_enemiesWaves;

    void Start ()
    {

    }
	

	void Update ()
    {
		
	}

    void OnDrawGizmos()
    {
        if(m_showSpawnsInEditor)
        {
            foreach (EnemyWave enemyWave in m_enemiesWaves)
            {
                foreach (EnemyWave.Spawn spawn in enemyWave.Spawns)
                {
                    Mesh mesh;
                    if(spawn.Enemy)
                    {
                        mesh = spawn.Enemy.GetComponentsInChildren<MeshFilter>()[0].sharedMesh;
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
                }
            }
        }
    }
}
