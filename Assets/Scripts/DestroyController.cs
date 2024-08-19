using UnityEngine;

public class DestroyController : MonoBehaviour
{
    [SerializeField]
    LayerMask ignoreMask;

    GameObject _enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController2D controller = other.GetComponent<PlayerController2D>();
            controller.Die();
            return;
        }

        if ((ignoreMask & (1 << other.gameObject.layer)) != 0)
        {
            return;
        }

        Destroy(other.gameObject);
    }

    public void AttackEnemy()
    {
        _enemy.SetActive(false);
        Destroy(_enemy);
    }
}