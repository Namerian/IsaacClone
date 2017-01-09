using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;
    private float _damage;
    private bool _isDead = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDead)
        {
            MoveProjectile();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Projectile")
        {
            return;
        }

        //Debug.Log("ProjectileController:OnCollisionEnter:hit with something!");

        Destroy(this.gameObject);
    }

    public void Initialize(Vector2 direction, float speed, float damage)
    {
        _direction = direction;
        _speed = speed;
        _damage = damage;
    }

    protected void MoveProjectile()
    {
        Vector3 movement = new Vector3(_direction.x, 0, _direction.y) * _speed * Time.deltaTime;
        this.transform.Translate(movement);
    }
}
