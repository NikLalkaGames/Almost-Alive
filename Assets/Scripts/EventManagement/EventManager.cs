using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventManagement
{
    public struct EventParam
    {
        public string stringValue;
        public int intValue;
        public float floatValue;
        public bool boolValue;
    }
    
    public class EventManager : MonoBehaviour
    {
        [SerializeField] 
        private Dictionary<string, Action<EventParam>> eventDictionary;

        private static EventManager _eventManager;

        public static EventManager Instance
        {
            get
            {
                if (!_eventManager)
                {
                    _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    if (!_eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                    }
                    else
                    {
                        _eventManager.Init();
                    }
                }
                return _eventManager;
            }
        }

        private void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, Action<EventParam>>();
            }
        }

        public static void StartListening(string eventName, Action<EventParam> listener)
        {
            if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                //Add more event to the existing one
                thisEvent += listener;

                //Update the Dictionary
                Instance.eventDictionary[eventName] = thisEvent;
            }
            else
            {
                //Add event to the Dictionary for the first time
                thisEvent += listener;
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<EventParam> listener)
        {
            if (_eventManager == null) return;
            if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                //Remove event from the existing one
                thisEvent -= listener;

                //Update the Dictionary
                Instance.eventDictionary[eventName] = thisEvent;
            }
            
            Debug.LogError($"EventManager is cannot unsubscribe from the event called {eventName}. Event doesn't exist.");
        }

        public static void TriggerEvent(string eventName, EventParam eventParam)
        {
            if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Invoke(eventParam);       // thisEvent can't be null because every event in dictionary has listener
                // OR USE  instance.eventDictionary[eventName](eventParam);
                return;
            }
            
            Debug.LogError($"EventManager is unable to trigger the event called {eventName}");
        }
    }
}