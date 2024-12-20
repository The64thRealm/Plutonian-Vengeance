using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    public BroadcastEvent listenToEvent;
    public UnityEvent response;

    public void setBroadcastToListen(BroadcastEvent newBroadcastEvent)
    {
        if (listenToEvent != null)
        {
            listenToEvent.UnregisterListener(this);
        }
        listenToEvent = newBroadcastEvent;
        listenToEvent.RegisterListener(this);
    }

    public void disableBroadcastListening()
    {
        listenToEvent?.UnregisterListener(this);
        listenToEvent = null;
    }

    private void OnEnable()
    {
        if (listenToEvent != null)
        {
            listenToEvent.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        if (listenToEvent != null)
        {
            listenToEvent.UnregisterListener(this);
        }
    }

    public void OnEventTriggered()
    {
        response.Invoke();
    }
}
