using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuintaEvaluacion.Models
{
    public class Imagen
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public byte[] Img { get; set; }
    }
}