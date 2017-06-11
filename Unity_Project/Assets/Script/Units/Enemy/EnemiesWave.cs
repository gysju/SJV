using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesWave : MonoBehaviour
{
    protected List<BaseEnemy> m_enemies = new List<BaseEnemy>();

    protected bool m_waveDestroyed = false;

    public void AddEnemy (BaseEnemy newEnemy)
	{
        m_enemies.Add(newEnemy);
	}
	
    public bool IsWaveDestroyed()
    {
        return m_waveDestroyed || m_enemies.Count == 0;
    }

    protected bool CheckWaveStatus()
    {
        bool allDestroyed = true;

        foreach (BaseEnemy enemy in m_enemies)
        {
            allDestroyed = enemy.IsDestroyed();
            if (!allDestroyed) break;
        }

        return allDestroyed;
    }

	void Update ()
	{
		if (!m_waveDestroyed && m_enemies.Count > 0)
        {
            m_waveDestroyed = CheckWaveStatus();
        }
	}
}
