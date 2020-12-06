using UnityEngine;
using UnityEngine.Events;
using Pathfinding;


public enum EnemyState
{

    Idle,
    Patrolling,
    Chasing,
    Attacking
}


[RequireComponent(typeof(IAstarAI))]
public class EnemyBehaviour : MonoBehaviour
{



    [SerializeField] Transform enemyGFX;
    [SerializeField] Transform visionCone;
    [SerializeField] protected ObjectPooler effectPooler;

    protected bool isDead = false;
    protected Animator enemyAnim;
    protected IAstarAI agent;
    protected Health health;
    protected EnemyState currentState;
    public Vector3[] patrolPoints;
    /// Time in seconds to wait at each target
    public float delay = 0;
    ///Current target index
    int index = 0;
    float switchTime = float.PositiveInfinity;


    [HideInInspector] public bool playerInView = false;
    [HideInInspector] public Vector2 playerLastPosition;
    [HideInInspector] public UnityEvent OnDeath;

    void OnEnable()
    {
        Restore();
    }

    void Awake()
    {

        if (OnDeath == null)
            OnDeath = new UnityEvent();

        enemyAnim = enemyGFX.GetComponent<Animator>();
        health = GetComponent<Health>();
        agent = GetComponent<IAstarAI>();


    }

    protected void Restore()
    {
        isDead = false;
        health.RestoreHealth();
        health.OnHealtedChange.Invoke();
        agent.isStopped = false;
        switchTime = float.PositiveInfinity;
        currentState = EnemyState.Idle;
    }

    protected void Patrol(float speed)
    {

        if (patrolPoints.Length == 0) return;

        bool search = false;
        currentState = EnemyState.Patrolling;
        agent.maxSpeed = speed;

        //If switching time is reverted to 0 add a new switching time
        if (agent.reachedEndOfPath && !agent.pathPending && float.IsPositiveInfinity(switchTime))
        {
            switchTime = Time.time + delay;
        }

        if (Time.time >= switchTime)
        {
            index = index + 1;
            search = true;
            // Reverted switchTime back to 0 
            // to signal that npc is ready to move to next point
            switchTime = float.PositiveInfinity;
        }

        index = index % patrolPoints.Length;
        agent.destination = patrolPoints[index];

        if (search) agent.SearchPath();

    }

    protected void Chase(float speed)
    {

        if (playerLastPosition == null)
            return;
        agent.maxSpeed = speed;
        agent.destination = playerLastPosition;
        currentState = EnemyState.Chasing;

        if (!agent.reachedDestination)
        {
            agent.SearchPath();

        }

    }

    public virtual void UpdateGFX()
    {
        if (agent.velocity.x >= 0.01f)
            enemyGFX.localScale = new Vector3(-1, 1, 1);

        else if (agent.velocity.x <= -0.01f)
            enemyGFX.localScale = new Vector3(1, 1, 1);

        enemyAnim.SetFloat("Horizontal", agent.velocity.x);
        enemyAnim.SetFloat("Vertical", agent.velocity.y);


    }


    
    public virtual void Death()
    {
        isDead = true;
    }



}