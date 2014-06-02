using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AtomViewer
{
    public class Sphere
    {
        private Vector3 position;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        private Vector3 color;

        public Vector3 Color
        {
            get { return color; }
            set { color = value; }
        }

        private Type type;

        public Type Type
        {
            get { return type; }
            //set { type = value; }
        }

        public Sphere( Vector3 position, Vector3 color, Type type )
        {
            this.type = type;
            this.position = position;
            this.color = color;
        }
    }
}
