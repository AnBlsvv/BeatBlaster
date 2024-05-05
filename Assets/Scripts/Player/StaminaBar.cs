using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Image fillArea;
    private float maxStamina;

    public void SetMaxStamina(int stamina)
    {
        fillArea.fillAmount = stamina;
        maxStamina = stamina;
    }

    public void SetStamina(int stamina)
    {
        fillArea.fillAmount = stamina / maxStamina;
    }
}
