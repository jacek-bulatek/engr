using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engr.canvasController
{
    class CanvasMatrix
    {
        int _cellSize;                  // Width and height of single cell
        public int _col { get; set; }   // number of columns
        public int _row { get; set; }   // number of rows
        bool[,] _isRobotPresent;        // Array of fields true if robot is present on field flase otherwise [rows][columns]
        bool[,] _isDestPresent;         // Array of fields true if destination is present on field false otherwise

        public CanvasMatrix(int cellSize, int col, int row)
        {
            _cellSize = cellSize;
            _col = col;
            _row = row;
            _isRobotPresent = new bool[row, col];
            _isDestPresent = new bool[row, col];
        }

        public int getCellSize() { return _cellSize; }
        public bool isRobot(int row, int col) { return _isRobotPresent[row, col]; }
        public bool isDest(int row, int col) { return _isDestPresent[row, col]; }
        public void setRobot(int row, int col) { _isRobotPresent[row, col] = true; }
        public void setDest(int row, int col) { _isDestPresent[row, col] = true; }
        public void swichIsRobot(int row, int col) { _isRobotPresent[row, col] = !_isRobotPresent[row, col]; }
        public void clearIsRobot() {
            for (int i = 0; i < _row; i++)
                for (int j = 0; j < _col; j++)
                    _isRobotPresent[i, j] = false;
        }
        public void clearIsDest()
        {
            for (int i = 0; i < _row; i++)
                for (int j = 0; j < _col; j++)
                    _isDestPresent[i, j] = false;
        }
    }
}
