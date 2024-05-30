using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerConditions : MonoBehaviour, IDamagable
{
    public UIConditions uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealDecay;
    public event Action onTakeDamage;

    void Update()
    {
        // 플레이어가 뛰고 있는 중에
        // 스테미나 감소
        // 스테미나 == 0이면 뛰지 못함

        stamina.Add(stamina.regenRate * Time.deltaTime);

        if(health.curValue == 0.0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Adrenaline(float amount) // 아드레날린 아이템 사용 시
    {
        stamina.Add(amount);
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if(stamina.curValue - amount < 0)
        {
            Debug.Log("false");
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }

    private void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }

}
