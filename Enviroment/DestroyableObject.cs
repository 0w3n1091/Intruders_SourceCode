using System;
using UnityEngine;

public class DestroyableObject : MonoBehaviour, IAttackable
{
    public int durability = 3;

    public Action OnDestroy
    {
        get => m_onDestroy;
        set => m_onDestroy = value;
    }

    public GameObject GameObject => throw new NotImplementedException();

    private Action m_onDestroy;
    public void ReceveAttack(int damage)
    {
        durability -= 1;
        if (durability <= 0)
        {
            Destroy(gameObject);
            OnDestroy?.Invoke();
        }
    }
}