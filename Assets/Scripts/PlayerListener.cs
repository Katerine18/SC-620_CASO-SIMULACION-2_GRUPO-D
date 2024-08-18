using UnityEngine;

public class PlayerListener : MonoBehaviour
{
    private PlayerController2D _character;

    private void Awake()
    {
        _character = GetComponentInParent<PlayerController2D>();
    }
}
