using Catfood.Shapefile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeMap
{
    public partial class Form1 : Form
    {
        private Shapefile shp;
        private PaintEventArgs gView;
        public Form1()
        {
            InitializeComponent();
            
        }

        public void GetShapeFile(string filePath)
        {
            using (Shapefile shapefile = new Shapefile(filePath))
            {
                Console.WriteLine("ShapefileDemo Dumping {0}", filePath);
                Console.WriteLine();

                // a shapefile contains one type of shape (and possibly null shapes)
                Console.WriteLine("*Type: {0}, Shapes: {1:n0}", shapefile.Type, shapefile.Count);

                // a shapefile also defines a bounding box for all shapes in the file
                Console.WriteLine("Bounds: {0},{1} -> {2},{3}",
                    shapefile.BoundingBox.Left,
                    shapefile.BoundingBox.Top,
                    shapefile.BoundingBox.Right,
                    shapefile.BoundingBox.Bottom);
                Console.WriteLine();

                // enumerate all shapes
                foreach (Shape shape in shapefile)
                {
                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("Shape {0:n0}, Type {1}", shape.RecordNumber, shape.Type);

                    // each shape may have associated metadata
                    string[] metadataNames = shape.GetMetadataNames();
                    if (metadataNames != null)
                    {
                        Console.WriteLine("Metadata:");
                        foreach (string metadataName in metadataNames)
                        {
                            Console.WriteLine("{0}={1} ({2})", metadataName, shape.GetMetadata(metadataName), shape.DataRecord.GetDataTypeName(shape.DataRecord.GetOrdinal(metadataName)));
                        }
                        Console.WriteLine();
                    }

                    // cast shape based on the type
                    switch (shape.Type)
                    {
                        case ShapeType.Point:
                            // a point is just a single x/y point
                            ShapePoint shapePoint = shape as ShapePoint;
                            Console.WriteLine("Point={0},{1}", shapePoint.Point.X, shapePoint.Point.Y);
                            break;

                        case ShapeType.Polygon:
                            // a polygon contains one or more parts - each part is a list of points which
                            // are clockwise for boundaries and anti-clockwise for holes 
                            // see http://www.esri.com/library/whitepapers/pdfs/shapefile.pdf
                            ShapePolygon shapePolygon = shape as ShapePolygon;
                            foreach (PointD[] part in shapePolygon.Parts)
                            {
                                //Console.WriteLine("Polygon part:");
                                foreach (PointD point in part)
                                {
                                    //Console.WriteLine("{0}, {1}", point.X, point.Y);
                                }
                                //Console.WriteLine();
                            }
                            break;

                        default:
                            // and so on for other types...
                            break;
                    }

                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine();
                }

            }
        }

        private void shapeView1_Paint(object sender, PaintEventArgs e)
        {
            gView = e;

            shp = new Shapefile(Path.Combine(Application.StartupPath, "CTPRVN_201905", "TL_SCCO_CTPRVN.shp"));
            shapeView1.shapeFile = shp;
            shapeView1.DrawMap();
        }

        private void shapeView1_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(e.X + "," + e.Y);
        }

        private void shapeView1_MouseMove(object sender, MouseEventArgs e)
        {

            //System.Drawing.Drawing2D.Matrix m = gView.Graphics.Transform;
            //System.Drawing.Drawing2D.Matrix m2 = new System.Drawing.Drawing2D.Matrix();
            //m2.Translate(200, 100);
            //gView.Graphics.Transform = m2;

        }
    }
}
