using UnityEngine;

public class RedField : MonoBehaviour
{
    private GameManager _gm;
    private bool isPlayerIn;
    private Transform _player;
    private PlayerController _playerController;

    private void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            _player = other.transform;
            _playerController = _player.GetComponent<PlayerController>();
            if (_playerController.GetGreenState()) isPlayerIn = true;
            else _gm.RestartGame();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            isPlayerIn = false;
        }
    }

    private void FixedUpdate()
    {
        if (isPlayerIn && !_playerController.GetGreenState()) _gm.RestartGame();
    }
}
