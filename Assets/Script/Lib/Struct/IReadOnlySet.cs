using System.Collections.Generic;

public interface IReadOnlySet<T> : IEnumerable<T>, IReadOnlyCollection<T> {
    public bool Contains(T item);
}
