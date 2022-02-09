using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image weaponDurability;
    public Image kidHealth;
    public Image playerHealth;

    /// <summary>
    /// Sets weaponDurability.fillAmount to given value
    /// </summary>
    /// <param name="aDurability"></param>
    public void SetWeaponDurability(float aDurability)
    {
        weaponDurability.fillAmount = aDurability / 8f;
    }

    /// <summary>
    /// Updates weaponDurability.fillAmount by given damage
    /// </summary>
    public void UpdateWeaponDurability(float aDamage)
    {
        weaponDurability.fillAmount -= aDamage * 0.125f;
    }

    /// <summary>
    /// Updates playerHealt.fillAmount by given damage
    /// </summary>
    public void UpdatePlayerHealth(float aDamage)
    {
        playerHealth.fillAmount -= aDamage * 0.2f;
    }

    /// <summary>
    /// Updates kidHealth.fillAmount by given damage
    /// </summary>
    public void UpdateKidHealth(float aDamage)
    {
        kidHealth.fillAmount -= aDamage * 0.2f;
    }
}
