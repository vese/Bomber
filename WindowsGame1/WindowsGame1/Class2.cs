using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    class Grenader
    {
        
        public Vector2 pos = new Vector2();
        public Rectangle rect;
        public Boolean alive = true;
        public float rotate = 0;
        public Boolean grenadethown = false;
        public Vector2 grenadepos = new Vector2(-1000,-1000);
        public Vector2 grenadespeed = Vector2.Zero;
        public int grenadethrowtime = 0;
        public int currentgrenade = 0;
        public Vector2 btoompos = new Vector2(-1000, -1000);
        public Boolean isbtoom = false;
        public int timebtoom = 0;
        public int timeSinceLastFrame = 0;
        public int currentFrame = 0;
        public Rectangle btoomrect;
        public int frame = 0;
        public int time = 0;

    }
}
