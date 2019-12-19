using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    public Action<T> action;
    public abstract State<T> InputHandle(T t);
    public State()
    {
        action = Enter;
    }
    public virtual void Enter(T t)
    {
        action = Update;
    }
    public virtual void Update(T t)
    {

    }
    public virtual void Exit(T t)
    {

    }


}
