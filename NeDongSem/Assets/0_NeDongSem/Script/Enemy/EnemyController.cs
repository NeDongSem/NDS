using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    None,
    Moveable,//������ �� �ִ� ����
    Move,//�����̴� ����
    Arrival,//��ǥ ������ ������ ����
}

public class EnemyController : MonoBehaviour
{
    private bool m_isEnemyMoveStart = false;//���� Ȱ��ȭ �Ǿ��°�
    private float m_elapsedTime = 0.0f;
    private float m_delayTime = 0.0f;
    private float m_updateDelayTime = 2.0f; //3�� ���� Update �Լ� ����
    private List<GameObject> m_enemyTileList = new List<GameObject>();
    [SerializeField] private EnemyState m_enemyState = EnemyState.Moveable;
    private int m_enemyTargetPosNumber = 1;//MapMng.Instance.EnemyTileList �ε��� ��
    private Vector3 m_v3EnemyTargetPos;//���� ������ ��ǥ��
    private float m_enemySpeed = 0;

    void Start()
    {
        
    }

    void Update()
    {
        //Eneemy ��� ����
        if (m_isEnemyMoveStart)
        {
            //���� ���� ���� �̵�
            EnemyMove();
        }

        //m_updateDelayTime���� Enemy�� �������� �ʴ´�.
        //MapMng.cs�� m_EnemyTileList�� �����ϱ� �����̴�.
        else if (!m_isEnemyMoveStart)
        {
            if (m_enemySpeed == 0)
            {
                if (InfoMng.Instance.EnemyInfoDictionary != null)
                {
                    m_enemySpeed = int.Parse(InfoMng.Instance.Get_EnemyInfo("1", "Speed"));
                }
            }
            
            //���� ����
            //���� ���� �ν�
            m_enemyTileList = MapMng.Instance.EnemyTileList;
            //GoogleSheetMng.cs�� Coroutine�� ������ ���� Update �̺�Ʈ �Լ��� ���������� ȣ��Ǿ� m_enemyTitleList.Count�� 0�� �Ǵ� ������ �����ϱ� ���� ���� ó��
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
