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
        // �÷��̾ �ٰ� �ִ� �߿�
        // ���׹̳� ����
        // ���׹̳� == 0�̸� ���� ����

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

    public void Adrenaline(float amount) // �Ƶ巹���� ������ ��� ��
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
        Debug.Log("�÷��̾ �׾���.");
    }

}
