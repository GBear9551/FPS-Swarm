using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{

  // Parameters
  [SerializeField] Transform target;

  // Cache
  NavMeshAgent agent;

  private void Awake()
  {
    agent = GetComponent<NavMeshAgent>();
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    agent.SetDestination(target.position);
  }
}
