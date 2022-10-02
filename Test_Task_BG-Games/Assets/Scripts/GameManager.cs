using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _positionToGo;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private MazeSpawner _mazeSpawner;
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private Shield _shield;
    [SerializeField] private Pause _pause;
    [SerializeField] private Animator _blackScreen;
    [SerializeField] private ParticleSystem _confetti;
    private Transform _currentPlayer;
    private NavMeshAgent _agent;
    private Rigidbody _rigidbody;
    private bool isGenerating;

    private void Start()
    {
        StartGame();
        StartCoroutine(ReGenerateCoroutine());
    }

    private void FixedUpdate()
    {
        if(Mathf.Pow(_currentPlayer.position.x - _positionToGo.position.x, 2) +
            Mathf.Pow(_currentPlayer.position.z - _positionToGo.position.z, 2) < 1f && !isGenerating)
        {
            StartCoroutine(ReGenerateCoroutine());
        }
    }

    private void StartGame()
    {
        isGenerating = false;
        _blackScreen.gameObject.SetActive(true);
        _blackScreen.Play("BlackScreenDisappear");
        Invoke("DeActivateBlackScreen", 2f);
        _currentPlayer = Instantiate(_player);
        _currentPlayer.position = _spawnPosition.position;
        _rigidbody = _currentPlayer.GetComponent<Rigidbody>();
        PlayerController pc = _currentPlayer.GetComponent<PlayerController>();
        pc.SetTargetPosition(_positionToGo.position);
        _shield.SetPlayerController(pc);
        _pause.SetPlayerController(pc);
        _virtualCamera.Follow = _currentPlayer;
        _virtualCamera.LookAt = _currentPlayer;
        _mazeSpawner.enabled = true;
        _agent = _currentPlayer.GetComponent<NavMeshAgent>();
        _mazeSpawner.SetAgent(_agent);
        _mazeSpawner.SetPosToGo(_positionToGo);
    }

    public void RestartGame()
    {
        _mazeSpawner.enabled = true;
        _deathParticles.transform.position = _currentPlayer.position;
        _deathParticles.Play();
        Destroy(_currentPlayer.gameObject);
        Invoke("StartGame", 2f);
    }

    private void DeActivateBlackScreen()
    {
        _blackScreen.gameObject.SetActive(false);
    }

    private IEnumerator ReGenerateCoroutine()
    {
        isGenerating = true;
        _mazeSpawner.enabled = true;
        _confetti.Play();
        _blackScreen.gameObject.SetActive(true);
        _blackScreen.Play("BlackScreenPopUp");
        yield return new WaitForSeconds(2f);
        _mazeSpawner.ReGenerateMaze();
        Destroy(_currentPlayer.gameObject);
        StartGame();
    }
}
