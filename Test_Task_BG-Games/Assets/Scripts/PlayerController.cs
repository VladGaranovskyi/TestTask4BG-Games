using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Material _yellowMaterial;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private MeshRenderer _mesh;
    private Vector3 _positionToGo;
    private bool isGreen;

    private void Start()
    {
        Invoke("MovePlayer", 2f);
    }

    private void MovePlayer()
    {
        _agent.SetDestination(_positionToGo);
    }

    public void SetTargetPosition(Vector3 pos)
    {
        _positionToGo = pos;
    }

    public bool GetGreenState()
    {
        return isGreen;
    }

    public void SetGreenState(bool b)
    {
        isGreen = b;
        _mesh.material = (isGreen ? _greenMaterial : _yellowMaterial);
    }

    public void SetAgentEnabled(bool b)
    {
        _agent.enabled = b;
        if(b) MovePlayer();
    }
}
