using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Broadcast Event", menuName = "Events/BroadcastEvent")]
public class BroadcastEvent : ScriptableObject
{
    public List<EventListener> listeners = new();
    public UnityEvent OnBroadcast;

    public void RegisterListener(EventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(EventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Trigger()
    {
        foreach (EventListener listener in listeners)
        {
            listener.OnEventTriggered();
        }
    }
}
