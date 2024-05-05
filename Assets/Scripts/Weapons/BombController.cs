using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BombController : MonoBehaviour
{
    public static BombController _BombInstance;

    PlayerController playerController;
    public GameObject bombPrefab;
    public float throwForce = 20f;
    public Transform parentForBomb;
    public int bombAmount;
    public TMP_Text bombAmountTxt;
    public float explosionRadius;
    public int damage;

    public Button bombButton;

    private void Awake() {
        if(_BombInstance != null && _BombInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _BombInstance = this;
        } 
    }

    private void Start()
    {
        playerController = PlayerController._PCInstance;
        bombAmountTxt.text = bombAmount.ToString();
    }

    public void StartAnimation()
    {
        if(bombAmount > 0 && playerController.canThrow)
        {
            bombAmount--;
            playerController.ThrowItem();
        }
    }

    public void ThrowBomb()
    {
        bombAmountTxt.text = bombAmount.ToString();

        Vector3 targetDirection = playerController.GetLookDirection();
        targetDirection.y = 0f;
        targetDirection.Normalize();

        Vector3 throwPoint = parentForBomb.position + targetDirection;

        GameObject bombObject = Instantiate(bombPrefab, throwPoint, Quaternion.identity);
        Rigidbody bombRigidbody = bombObject.GetComponent<Rigidbody>();

        bombRigidbody.AddForce(targetDirection * throwForce, ForceMode.Impulse);

        bombObject.GetComponent<Bomb>().ExplodeBomb();
    }
}