using System;

public readonly struct Direction {
    public int v { get; }
    public int h { get; }

    public Direction(int v, int h) {
        this.v = Math.Sign(v);
        this.h = Math.Sign(h);
    }

    public Direction(Index2D index2D) : this(index2D.v, index2D.h) { }

    public static readonly Direction None = new(0, 0);
    public static readonly Direction Up = new(-1, 0);
    public static readonly Direction RightUp = new(-1, 1);
    public static readonly Direction Right = new(0, 1);
    public static readonly Direction RightDown = new(1, 1);
    public static readonly Direction Down = new(1, 0);
    public static readonly Direction LeftDown = new(1, -1);
    public static readonly Direction Left = new(0, -1);
    public static readonly Direction LeftUp = new(-1, -1);
    public static Direction[] AllDirections => new[] { None, Up, RightUp, Right, RightDown, Down, LeftDown, Left, LeftUp };
    public static Direction[] EightDirections => new[] { Up, RightUp, Right, RightDown, Down, LeftDown, Left, LeftUp };
    public static Direction[] SearchDirections => new[] { Up, Right, Down, Left, RightUp, RightDown, LeftDown, LeftUp };

    public static Direction[] FourDirections => new[] { Up, Right, Down, Left };
    public static explicit operator Index2D(Direction d) => new Index2D(d.v, d.h);

    public static bool operator ==(Direction a, Direction b) => a.Equals(b);
    public static bool operator !=(Direction a, Direction b) => !a.Equals(b);
    public static Index2D operator *(Direction d, int scalar) => new Index2D(d.v * scalar, d.h * scalar);
    public override bool Equals(object obj) => obj is Direction other && v == other.v && h == other.h;

    public override int GetHashCode() => HashCode.Combine(v, h);

    public override string ToString() => $"({v}, {h})";
}
