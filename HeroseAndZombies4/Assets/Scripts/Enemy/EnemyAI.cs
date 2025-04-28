using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum AIState { Patrol, Chase, Attack }
    
    [Header("Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 5f;

    public float MoveSpeed => moveSpeed;
    public AIState CurrentState => currentState;
    
    private AIState currentState = AIState.Patrol;
    private NavMeshAgent agent;
    private Enemy enemy;
    private int currentPatrolIndex;
    private Transform player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        InitializeAgent();
    }

    private void InitializeAgent()
    {
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
        agent.stoppingDistance = attackRange * 0.9f;
    }

    private void Update()
    {
        UpdateStateMachine();
        FaceTargetWhenAttacking();
    }

    private void UpdateStateMachine()
    {
        switch(currentState)
        {
            case AIState.Patrol: UpdatePatrol(); break;
            case AIState.Chase: UpdateChase(); break;
            case AIState.Attack: UpdateAttack(); break;
        }
    }

    private void UpdatePatrol()
    {
        if (patrolPoints.Length == 0) 
        {
            currentState = AIState.Chase;
            return;
        }
        
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        
        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 1f)
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

        if (PlayerInRange(detectionRange))
            currentState = AIState.Chase;
    }

    private void UpdateChase()
    {
        agent.SetDestination(player.position);
        
        if (PlayerInRange(attackRange))
            currentState = AIState.Attack;
        else if (!PlayerInRange(detectionRange * 1.5f))
            currentState = AIState.Patrol;
    }

    private void UpdateAttack()
    {
        if (!PlayerInRange(attackRange * 1.2f))
        {
            currentState = AIState.Chase;
        }
        else
        {
            enemy.AttackPlayer();
        }
    }

    private void FaceTargetWhenAttacking()
    {
        if (currentState == AIState.Attack)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private bool PlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.position) <= range;
    }

    public void SetSpeed(float newSpeed)
    {
        agent.speed = newSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}