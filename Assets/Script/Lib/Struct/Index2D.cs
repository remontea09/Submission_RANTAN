public struct Index2D {
    public int v;
    public int h;

    public Index2D(int v, int h) {
        this.v = v;
        this.h = h;
    }

    public override string ToString() => $"({v}, {h})";

    public override bool Equals(object obj) => obj is Index2D other && v == other.v && h == other.h;

    public override int GetHashCode() => (v, h).GetHashCode();

    public static bool operator ==(Index2D a, Index2D b) => a.Equals(b);
    public static bool operator !=(Index2D a, Index2D b) => !a.Equals(b);

    public static Index2D operator +(Index2D a, Index2D b) => new Index2D(a.v + b.v, a.h + b.h);

    public static Index2D operator -(Index2D a, Index2D b) => new Index2D(a.v - b.v, a.h - b.h);

    public static Index2D operator +(Index2D pos, Direction dir) => new(pos.v + dir.v, pos.h + dir.h);

    public static int DistanceSquare(Index2D a, Index2D b) {
        int dv = a.v - b.v;
        int dh = a.h - b.h;
        return dv * dv + dh * dh;
    }
}
