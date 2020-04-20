using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Catfood.Shapefile;
using System.Threading;
using System.Drawing.Drawing2D;

namespace ShapeMap
{
    public partial class shapeView : UserControl
    {
        public event MouseEventHandler MouseWheel;
        private Shapefile _shapeFile;
        /*Client Screen Value*/
        private double _ClientWidth;
        private double _ClientHeight;
        private PointD _ClientCenter;
        /*Shape File Value*/
        private double _GisWidth;
        private double _GisHeight;
        private PointD _GisCenter;

        private Double _Ratio; // Screen 값과 Shp 파일의 비율
        private Double _ZoomFactor = 1; // 확대 축소 배율 값. 기본 = 1

        private Graphics g;

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0) _ZoomFactor = _ZoomFactor * 1.2;
            else _ZoomFactor = _ZoomFactor / 1.2;
            //if (MouseWheel != null)
            //{
            this.Refresh();
            //}
        }

        public Shapefile shapeFile
        {
            set { _shapeFile = value; }
        }
        public shapeView()
        {
            InitializeComponent();
            //this.SetStyle(ControlStyles.DoubleBuffer, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.UpdateStyles();

        }


        private void shapeView_Load(object sender, EventArgs e)
        {
            g = this.CreateGraphics();
        }

        private void shapeView_Paint(object sender, PaintEventArgs e)
        {

        }


        private Bitmap bmp = null;
        private Graphics gOff = null;
        public void DrawMap()
        {

            if (_shapeFile == null) return;

            //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


            _ClientWidth = this.Width;    //컨트롤 가로
            _ClientHeight = this.Height;  //컨트롤 세로
            _ClientCenter = new PointD(_ClientWidth / 2.0, _ClientHeight / 2.0);  //스크린 센터 좌표

            _GisWidth = _shapeFile.BoundingBox.Right - _shapeFile.BoundingBox.Left;   //SHAPE 파일 가로
            _GisHeight = _shapeFile.BoundingBox.Bottom - _shapeFile.BoundingBox.Top;  //SHPAE 파일 세로

            _GisCenter = new PointD(_GisWidth / 2.0 + _shapeFile.BoundingBox.Left, _GisHeight / 2.0 + _shapeFile.BoundingBox.Top);  //SHAPE 파일 센터 좌표.

            //비율 구하기
            double RatioX = _ClientWidth / _GisWidth;
            double RatioY = _ClientHeight / _GisHeight;

            if (RatioX < RatioY)
            {
                _Ratio = RatioX;
            }
            else
            {
                _Ratio = RatioY;

            }
            //using (BufferedGraphics bufferedgraphic = BufferedGraphicsManager.Current.Allocate(e.Graphics, this.ClientRectangle))
            //{

            //    bufferedgraphic.Graphics.Clear(Color.White);
            //    bufferedgraphic.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //    bufferedgraphic.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //    bufferedgraphic.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);


            bmp = new Bitmap((int)_ClientWidth, (int)_ClientHeight);
            gOff = Graphics.FromImage(bmp);

            gOff.FillRectangle(new SolidBrush(Color.White), 0, 0, bmp.Width, bmp.Height);
            gOff.DrawRectangle(Pens.Violet, 0, 0, bmp.Width, bmp.Height);
            gOff.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gOff.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            foreach (Shape shape in _shapeFile)
            {
                switch (shape.Type)
                {
                    case ShapeType.Polygon:
                        ShapePolygon shapePolygon = shape as ShapePolygon;
                        foreach (PointD[] part in shapePolygon.Parts)
                        {
                            List<PointF> points = new List<PointF>();
                            foreach (PointD point in part)
                            {

                                float screenX = Convert.ToSingle(GetGisToScreen(point).X);
                                float screenY = Convert.ToSingle(GetGisToScreen(point).Y);

                                points.Add(new PointF(screenX, screenY));
                            }
                            gOff.DrawPolygon(Pens.Black, points.ToArray());
                            gOff.FillPolygon(Brushes.YellowGreen, points.ToArray());
                            //bufferedgraphic.Graphics.DrawPolygon(Pens.Black, points.ToArray());
                        }
                        break;
                    default:
                        break;
                }
            }


            Image img = bmp as Image;

            g.DrawImage(img, 0, 0);


            //bufferedgraphic.Graphics.DrawImage(img, 0, 0);

            //p.Dispose();

            //bufferedgraphic.Render(e.Graphics);
        }

        private PointD GetGisToScreen(PointD pGisPoint)
        {
            PointD screenPoint;

            screenPoint.X = _Ratio * (pGisPoint.X - _GisCenter.X) * _ZoomFactor + _ClientCenter.X;
            screenPoint.Y = _Ratio * (_GisCenter.Y - pGisPoint.Y) * _ZoomFactor + _ClientCenter.Y;


            return screenPoint;
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        private bool isMouseDown = false;
        private Point currentPoint;
        int moveX = 0;  //이동 거리
        int moveY = 0;  //이동 거리
        private void shapeView_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            currentPoint = new Point(e.X, e.Y);
        }

        private void shapeView_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            currentPoint = new Point();
        }

        private void shapeView_MouseMove(object sender, MouseEventArgs e)
        {
          
            if (isMouseDown)
            {
               
                //g.Clear(Color.White);
              

                if(currentPoint.X <= e.X)
                {
                    moveX = e.X - currentPoint.X;
                    Console.WriteLine("right:" + moveX.ToString());
                }
                else
                {
                    Console.WriteLine("left");
                    moveX = e.X - currentPoint.X;
                }

                if (currentPoint.Y <= e.Y)
                {
                    moveX =  currentPoint.Y - e.Y;
                }
                else
                {
                    moveY = e.Y - currentPoint.Y;
                }


                label1.Text = moveX + "," + moveY;
                //Invalidate();

                //System.Drawing.Drawing2D.Matrix m = g.Transform;

                System.Drawing.Drawing2D.Matrix m2 = new System.Drawing.Drawing2D.Matrix();
                m2.Translate(moveX, moveY);
                ////Console.WriteLine(currentPoint.X.ToString() + "," + currentPoint.Y.ToString());
                ////Console.Write("->" + e.X.ToString() + "," + e.Y.ToString());
                //Console.WriteLine("=" + moveX.ToString() + "," + moveY.ToString());
                g.Transform = m2;

                //g.Clear(Color.White);

               

                Image img = bmp as Image;
                g.DrawImage(img, moveX, moveY);

            }
        }
    }
}

