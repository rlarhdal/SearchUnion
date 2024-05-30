using UnityEngine;

public class Mettress : MonoBehaviour
{
    private float jumpForce;
    private float jumpPower = 180;

    private void OnTriggerEnter(Collider other)
    {
        jumpForce = CharacterManager.Instance.player.controller.jumpForce;
        if (other.CompareTag("Player"))
        {
            CharacterManager.Instance.player.controller.jumpForce = jumpPower;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterManager.Instance.player.controller.jumpForce = jumpForce;
        }
    }
}
