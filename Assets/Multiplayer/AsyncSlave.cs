using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class AsyncSlave : MonoBehaviour
{
    internal static AsyncSlave slave;
    Queue<System.Action> tasks = new Queue<System.Action>();

    void Awake()
    {
        slave = this;
    }

    void Update()
    {
        while (tasks.Count > 0)
        {
            tasks.Dequeue().Invoke();
        }
    }
    
    internal void AddTask(System.Action newTask)
    {
        tasks.Enqueue(newTask);
    }
}
