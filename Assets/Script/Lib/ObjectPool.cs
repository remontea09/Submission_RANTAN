using System;
using System.Collections.Generic;

public class ObjectPool<T> {
    private readonly Queue<T> pool = new();

    private event Func<T> createFunc;
    private event Action<T> actionOnGet;
    private event Action<T> actionOnRelease;

    public ObjectPool(
        Func<T> createFunc,
        Action<T> actionOnGet = null,
        Action<T> actionOnRelease = null
    ) {
        this.createFunc = createFunc;
        this.actionOnGet = actionOnGet;
        this.actionOnRelease = actionOnRelease;
    }

    public T Get() {
        T obj;
        if (pool.Count > 0) {
            obj = pool.Dequeue();
        }
        else {
            obj = createFunc();
        }

        actionOnGet?.Invoke(obj);

        return obj;
    }
    public void Release(T obj) {
        actionOnRelease?.Invoke(obj);
        pool.Enqueue(obj);
    }
}
