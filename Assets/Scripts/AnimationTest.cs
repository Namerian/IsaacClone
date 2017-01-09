using UnityEngine;
using System.Collections;

public class AnimationTest : MonoBehaviour
{
    private Animator _animatorComponent;

    // Use this for initialization
    void Start()
    {
        _animatorComponent = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _animatorComponent.SetTrigger("DeathTrigger");
        }
    }
}
