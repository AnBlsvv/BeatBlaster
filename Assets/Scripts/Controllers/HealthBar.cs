using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Camera cam;
    public Image fillArea;
    private float maxHealth;

    private void Start() {
        GameObject cameraObject = GameObject.FindWithTag("MainCamera");
        cam = cameraObject.GetComponent<Camera>();
    }

    private void Update() {
        if(this.gameObject.CompareTag("EnemyHealthBar"))
        {
            transform.rotation = cam.transform.rotation;
        }
    }

    public void SetMaxHealth(float health)
    {
        fillArea.fillAmount = health;
        maxHealth = health;
    }

    public void SetHealth(float health)
    { 
        fillArea.fillAmount = health / maxHealth;
    }
}
