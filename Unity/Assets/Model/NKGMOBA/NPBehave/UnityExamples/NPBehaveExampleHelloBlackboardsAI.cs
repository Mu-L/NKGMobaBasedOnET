﻿using ETModel;
using UnityEngine;
using NPBehave;

public class NPBehaveExampleHelloBlackboardsAI: MonoBehaviour
{
    private Root behaviorTree;

    void Start()
    {
        behaviorTree = new Root(
            // toggle the 'toggled' blackboard boolean flag around every 500 milliseconds
            new Service(0.5f, () => { behaviorTree.Blackboard.SetBool("foo", !this.behaviorTree.Blackboard.GetFromSelf("foo")._bool); },
                new Selector(
                    // Check the 'toggled' flag. Stops.IMMEDIATE_RESTART means that the Blackboard will be observed for changes 
                    // while this or any lower priority branches are executed. If the value changes, the corresponding branch will be
                    // stopped and it will be immediately jump to the branch that now matches the condition.
                    new BlackboardCondition("foo", Operator.IS_EQUAL, new NP_BlackBoardDataForCompare() { _bool = true },
                        Stops.IMMEDIATE_RESTART,
                        // when 'toggled' is true, this branch will get executed.
                        new Sequence(
                            // print out a message ...
                            new Action(() =>
                            {
                                CodeTimeCostObserver.StartObserve();
                                Debug.Log("foo");
                            }),
                            // ... and stay here until the `BlackboardValue`-node stops us because the toggled flag went false.
                            new WaitUntilStopped())),

                    // when 'toggled' is false, we'll eventually land here
                    new Sequence(new Action(() =>
                        {
                            CodeTimeCostObserver.StopObserve();
                            Debug.Log("bar");
                        }),
                        new WaitUntilStopped()))));
        behaviorTree.Start();
    }
}