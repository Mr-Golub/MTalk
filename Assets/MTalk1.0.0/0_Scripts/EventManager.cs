using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    [Header("初始化")]
    [SerializeField] private bool isInit;
    private Dictionary<string, List<IEventListener>> listeners;

    private void OnEnable()
    {
        if (isInit)
        {
            instance = this;
            Init();
        }
    }

    public static void Init()
    {
        Time.timeScale = 1.0f;
        Debug.Log($"EventManager inited.");
        if (instance.listeners == null)
        {
            instance.listeners = new Dictionary<string, List<IEventListener>>();
        }
        else
        {
            instance.listeners.Clear();
        }
    }

    public static void RegisterEvent(string eventName, IEventListener listener, UnityAction callback = null)
    {
        List<IEventListener> listeners = null;
        if (instance.listeners.TryGetValue(eventName, out listeners))
        {
            listeners.Add(listener);
        }
        else
        {
            listeners = new List<IEventListener>();
            listeners.Add(listener);
            instance.listeners.Add(eventName, listeners);
        }
        callback?.Invoke();
    }

    public static void RemoveEvent(string eventName, IEventListener listener)
    {
        if (instance == null) return;
        List<IEventListener> listeners = null;
        if (instance.listeners.TryGetValue(eventName, out listeners))
        {
            listeners.Remove(listener);
        }
    }

    public static void FireEvent(string eventName, params object[] data)
    {
        List<IEventListener> listeners = null;
        if (instance.listeners.TryGetValue(eventName, out listeners))
        {
            foreach (var listener in listeners)
            {
                listener.OnEvent(eventName, data);
            }
        }
    }
}

public interface IEventListener
{
    void OnEvent(string eventName, params object[] data);
}