using UnityEngine;
using UnityEngine.AI;

public class Robot : Unit 
{

  // Parameters
  [SerializeField] Transform target;

  // Cache
  NavMeshAgent agent;

  private void Awake()
  {
    agent = GetComponent<NavMeshAgent>();

    if (target == null)
    {
    target = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has the tag "Player"
      
    }
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
