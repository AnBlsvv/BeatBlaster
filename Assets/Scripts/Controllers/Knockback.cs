using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Knockback : MonoBehaviour
{
    public float explosionRadius;
    public float explosionForce;

    [SerializeField] int timerValue;
    [SerializeField] Image timerCircle;
    [SerializeField] TMP_Text timerText;

    private int timerInt;
    private float timerFloat;
    private bool isCountingDown;

    private void Start() {
        timerInt = 0;
        timerFloat = 0f;
        isCountingDown = false;
        timerText.text = timerValue.ToString();
        timerCircle.enabled = false;
        timerText.enabled = false;
    }

    private void StartCountdown()
    {
        if (isCountingDown) return;
        timerInt = timerValue;
        timerFloat = (float)timerValue;
        timerCircle.enabled = true;
        timerText.enabled = true;
        StartCoroutine(Countdown());
    }

    private void StopCountdown()
    {
        timerCircle.enabled = false;
        timerText.enabled = false;
        isCountingDown = false;
        StopCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        isCountingDown = true;
        while (timerFloat > 0)
        {
            timerFloat -= Time.deltaTime;
            timerCircle.fillAmount = timerFloat / timerValue;

            if(timerInt > (int)timerFloat)
            {
                timerText.text = timerInt.ToString();
                timerInt -= 1;
            }
            yield return null;
        }
        StopCountdown();
    }

    public void TriggerExplosion()
    {
        if(timerInt == 0)
        {
            StartCountdown();
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();
                    enemyController.isKnockBack = true;
                }
            }
        }
    }
}
