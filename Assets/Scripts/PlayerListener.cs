using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListener : MonoBehaviour
{
    [SerializeField]
    AttackMode[] attackModes;

    private PlayerController2D _character;

    private void Awake()
    {
        _character = GetComponentInParent<PlayerController2D>();
    }

    private AttackMode GetAttackMode(string name)
    {
        foreach (AttackMode attackMode in attackModes)
        {
            if (attackMode.getName().Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                return attackMode;
            }
        }
        return null;
    }

    public void OnMelee()
    {
        AttackMode attackMode = GetAttackMode("OnMelee");
        _character.Melee(attackMode.getDamage(), attackMode.getIsPercentage());
    }

    public void OnFire()
    {
        AttackMode attackMode = GetAttackMode("OnFire");
        _character.Fire(attackMode.getDamage(), attackMode.getIsPercentage());
    }
}
