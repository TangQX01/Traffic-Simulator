using System.Runtime.InteropServices;

namespace CitySim {

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct HOIDetection
    {
        public readonly string imagePath;
        public readonly bool isIlleg;

        // sizeof(Detection)
        public static int Size = 6 * sizeof(int);

        // String formatting
        public override string ToString()
          => $"{imagePath} {(isIlleg ? "是" : "不是")} 骑行违法";
    };
}