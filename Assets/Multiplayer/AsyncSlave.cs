using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * With thanks to https://stackoverflow.com/questions/53916533/setactive-can-only-be-called-from-the-main-thread
 * Alternative to ThreadManager queue
 */
class AsyncSlave : MonoBehaviour
{
    internal static AsyncSlave slave;
    readonly Queue<System.Action> tasks = new Queue<System.Action>();

    private void Awake()
    {
        slave = this;
    }

    private void Update()
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
