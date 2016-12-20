using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
	public BattleManager Instance { get; private set;}

    public Player m_player;

    public Unit m_allyBaseCenter;
    public Unit m_enemyBaseCenter;

    public Vector3 m_poolPosition = Vector3.zero;

    private List<GameObject> m_unusedBullets = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_unusedTanks = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_unusedDrones = new List<GameObject>();

    void Start ()
    {
		if (Instance == null) 
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);
		
    }

    #region Pools
    public bool IsThereUnusedBullets()
    {
        return (m_unusedBullets.Count > 0);
    }

    public bool IsThereUnusedTanks()
    {
        return (m_unusedTanks.Count > 0);
    }

    public bool IsThereUnusedDrones()
    {
        return (m_unusedDrones.Count > 0);
    }

    public GameObject GetUnusedBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = m_unusedBullets[0];
        m_unusedBullets.RemoveAt(0);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        return bullet;
    }

    public GameObject GetUnusedTank(Vector3 position, Quaternion rotation)
    {
        GameObject tank = m_unusedTanks[0];
        m_unusedTanks.RemoveAt(0);
        tank.transform.position = position;
        tank.transform.rotation = rotation;
        tank.GetComponent<Unit>().ResetUnit();

        return tank;
    }

    public GameObject GetUnusedDrone(Vector3 position, Quaternion rotation)
    {
        GameObject drone = m_unusedDrones[0];
        m_unusedDrones.RemoveAt(0);
        drone.transform.position = position;
        drone.transform.rotation = rotation;
        drone.GetComponent<Unit>().ResetUnit();
        return drone;
    }

    public void PoolAmmo(GameObject ammoToPool)
    {
        ammoToPool.transform.position = m_poolPosition;
        m_unusedBullets.Add(ammoToPool);
    }

    public void PoolUnit(Unit unitToPool)
    {
        unitToPool.transform.position = m_poolPosition;
        if (unitToPool is AirUnit)
        {
            m_unusedDrones.Add(unitToPool.gameObject);
        }
        else if (unitToPool is HoverTank)
        {
            m_unusedTanks.Add(unitToPool.gameObject);
        }
        else
        {
            Destroy(unitToPool.gameObject);
        }
    }
    #endregion

    #region Gameplay End
    private bool VictoryCondition()
    {
        return (m_enemyBaseCenter.IsDestroyed());
    }

    private bool DefeatCondition()
    {
        return (m_allyBaseCenter.IsDestroyed() || m_player.IsDestroyed());
    }

    private void VictoryEvent()
    {
		CanvasManager.Get.eState_Menu = CanvasManager.EState_Menu.EState_Menu_Victory;
        StartCoroutine(BackTimer());
    }

    private void DefeatEvent()
    {
		CanvasManager.Get.eState_Menu = CanvasManager.EState_Menu.EState_Menu_Defeat;
        StartCoroutine(BackTimer());
    }

    IEnumerator BackTimer()
    {
        yield return new WaitForSeconds(2f);
        BackToMainMenu();
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
    #endregion

    void Update ()
    {
		switch (CanvasManager.Get.eState_Menu)
        {
			case CanvasManager.EState_Menu.EState_Menu_InGame:
                if (VictoryCondition())
                    VictoryEvent();
                else if (DefeatCondition())
                    DefeatEvent();
                break;
        }
    }
}
