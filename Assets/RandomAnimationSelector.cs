using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationSelector : StateMachineBehaviour {
    [SerializeField] string parameterName = "RandomIndex";
    [SerializeField] int min_inclusive = 0;
    [SerializeField] int max_exclusive = 1;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash) {
        animator.SetInteger(parameterName, UnityEngine.Random.Range(min_inclusive, max_exclusive));
    }
}