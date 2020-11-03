using UnityEngine;
using UnityEngine.Events;







public class EnemyBehaviour : MonoBehaviour
{



    [SerializeField] Transform enemyGFX;
    public UnityEvent OnDeath;
    public Transform deathSFX;
    protected bool isDead = false;
    protected Animator enemyAnim;
    protected AstarAgent aIAgent;

    void Awake()
    {

        if (OnDeath == null)
            OnDeath = new UnityEvent();

        enemyAnim = enemyGFX.GetComponent<Animator>();

    }



    public virtual void Death()
    {

        isDead = true;
    }

    public virtual void UpdateGFX()
    {
        if (aIAgent.velocity.x >= 0.01f)
            enemyGFX.localScale = new Vector3(-1, 1, 1);

        else if (aIAgent.velocity.x <= -0.01f)
            enemyGFX.localScale = new Vector3(1, 1, 1);

        enemyAnim.SetFloat("Horizontal", aIAgent.velocity.x);
        enemyAnim.SetFloat("Vertical", aIAgent.velocity.y);
    }
    void OnEnable()
    {
        isDead = false;
    }

}