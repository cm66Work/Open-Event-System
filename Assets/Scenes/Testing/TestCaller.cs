using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCaller : MonoBehaviour
{
    [SerializeField]
    private OpenEvents.GameEventSO _gameEventTestTrigger;

    public void TriggerGameObjectEvent()
    {
        _gameEventTestTrigger?.Raise(this, null);
    }
}
