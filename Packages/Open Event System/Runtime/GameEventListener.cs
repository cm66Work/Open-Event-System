using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OpenEvents
{
    // This give us our GUI on Game Event Listener Component within the Inspector.
    [System.Serializable]
    public class CustomeGameEvent : UnityEvent<Component, object> { }

    public class GameEventListener : MonoBehaviour
    {
        public GameEventSO GameEventSO;

        public CustomeGameEvent Response;

        public void OnEnable()
        {
            GameEventSO.RegisterListener(this);
        }

        private void OnDisable()
        {
            GameEventSO.UnregisterListener(this);
        }

        /// <summary>
        /// Invokes a unity event when called.
        /// </summary>
        /// <param name="sender"> Is used for callbacks to the component that trigged the event. </param>
        /// <param name="data"> Holds the data that is broadcast to listeners.</param>
        public void OnEventRaised(Component sender, object data)
        {
            Response?.Invoke(sender, data);
        }
    }
}
