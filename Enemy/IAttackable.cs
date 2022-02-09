using System;
using UnityEngine;

interface IAttackable
{
    public Action OnDestroy { get; set; }
    public GameObject @GameObject { get; }
    public void ReceveAttack(int damage);
}