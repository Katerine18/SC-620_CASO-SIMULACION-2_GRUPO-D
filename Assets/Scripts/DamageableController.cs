using UnityEngine;

public class DamageableController : MonoBehaviour
{
    [SerializeField]
    float maxHealth;

    [SerializeField]
    bool isEnemy;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float value, bool isPercentage = false)
    {
        float damage = Mathf.Abs(value);

        if (isPercentage)
        {
            damage = maxHealth * 100 / maxHealth;
        }

        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            if (isEnemy)
            {
                EnemyController enemy = GetComponent<EnemyController>();

                if (enemy != null)
                {
                    enemy.Die();
                }
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }


}