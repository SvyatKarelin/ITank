using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventHandler : MonoBehaviour
{
    [System.Serializable]
    public class GameEvent
    {
        public string name;
        public UnityEvent Event;
    }

    public List<GameEvent> Events;

    private GameEvent GetEvent(string EventName) {
        foreach (GameEvent Evnt in Events) if(Evnt.name == EventName) return Evnt;
        return null;
    }

    public UnityEvent GetEventByName(string EventName) => GetEvent(EventName)?.Event;

    public void Invoke(string EventName)
    {
        GetEvent(EventName)?.Event?.Invoke();
    }
}
