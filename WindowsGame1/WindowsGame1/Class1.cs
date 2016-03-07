using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    public class Lvl
    {
        public class LvlInfo
        {
            public List<Vector4> li1 = new List<Vector4>();
            public List<Vector2> li2 = new List<Vector2>();

            public List<Vector2> li3 = new List<Vector2>();
        }
            string n1 = "map";
            string n2 = ".xml";
            public void Save(List<Vector4> l1, List<Vector2> l2,List<Vector2> l3)
            {
                var p = new List<LvlInfo>()
                {
                   new LvlInfo()
                   {li1 = l1, li2 = l2, li3 = l3}
                };
                try
                {
                    using (var fs = new StreamWriter(n1+"2"+n2))
                    {
                        var s = new XmlSerializer(p.GetType());
                        s.Serialize(fs, p);
                    }
                }
                catch (Exception e)
                { }
            }
            public void Read(ref List<Vector4> l1, ref List<Vector2> l2, ref List<Vector2> l3, int i)//ref List<Texture2D> l1, ref List<Vector2> l2, ref List<Rectangle> l3)
            {
                try
                {
                    using (var rd = new StreamReader(n1+i.ToString()+n2))
                    {
                        var s = new XmlSerializer(typeof(List<LvlInfo>));
                        var p = (List<LvlInfo>)s.Deserialize(rd);
                        foreach (var c in p)
                        {
                            l1 = c.li1;
                            l2 = c.li2;
                            l3 = c.li3;
                        }
                    }
                }
                catch (Exception e)
                { }
            }

       
    }
}
