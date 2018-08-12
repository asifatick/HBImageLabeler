using System;
using System.Collections;
using System.Collections.Generic;

namespace HBImageLabaler
{
    public class Project
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string LastActiveSourceFolder { get; set; }
        public DateTime CreatedOn { get; set; }
        public string User { get; set; }
        public List<string> Classes { get; set; }
        public List<Img>  Images { get; set; }

    }
    public class Img
    {
        public string Id { get; set; }
        public string OriginalName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public List<ImgLabel> AnnotatedLabels { get; set; }
    }
    
    public class ImgLabel
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public string Label { get; set; }
    }
}