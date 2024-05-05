using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController _PCInstance;

    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private FloatingJoystick _joystickAim;
    public Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private FireScript fireScript;
    AnimatorStateInfo stateInfo;
    AnimatorStateInfo stateInfoLayerTwo;
    RunButton runButton;
    ItemController itemController;

    [Header("Rolling Parameters")]
    public bool isRolling = false;
    public int takesStaminaCount = 20;
    private float lastRollTime = 0f;
    private float timeBetweenRolls = 2f;

    [Header("Stamina Parameters")]
    private float staminaTimer = 0f;
    private float staminaReduceInterval = 0.5f;
    private float staminaRecoveryInterval = 0.3f;
    private float staminaRecoveryTimer = 0f;
    
    public float angleThreshold = 45f; // Threshold angle for direction change
    [HideInInspector] public bool canThrow = true;
    [HideInInspector] public bool canAttack = true;

    void Awake()
    {
        if(_PCInstance != null && _PCInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _PCInstance = this;
        }
    }
 
    private void Start()
    {
        //_rigidbody = GetComponent<Rigidbody>();
        //_animator = GetComponentInChildren<Animator>();
        //agent = GetComponent<NavMeshAgent>();
        //playerStats = GetComponent<PlayerStats>();
        runButton = RunButton._RunBtnInstance;
        itemController = ItemController._ICInstance;
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    private void Update()
    {
        ChekingParameters();
        Move();
    }

    private void ChekingParameters()
    {
        stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        stateInfoLayerTwo = _animator.GetCurrentAnimatorStateInfo(1);
        if (stateInfo.IsName("Rolling") && stateInfo.normalizedTime >= 0.8f && isRolling)
        {
            isRolling = false;
            canAttack = true;
        }

        if (!canThrow && stateInfoLayerTwo.IsName("Throw") && stateInfoLayerTwo.normalizedTime >= 1f)
        {
            canThrow = true;
        }

        if (playerStats.currentStamina < playerStats.maxStamina)
        {
            if (staminaRecoveryTimer <= 0)
            {
                playerStats.StaminaRecovery();
                staminaRecoveryTimer = staminaRecoveryInterval;
            }
            else
            {
                staminaRecoveryTimer -= Time.deltaTime;
            }
        }
    }

    private void Move()
    {
        agent.speed = 16;
        if(_animator.GetBool("isRolling") || runButton.isPressedRun)
        {
            agent.speed *= 2;
        }
        Vector3 _moveVector = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        Vector3 _rotateVector = new Vector3(_joystickAim.Horizontal, 0, _joystickAim.Vertical);

        // get camera angle
        var cameraAngle = Camera.main.transform.eulerAngles.y;
        // transform the motion vector with the camera angle
        _moveVector = Quaternion.Euler(0, cameraAngle, 0) * _moveVector;
        _rotateVector = Quaternion.Euler(0, cameraAngle, 0) * _rotateVector;

        Vector2 inputDirectionAim = new Vector2(_joystickAim.Horizontal, _joystickAim.Vertical);
        Vector2 inputDirectionMove = new Vector2(_joystick.Horizontal, _joystick.Vertical);

        if((inputDirectionAim.x != 0 || inputDirectionAim.y != 0) && !playerStats.isDie)
        {
            Vector3 direction = Vector3.RotateTowards(transform.forward, _rotateVector, Time.deltaTime * 20f, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
            if(inputDirectionAim.x >= 0.7f || inputDirectionAim.y >= 0.7f || inputDirectionAim.x <= -0.7f || inputDirectionAim.y <= -0.7f)
            {
                fireScript.Fire();
            }
            else
            {
                fireScript.StopFire();
            }
        }
        else
        {
            fireScript.StopFire();
        }
        if((inputDirectionMove.x != 0 || inputDirectionMove.y != 0) && !playerStats.isDie)
        {
            agent.SetDestination(transform.position + _moveVector);

            var targetDirection = transform.position + _moveVector * agent.speed * 4f * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, agent.nextPosition, 0.3f);
            if(inputDirectionAim.x == 0 && inputDirectionAim.y == 0)
            {
                Vector3 direction = Vector3.RotateTowards(transform.forward, _moveVector, Time.deltaTime * 20f, 0.0f);
                transform.rotation = Quaternion.LookRotation(direction);
            }
            if(itemController.currentWeapon.CompareTag("Heavy"))
            {
                _animator.SetBool("isHeavyWeapon", true);
            }
            else
            {
                _animator.SetBool("isHeavyWeapon", false);
            }
            
            if(isRolling)
            {
                Rolling();
            }
            else if(runButton.isPressedRun)
            {
                if(playerStats.currentStamina > 0)
                {
                    _animator.SetBool("isWalking", false);
                    _animator.SetBool("isRunning", true);
                    staminaTimer += Time.deltaTime;
                    if (staminaTimer >= staminaReduceInterval)
                    {
                        playerStats.StaminaReduction(takesStaminaCount);
                        staminaTimer = 0f;
                        staminaRecoveryTimer = 4f;
                    }
                }
                else
                {
                    runButton.isPressedRun = false;
                    _animator.SetBool("isRunning", false);
                    _animator.SetBool("isWalking", true);
                }
            }
            else
            {
                runButton.isPressedRun = false;
                _animator.SetBool("isRunning", false);
                _animator.SetBool("isRolling", false);
                if(_joystickAim.Horizontal == 0 && _joystickAim.Vertical == 0)
                {
                    _animator.SetBool("isWalking", true);
                }
                else
                {
                    // Calculate the angle of the input direction relative to the x-axis
                    float angleRight = Mathf.Atan2(inputDirectionAim.y, inputDirectionAim.x) * Mathf.Rad2Deg;
                    float angleLeft = Mathf.Atan2(inputDirectionMove.y, inputDirectionMove.x) * Mathf.Rad2Deg;
                    
                    // Clamp angle to positive values between 0 and 360 degrees
                    if (angleRight < 0)
                    {
                        angleRight += 360;
                    }
                    if (angleLeft < 0)
                    {
                        angleLeft += 360;
                    }

                    // Determine the direction based on the angle
                    // Character goes right and left - Right Joystick
                    if (((angleRight >= 0 && angleRight <= angleThreshold) || (angleRight >= 360 - angleThreshold && angleRight <= 360)) || (angleRight >= 180 - angleThreshold && angleRight <= 180 + angleThreshold))
                    {
                        // Determine the direction based on the angle
                        if (((angleLeft >= 0 && angleLeft <= angleThreshold) || (angleLeft >= 360 - angleThreshold && angleLeft <= 360)) || (angleLeft >= 180 - angleThreshold && angleLeft <= 180 + angleThreshold))
                        {
                            // Character goes right or left
                            _animator.SetBool("isWalking", true);
                            _animator.SetBool("isWalkingRightLeft", false);
                        }
                        else if ((angleLeft >= 90 - angleThreshold && angleLeft <= 90 + angleThreshold) || (angleLeft >= 270 - angleThreshold && angleLeft <= 270 + angleThreshold))
                        {
                            // Character goes up and down
                            _animator.SetBool("isWalking", false);
                            _animator.SetBool("isWalkingRightLeft", true);
                        }
                        else
                        {
                            // Character stops moving
                            _animator.SetBool("isWalkingRightLeft", false);
                            _animator.SetBool("isWalking", false);
                        }
                    }
                    // Character goes up and down - Right Joystick
                    else if ((angleRight >= 90 - angleThreshold && angleRight <= 90 + angleThreshold) || (angleRight >= 270 - angleThreshold && angleRight <= 270 + angleThreshold))
                    {
                        // Determine the direction based on the angle
                        if (((angleLeft >= 0 && angleLeft <= angleThreshold) || (angleLeft >= 360 - angleThreshold && angleLeft <= 360)) || (angleLeft >= 180 - angleThreshold && angleLeft <= 180 + angleThreshold))
                        {
                            // Character goes right or left
                            _animator.SetBool("isWalkingRightLeft", true);
                            _animator.SetBool("isWalking", false);
                        }
                        else if ((angleLeft >= 90 - angleThreshold && angleLeft <= 90 + angleThreshold) || (angleLeft >= 270 - angleThreshold && angleLeft <= 270 + angleThreshold))
                        {
                            // Character goes up and down
                            _animator.SetBool("isWalkingRightLeft", false);
                            _animator.SetBool("isWalking", true);
                        }
                        else
                        {
                            // Character stops moving
                            _animator.SetBool("isWalkingRightLeft", false);
                            _animator.SetBool("isWalking", false);
                        }
                    }
                }
            }
        }
        /*else if(_joystick.Horizontal == 0 && _joystick.Vertical == 0)*/
        else if(inputDirectionMove.x == 0 && inputDirectionMove.y == 0)
        {
            if(isRolling && !_animator.GetBool("isRolling"))
            {
                isRolling = false;
            }
            if(!isRolling)
            {
                _animator.SetBool("isRolling", false);
            }
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isWalkingRightLeft", false);
            runButton.isPressedRun = false;
        }
    }

    public void CanRoll()
    {
        if(!isRolling && !itemController.currentWeapon.CompareTag("Heavy") && !stateInfoLayerTwo.IsName("ReloadWeapon") && !stateInfoLayerTwo.IsName("Shoot") 
            && !stateInfoLayerTwo.IsName("Attack"))
        {
            isRolling = true;
        }
    }

    public void Rolling()
    {
        if(playerStats.currentStamina > takesStaminaCount && Time.time - lastRollTime >= timeBetweenRolls)
        {
            playerStats.StaminaReduction(takesStaminaCount);
            staminaRecoveryTimer = 4f;

            lastRollTime = Time.time; // update time after the last rolling
            canAttack = false;
            _animator.SetBool("isRolling", true);
        }
    }

    public void AttackMeleeTrue()
    {
        if(playerStats.currentStamina > takesStaminaCount)
        {
            _animator.SetBool("isAttack", true);
            playerStats.StaminaReduction(takesStaminaCount);
            staminaRecoveryTimer = 4f;
        }
    }

    public void AttackMeleeFalse()
    {
        _animator.SetBool("isAttack", false);
    }

    public void AttackDistantTrue()
    {
        _animator.SetBool("isShoot", true);
    }
    
    public void AttackDistantFalse()
    {
        _animator.SetBool("isShoot", false);
    }

    public void ThrowItem()
    {
        _animator.SetTrigger("Throw");
        canThrow = false;
    }

    public void ReloadGun()
    {
        _animator.SetTrigger("Reload");
        canAttack = false;
        Invoke("SetCanAttack", 3f);
    }

    private void SetCanAttack()
    {
        canAttack = true;
    }

    public void DisableAllAnimations()
    {
        _animator.SetBool("isRunning", false); 
        _animator.SetBool("isRolling", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isAttack", false);
        _animator.SetBool("isShoot", false);
        _joystick.input = Vector2.zero;
        _joystick.handle.anchoredPosition = Vector2.zero;
    }

    public Vector3 GetLookDirection()
    {
        // Get the direction vector of the character's face
        Vector3 lookDirection = transform.forward;
        return lookDirection;
    }
}
