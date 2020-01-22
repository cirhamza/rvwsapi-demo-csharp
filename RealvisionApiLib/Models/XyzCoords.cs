using System;
using System.Collections.Generic;
using System.Text;

namespace REALvisionApiLib.Models
{
    public class XyzCoords
    {

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public XyzCoords()
        {

        }

        public XyzCoords(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

    }

    public class RotationXyz : XyzCoords
    {
        public RotationXyz()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }
    }

    public class ScaleXyz : XyzCoords
    {
        public ScaleXyz()
        {
            this.X = 1;
            this.Y = 1;
            this.Z = 1;
        }
    }

    public class PositionXyz : XyzCoords
    {
        public PositionXyz()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }
    }
}
