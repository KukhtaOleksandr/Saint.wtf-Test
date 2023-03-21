using System.Collections;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string RunTrigger = "DoRun";
    private const string IdleTrigger = "DoIdle";
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private Animator _animator;
    private bool _objectAlive;
    private bool isLastRun = false;

    void Start()
    {
        _objectAlive = true;
        StartCoroutine("CheckForRun");
    }

    void OnDestroy()
    {
        _objectAlive = false;
    }

    private IEnumerator CheckForRun()
    {
        while (_objectAlive)
        {
            if (_joystick.Horizontal != 0 && _joystick.Vertical != 0)
            {
                if(isLastRun == false)
                    _animator.SetTrigger(RunTrigger);
                isLastRun = true;
            }
            else
            {
                if(isLastRun == true)
                    _animator.SetTrigger(IdleTrigger);
                isLastRun = false;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }


}
