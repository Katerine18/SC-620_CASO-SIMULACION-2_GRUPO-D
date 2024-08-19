using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField, Tooltip("Range interval for attacks")]
    float attackRange;

    [SerializeField, Tooltip("Amount of attacks per Range")]
    int attackRate;

    PlayerController2D _character;

    private float _attackTime;

    private void Awake()
    {
        _character = GetComponent<PlayerController2D>();
    }

    private void Update()
    {
        _attackTime -= Time.deltaTime;
        if (_attackTime < 0.0F)
        {
            _attackTime = 0.0F;
        }

        if (_attackTime == 0)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                _character.Melee();
            }
        }
    }
}
