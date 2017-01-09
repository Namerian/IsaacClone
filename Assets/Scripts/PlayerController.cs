using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public int _healthPoints = 5;
    public float _movementSpeed = 2;
    public float _firingSpeed = 1;
    public float _projectileSpeed = 4;
    public float _projectileDamage = 1;

    public GameObject _projectilePrefab;

    private CharacterController _characterControllerComponent;

    private Animator _headAnimator;

    private Vector2 _movementDirection = new Vector2(0, 1);
    private Vector2 _facingDirection = new Vector2(0, 1);
    private bool _firingCooldown = false;
    private bool _firingOffset = false;

    // Use this for initialization
    void Start()
    {
        _characterControllerComponent = GetComponent<CharacterController>();

        _headAnimator = this.transform.FindChild("Head").GetComponent<Animator>();

        _headAnimator.SetBool("isFacingDown", true);
    }

    // Update is called once per frame
    void Update()
    {

        //movement
        Vector3 newDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Z))
        {
            newDirection.z = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            newDirection.z = -1;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newDirection.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            newDirection.x = 1;
        }

        if (newDirection.magnitude > 0)
        {
            newDirection.Normalize();
            float speed = _movementSpeed * Time.deltaTime;
            _characterControllerComponent.Move(newDirection * speed);

            _movementDirection = new Vector2(newDirection.x, newDirection.z);

            if (Mathf.Abs(_movementDirection.x) == 1f || Mathf.Abs(_movementDirection.y) == 1f)
            {
                _facingDirection = _movementDirection;

                if(_facingDirection.x == -1)
                {
                    _headAnimator.SetBool("isFacingUp", false);
                    _headAnimator.SetBool("isFacingDown", false);
                    _headAnimator.SetBool("isFacingLeft", true);
                    _headAnimator.SetBool("isFacingRight", false);
                }
                else if(_facingDirection.x == 1)
                {
                    _headAnimator.SetBool("isFacingUp", false);
                    _headAnimator.SetBool("isFacingDown", false);
                    _headAnimator.SetBool("isFacingLeft", false);
                    _headAnimator.SetBool("isFacingRight", true);
                }
                else if(_facingDirection.y == -1)
                {
                    _headAnimator.SetBool("isFacingUp", false);
                    _headAnimator.SetBool("isFacingDown", true);
                    _headAnimator.SetBool("isFacingLeft", false);
                    _headAnimator.SetBool("isFacingRight", false);
                }
                else if(_facingDirection.y == 1)
                {
                    _headAnimator.SetBool("isFacingUp", true);
                    _headAnimator.SetBool("isFacingDown", false);
                    _headAnimator.SetBool("isFacingLeft", false);
                    _headAnimator.SetBool("isFacingRight", false);
                }
            }
        }

        //shooting
        if (!_firingCooldown && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 projectilePosition = new Vector3(_facingDirection.x, 0, _facingDirection.y) + this.transform.position;

            if (_facingDirection.x == 1)
            {
                if (_firingOffset)
                {
                    projectilePosition.z += 0.3f;
                }
                else
                {
                    projectilePosition.z -= 0.3f;
                }
            }
            else if (_facingDirection.y == 1)
            {
                if (_firingOffset)
                {
                    projectilePosition.x += 0.3f;
                }
                else
                {
                    projectilePosition.x -= 0.3f;
                }
            }

            _firingOffset = !_firingOffset;

            GameObject projectileObject = (GameObject)Instantiate(_projectilePrefab, projectilePosition, Quaternion.identity);
            projectileObject.GetComponent<ProjectileController>().Initialize(_facingDirection, _projectileSpeed, _projectileDamage);

            //Debug.Log("PlayerController:Update:projectile fired! pos=" + projectilePosition);

            _firingCooldown = true;
            Invoke("OnFiringCooldownComplete", _firingSpeed);
        }
    }

    private void OnFiringCooldownComplete()
    {
        _firingCooldown = false;
    }
}
