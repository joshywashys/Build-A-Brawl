using UnityEngine;
using UnityEngine.InputSystem;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInput player = other.GetComponent<PlayerInput>();
        if (player != null)
            Kill(player);
    }

    private void Kill(PlayerInput player)
    {
        player.DeactivateInput();
        GameController.GetPlayer(player.playerIndex).isAlive = false;
        player.gameObject.SetActive(false);
    }
}
