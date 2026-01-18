using System.Collections.Generic;
using System.Linq;

public struct UnionFind {
    List<int> par;
    List<int> siz;

    public UnionFind(int n) {
        par = Enumerable.Repeat(-1, n).ToList();
        siz = Enumerable.Repeat(1, n).ToList();
    }

    public int root(int x) {
        if (par[x] == -1) return x;
        else return par[x] = root(par[x]);
    }

    public bool issame(int x, int y) {
        return root(x) == root(y);
    }

    public bool unite(int x, int y) {
        x = root(x);
        y = root(y);
        if (x == y) return false;
        if (siz[x] < siz[y]) {
            var temp = siz[x];
            siz[x] = siz[y];
            siz[y] = temp;
        }
        par[y] = x;
        siz[x] += siz[y];
        return true;
    }

    public int size(int x) {
        return siz[root(x)];
    }
}
