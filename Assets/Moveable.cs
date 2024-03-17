using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Moveable : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField]
    List<Transform> targets = new List<Transform>();  // ���� ���� Ÿ���� �����ϴ� List
    int currentTargetIndex = 0;  // ���� ���õ� Ÿ���� �ε���
    bool isMoving = false;  // �̵� ������ ���θ� ��Ÿ���� �÷���

    private void Awake()
    {
        // NavMeshAgent ������Ʈ ��������
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // ���콺 ���� ��ư�� Ŭ���Ǹ�
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            // ��� ��ǥ ������ ���������� �̵��ϴ� �޼��� ȣ��
            StartCoroutine(MoveToTargets());
        }
    }

    IEnumerator MoveToTargets()
    {
        // �̵� �� �÷��� ����
        isMoving = true;

        // ��� ��ǥ ������ ���������� �̵�
        for (int i = 0; i < targets.Count; i++)
        {
            Transform currentTarget = targets[i];

            if (currentTarget != null)
            {
                // NavMeshAgent�� ����Ͽ� ���� Ÿ���� ��ġ�� �̵�
                agent.SetDestination(currentTarget.position);

                // �̵��� �Ϸ�� ������ ���
                yield return new WaitUntil(() => agent.remainingDistance < 0.1f);
            }
        }

        // �̵� �Ϸ� �� �̵� �� �÷��� ����
        isMoving = false;
    }
}
