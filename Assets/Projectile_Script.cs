using UnityEngine;

public class Projectile_Script : MonoBehaviour
{
    public Vector3 target;
    public float speed = 0.5f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if ((target - transform.position).magnitude < 1)
        {
            Destroy(gameObject);
        }
    }
}
