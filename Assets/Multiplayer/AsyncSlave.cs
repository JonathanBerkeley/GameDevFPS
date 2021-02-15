using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * With thanks to https://stackoverflow.com/questions/53916533/setactive-can-only-be-called-from-the-main-thread
 */
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
