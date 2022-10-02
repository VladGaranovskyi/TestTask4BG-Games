using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeSpawner : MonoBehaviour
{
    [SerializeField] private Transform _wallPrefab;
    [SerializeField] private float _wallCount;
    [SerializeField] private Vector2 _positionBorder1;
    [SerializeField] private Vector2 _positionBorder2;
    [SerializeField] private Vector2 _scaleBorders;
    [SerializeField] private float _redFieldCount;
    [SerializeField] private NavigationBaker _navigationBaker;
    [SerializeField] private Transform _redFieldPrefab;
    private List<Transform> _walls = new List<Transform>();
    private List<Transform> _redFields = new List<Transform>();
    private NavMeshAgent _agent;
    private Transform _positionToGo;
    private readonly Vector2 _borderZone1 = new Vector2(45f, -35f);
    private readonly Vector2 _borderZone2 = new Vector2(35f, 45f);
    private bool isGenerating;

    private void Start()
    {
        GenerateMaze();
    }

    private void FixedUpdate()
    {
        if (!isGenerating)
        {
            if (CalculateNewPath()) this.enabled = false;
            else
            {
                ReGenerateMaze();
            }
        }
    }

    private void GenerateMaze()
    {
        isGenerating = true;
        for (int i = 0; i < _wallCount; i++)
        {
            Transform obj = Instantiate(_wallPrefab);
            obj.position = Vector3.right * Random.Range(_positionBorder1.x, _positionBorder2.x)
                + Vector3.forward * Random.Range(_positionBorder1.y, _positionBorder2.y) +
                Vector3.up * 2.5f;
            if (_walls.Count > 0)
            {
                bool isNear = false;
                foreach (var wall in _walls)
                {
                    if ((obj.position - wall.position).magnitude < 9f)
                    {
                        isNear = true;
                    }
                }
                if (isNear)
                {
                    Destroy(obj.gameObject);
                    i--;
                    continue;
                }
            }

            obj.eulerAngles = Vector3.up * (i % 2) * 90f;

            obj.localScale = Vector3.right * 
                Random.Range(_scaleBorders.x, _scaleBorders.y)
                + Vector3.forward * 0.5f + Vector3.up * 5f;
            NavMeshSurface surface = obj.gameObject.AddComponent<NavMeshSurface>();
            _navigationBaker.AddSurface(surface);
            _navigationBaker.OnSpawnObject?.Invoke();
            _walls.Add(obj);
        }       

        for (int i = 0; i < _redFieldCount; i++)
        {
            Transform obj = Instantiate(_redFieldPrefab);
            obj.position = Vector3.right * Random.Range(_borderZone1.x, _borderZone2.x)
                + Vector3.forward * Random.Range(_borderZone1.y, _borderZone2.y) +
                Vector3.up * 0.01f;
            if(_redFields.Count > 0)
            {
                bool isNear = false;
                foreach (var field in _redFields)
                {
                    if ((obj.position - field.position).magnitude < 4f)
                    {
                        isNear = true;
                    }
                }
                if (isNear)
                {
                    Destroy(obj.gameObject);
                    i--;
                    continue;
                }
            }
            _redFields.Add(obj);
        }

        foreach(var field in _redFields)
        {
            foreach(var wall in _walls)
            {
                if((field.position - wall.position).magnitude < 1)
                {
                    Destroy(field.gameObject);
                }
            }
        }
        _redFields.Clear();
        _walls.Clear();
        isGenerating = false;
    }

    public void ReGenerateMaze()
    {
        _navigationBaker.ClearSurfaces();
        GenerateMaze();
    }

    private bool CalculateNewPath()
    {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(_positionToGo.position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    public void SetAgent(NavMeshAgent agent)
    {
        _agent = agent;
    }

    public void SetPosToGo(Transform pos)
    {
        _positionToGo = pos;
    }
}
