using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Component : Unit
{
    private NavMeshAgent agent; // NavMeshAgent�� ����ϱ� ���� ����
    public int mana; // ������ ��Ÿ���� ����
    public int unitAmount; // ���� ���� ��Ÿ���� ����
    public float movementSpeed; // �̵� �ӵ��� ��Ÿ���� ����
    public float sightRange; // �þ� ������ ��Ÿ���� ����
    public GameObject rightPath; // ������ ��θ� ��Ÿ���� ����
    public GameObject leftPath; // ���� ��θ� ��Ÿ���� ����
    private Animator anim; // Animator ������Ʈ�� ����ϱ� ���� ����
    public float rotationSpeed = 5f; // ȸ�� �ӵ��� ��Ÿ���� ����
    public AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Animator ������Ʈ�� Awake �������� ������
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent ������Ʈ�� Start �������� ������
    }

    private void Update()
    {
        FindTarget(); // ����� ã�� �޼��� ȣ��
        Move(); // �̵��� ó���ϴ� �޼��� ȣ��
        FaceTarget(); // ����� ���� ȸ���ϴ� �޼��� ȣ��
    }

    private void FaceTarget()
    {
        if (currentTarget != null)
        {
            // �������� ���� ���͸� ����ϰ�, ĳ���Ͱ� �ٸ��� ������ ����
            Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
            directionToTarget.y = 0f;

            // ����� ���� ĳ���͸� ȸ����Ŵ
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void Move()
    {
        if (anim != null)
        {
            anim.SetBool("run", true); // "run" �ִϸ��̼� ���¸� Ȱ��ȭ
        }

        // ����� �ִ� ���
        if (currentTarget != null)
        {
            Vector3 closestPoint = currentTarget.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            float distanceToTarget = Vector3.Distance(transform.position, closestPoint);

            // ����� ���� ������ ������ ��� ������ �̵�
            if (distanceToTarget > attackRange)
            {
                Vector3 targetDirection = closestPoint - transform.position;
                targetDirection.y = 0f;
                targetDirection.Normalize();
                agent.isStopped = false;
                agent.SetDestination(closestPoint);
            }
            // ����� ���� ������ �������� �����ϰ� ����
            else
            {
                if (anim != null)
                {
                    anim.SetBool("run", false); // "run" �ִϸ��̼� ���¸� ��Ȱ��ȭ
                    anim.SetBool("attack", true); // "attack" �ִϸ��̼� ���¸� Ȱ��ȭ
                }
                agent.isStopped = true;
                if (attackCooldown <= 0f)
                {
                    Attack(); // ���� ����
                    audioSource.Play();
                    attackCooldown = attackSpeed; // ���� ������ ���� (��ٿ�)
                }
                // ��ٿ� ������Ʈ
                if (attackCooldown > 0f)
                {
                    attackCooldown -= Time.deltaTime;
                }
            }
        }
        // ����� ������ ���� ��ǥ��� ������ �̵�
        else
        {
            agent.isStopped = false;
            if (anim != null)
            {
                anim.SetBool("run", true); // "run" �ִϸ��̼� ���¸� Ȱ��ȭ
                anim.SetBool("attack", false); // "attack" �ִϸ��̼� ���¸� ��Ȱ��ȭ
            }

            // x ��ġ�� ���� ���� ����� ���(right �Ǵ� left)�� ����
            GameObject closestPath = transform.position.x >= 0f ? rightPath : leftPath;
            Vector3 pathPosition;

            // ���� ���� ������ ��� ��ġ�� ������
            if (team == Team.Blue)
            {
                pathPosition = closestPath.transform.GetChild(1).position;
            }
            else
            {
                pathPosition = closestPath.transform.GetChild(0).position;
            }

            // �������� ����
            agent.SetDestination(pathPosition);
        }
    }

    public void FindTarget()
    {
        // �þ� ���� ���� ��� �ݶ��̴��� ������
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange);

        // ������ ������ ������ ����Ʈ
        List<Transform> potentialTargets = new List<Transform>();

        // �� �ݶ��̴��� ���� �ݺ�
        foreach (Collider collider in colliders)
        {
            // ���� �ݺ��ϴ� �ݶ��̴��κ��� Unit ������Ʈ�� �õ��� ����
            if (collider.TryGetComponent(out Unit enemy))
            {
                // ����� ���� ������Ʈ�� ���� �ٸ��� ������Ʈ�� ���� ������ ����� ���
                if (team != enemy.team && targets.Contains(enemy.unitType))
                {
                    // ������ ������� �Ǵ��ϰ� ����Ʈ�� �߰�
                    potentialTargets.Add(collider.transform);
                }
            }
        }

        // ������ ����� �ϳ� �̻� ���� ��
        if (potentialTargets.Count > 0)
        {
            // ������ ���� �߿��� �����ϰ� ����� ����
            int randomIndex = Random.Range(0, potentialTargets.Count);
            currentTarget = potentialTargets[randomIndex];
        }
        else
        {
            // ������ ����� ������ ���� ����� null�� ����
            currentTarget = null;
        }
    }




    private void OnDrawGizmosSelected()
    {
        // �ð������� Gizmos�� �׷� �þ� ������ ���� ������ ��Ÿ��

        // ����, �þ� ������ ������� ǥ��
        Gizmos.color = Color.green;

        // ���� ���� ������Ʈ�� ��ġ���� �þ� ������ŭ�� �ݰ��� ������ ���� �׸��ϴ�.
        // �̴� �ش� ������ �þ߸� ��Ÿ����, �� ������ �þ� ���� �ȿ� ���� ��� ������ �� �ֽ��ϴ�.
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // ��������, ���� ������ ���������� ǥ��
        Gizmos.color = Color.red;

        // ���� ���� ������Ʈ�� ��ġ���� ���� ������ŭ�� �ݰ��� ������ ���� �׸��ϴ�.
        // �̴� �ش� ������ �� ������ ������ �� �ִ� ������ �ð������� ��Ÿ���ϴ�.
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
