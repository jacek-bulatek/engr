using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engr.canvasController
{
    class CanvasController
    {
        System.Windows.Forms.PictureBox _canvas;
        public CanvasMatrix _canvasMatrix;
        System.Drawing.Bitmap _bitmap;
        System.Drawing.Graphics _graphic;

        public CanvasController(ref System.Windows.Forms.PictureBox canvas)
        {
            _canvas = canvas;
            _canvasMatrix = new CanvasMatrix(10, canvas.Width / 10, canvas.Height / 10);
            _bitmap = new System.Drawing.Bitmap(canvas.Width, canvas.Height);
            _graphic = System.Drawing.Graphics.FromImage(_bitmap);
            _graphic.Clear(System.Drawing.Color.White);
            _canvas.Refresh();
        }
        
        public void refreshCanvas()
        {
            clearCanvas();
            drawMatrix();
            _canvas.Image = _bitmap;
            _canvas.Refresh();
        }
        void drawRobot(int row, int col)
        {
            _graphic.FillEllipse(System.Drawing.Brushes.Yellow, _canvasMatrix.getCellSize() * col, _canvasMatrix.getCellSize() * row, _canvasMatrix.getCellSize(), _canvasMatrix.getCellSize());
        }
        void drawDest(int row, int col)
        {
            _graphic.FillRectangle(System.Drawing.Brushes.Pink, _canvasMatrix.getCellSize() * col, _canvasMatrix.getCellSize() * row, _canvasMatrix.getCellSize(), _canvasMatrix.getCellSize());
        }
        void drawRobotOnDest(int row, int col)
        {

        }
        void drawMatrix()
        {
            for(int col = 0; col < _canvasMatrix._col; col++)
            {
                for(int row = 0; row < _canvasMatrix._row; row++)
                {
                    if (_canvasMatrix.isDest(row, col))
                        drawDest(row, col);
                }
            }
            for (int col = 0; col < _canvasMatrix._col; col++)
            {
                for (int row = 0; row < _canvasMatrix._row; row++)
                {
                    if (_canvasMatrix.isRobot(row, col))
                        drawRobot(row, col);
                }
            }
        }
        public void setDestPic(string[] destPic)
        {
            _canvasMatrix.clearIsDest();
            foreach (string field in destPic)
            {
                string[] coordinates = field.Split(' ');
                int row, col;
                Int32.TryParse(coordinates[0], out row);
                Int32.TryParse(coordinates[1], out col);
                _canvasMatrix.setDest(row, col);
            }
        }
        void clearCanvas()
        {
            _graphic.Clear(System.Drawing.Color.White);
        }
    }
}
