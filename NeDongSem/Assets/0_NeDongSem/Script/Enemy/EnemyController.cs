using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    None,
    Moveable,//움직일 수 있는 상태
    Move,//움직이는 상태
    Arrival,//목표 지점에 도착한 상태
}

public class EnemyController : MonoBehaviour
{
    private bool m_isEnemyMoveStart = false;//적이 활성화 되었는가
    private float m_elapsedTime = 0.0f;
    private float m_delayTime = 0.0f;
    private float m_updateDelayTime = 2.0f; //3초 동안 Update 함수 지연
    private List<GameObject> m_enemyTileList = new List<GameObject>();
    [SerializeField] private EnemyState m_enemyState = EnemyState.Moveable;
    private int m_enemyTargetPosNumber = 1;//MapMng.Instance.EnemyTileList 인덱스 값
    private Vector3 m_v3EnemyTargetPos;//적이 도착할 목표값
    private float m_enemySpeed = 0;

    void Start()
    {
        
    }

    void Update()
    {
        //Eneemy 기능 동작
        if (m_isEnemyMoveStart)
        {
            //적이 맵을 따라서 이동
            EnemyMove();
        }

        //m_updateDelayTime동안 Enemy를 동작하지 않는다.
        //MapMng.cs의 m_EnemyTileList를 인지하기 위함이다.
        else if (!m_isEnemyMoveStart)
        {
            if (m_enemySpeed == 0)
            {
                if (InfoMng.Instance.EnemyInfoDictionary != null)
                {
                    m_enemySpeed = int.Parse(InfoMng.Instance.Get_EnemyInfo("1", "Speed"));
                }
            }
            
            //얕은 복사
            //적이 맵을 인식
            m_enemyTileList = MapMng.Instance.EnemyTileList;
            //GoogleSheetMng.cs의 Coroutine이 끝나기 전에 Update 이벤트 함수가 지속적으로 호출되어 m_enemyTitleList.Count가 0이 되는 현상을 방지하기 위한 예외 처리
            try
            {
                transform.position = m_enemyTileList[0].transform.position;
            }
            catch (System.ArgumentOutOfRangeException)
            {

            }
            catch (System.NullReferenceException)
            {

            }

            if (UpdateDelayTime(m_updateDelayTime)) 
            {
                m_isEnemyMoveStart = true;
            }
        }
    }

    private bool UpdateDelayTime(float _time)
    {
        m_elapsedTime += Time.deltaTime / _time;
        m_elapsedTime = Mathf.Clamp01(m_elapsedTime);
        m_delayTime = Mathf.Lerp(0, 1, m_elapsedTime);
        if (m_delayTime >= 1.0f)
        {
            m_elapsedTime = 0.0f;
            m_delayTime = 0.0f;
            return true;
        }
        return false;
    }

    private void EnemyMove()
    {
        switch (m_enemyState)
        {
            case EnemyState.Moveable:
                {
                    m_v3EnemyTargetPos = new Vector3(m_enemyTileList[m_enemyTargetPosNumber].transform.position.x,
                        m_enemyTileList[m_enemyTargetPosNumber].transform.position.y, m_enemyTileList[m_enemyTargetPosNumber].transform.position.z - 0.001f);
                    m_enemyState = EnemyState.Move;
                    break;
                }
            case EnemyState.Move:
                {
                    transform.position = Vector3.MoveTowards(transform.position, m_v3EnemyTargetPos, m_enemySpeed * Time.deltaTime);
                    if (transform.position == m_v3EnemyTargetPos)
                    {
                        m_enemyState = EnemyState.Arrival;
                    }
                    
                    break;
                }
            case EnemyState.Arrival:
                {
                    m_enemyTargetPosNumber++;
                    if (m_enemyTargetPosNumber == m_enemyTileList.Count)
                    {
                        m_enemyState = EnemyState.None;
                    }
                    else
                    {
                        m_enemyState = EnemyState.Moveable;
                    }
                    break;
                }
            case EnemyState.None:
                {
                    break;
                }
        }
    }
}
