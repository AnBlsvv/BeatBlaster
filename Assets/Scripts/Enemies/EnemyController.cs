using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public static EnemyController _ECInstance;

    Transform target;
    NavMeshAgent agent;
    Animator _animator;
    PlayerManager playerManager;
    CharacterStats myStats;
    PlayerController playerController;
    PlayerStats targetStats;
    Rigidbody rb;

    public bool isWaiting;
    public bool isKnockBack;

    void Awake()
    {
        _ECInstance = this;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        playerManager = PlayerManager._PMInstance;
        target = playerManager.player.transform;
        isWaiting = false;

        myStats = GetComponent<CharacterStats>();
        playerController = PlayerController._PCInstance;

        targetStats = playerManager.player.GetComponent<PlayerStats>();

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(target != null)
        {
            if(!isWaiting)
            {
                Pursuit(); 
            }
            else
            {
                NotPursuit();
            }
        } 
    }

    private void FixedUpdate() 
    {
        if(isKnockBack)
        {
            StartCoroutine(KnockBack());
            isKnockBack = false;
        }
    }

    public void Pursuit()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        agent.SetDestination(target.position);
        if(distance <= agent.stoppingDistance)
        {
            if(targetStats != null)
            {
                _animator.SetBool("isWalk", false);
                _animator.SetBool("isAttack", true);
            }
        }
        else
        {
            _animator.SetBool("isWalk", true);
            _animator.SetBool("isAttack", false);
        }
        FaceTarget();
    }

    public void AttackEvent()
    {
        int damage = myStats.attackDamage;
        if(playerController.isRolling)
        {
            damage = 0;
        }
        targetStats.TakeDamage(damage);
    }

    public void NotPursuit()
    {
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isAttack", false);
        FaceTarget();
    }

    private IEnumerator KnockBack()
    {
        agent.enabled = false;
        isWaiting = true;
        rb.isKinematic = false;
        Vector3 direction =  transform.position - target.position;
        rb.AddForce(direction.normalized * 30f, ForceMode.Impulse);
        yield return new WaitForSeconds(2f);
        agent.enabled = true;
        rb.isKinematic = true;
        isWaiting = false;
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void DieAnimation()
    {
        isWaiting = true;
        _animator.SetTrigger("Die");
    }
}
