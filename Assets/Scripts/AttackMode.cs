using System;
using UnityEngine;

public class AttackMode
{
    [SerializeField]
    string name;

    [SerializeField]
    float damage;

    [SerializeField]
    bool isPercentage;

    public string getName()
    { return name; }

    public float getDamage()
    { return damage; }

    public bool getIsPercentage()
    { return isPercentage; }
}