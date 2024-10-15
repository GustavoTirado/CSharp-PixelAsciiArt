using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooldialog
{
    internal class Particle
    {
        public Rectangle Shape { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float Lifetime { get; set; }  // Tiempo de vida de la partícula

        public Particle(int x, int y, int width, int height, float velocityX, float velocityY, float lifetime)
        {
            Shape = new Rectangle(x, y, width, height);
            VelocityX = velocityX;
            VelocityY = velocityY;
            Lifetime = lifetime;
        }

        public void Update(float deltaTime)
        {
            // Actualizar la posición con la velocidad
            Shape = new Rectangle((int)(Shape.X + VelocityX * deltaTime), (int)(Shape.Y + VelocityY * deltaTime), Shape.Width, Shape.Height);

            // Reducir el tiempo de vida
            Lifetime -= deltaTime;
        }

        public bool IsAlive()
        {
            return Lifetime > 0;
        }
    }
}
