using UnityEngine;

public abstract class PickUp : MonoBehaviour
{

    [SerializeField] private float rotationRate = 50f;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up * Time.deltaTime * rotationRate);
    }

  private void OnTriggerEnter(Collider other)
  {
        if (other.CompareTag("Player"))
        {
            OnPickUp(other);
        }
  }
  

  protected abstract void OnPickUp(Collider other);

}
