
[System.Serializable]
public struct Energy {
    public int red;
    public int green;
    public int blue;

    public Energy(int value) {
        red = value;
        green = value;
        blue = value;
    }
    public Energy(int red, int green, int blue) {
        this.red = red;
        this.green = green;
        this.blue = blue;
    }

    public static Energy operator -(Energy a, Energy b) {
        return new Energy {
            red = a.red - b.red,
            green = a.green - b.green,
            blue = a.blue - b.blue
        };
    }
}
