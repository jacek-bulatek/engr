using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engr.agentController
{
    class Agent
    {
        event CollisionEventHandler collision;
        canvasController.CanvasMatrix _matrix;
        canvasController.CanvasMatrix _surroundings;
        delegate bool getInfo(int row, int col);
        bool _isDecided;
        Random _genRandom;
        enum Direction { N, NE, E, SE, S, SW, W, NW, None }
        public int[] _coordinates { get; set; }
        public Agent(ref canvasController.CanvasMatrix matrix, int rowNo, int colNo)
        {
            _matrix = matrix;
            _genRandom = new Random();
            _isDecided = false;
            _coordinates = new int[2];
            _surroundings = new canvasController.CanvasMatrix(10, 5, 5);
            drawCoordinates(rowNo, colNo);
            updateSurroundings();
        }
        public bool isDecided() { return _isDecided; }
        /*
         * Function: drawCoordinates
         * 
         * Function draws random coordinates
         * as long, as no other robot is present
         * on drawn.
         * 
         */
        public void drawCoordinates(int rowNo, int colNo)
        {
            _coordinates[0] = _genRandom.Next(0, rowNo);
            _coordinates[1] = _genRandom.Next(0, colNo);
            while (true)
            {
                if (_matrix.isRobot(_coordinates[0], _coordinates[1]))
                {
                    _coordinates[0] = _genRandom.Next(0, rowNo);
                    _coordinates[1] = _genRandom.Next(0, colNo);
                }
                else
                {
                    _matrix.setRobot(_coordinates[0], _coordinates[1]);
                    break;
                }
            }
        }
        /*
         * Function: move
         * 
         * Function invokes other functions
         * to determine moving direction if 
         * any and then changes coordinates
         * accordingly.
         * 
         */
        public void move()
        {
            if (_matrix.isDest(_coordinates[0], _coordinates[1]))
            {
                // Code if on destination
                return;
            }
            else
            {
                // Code if seeking destination
                Direction dir = examineSurroundings(_surroundings.isDest);
                //if (dir == Direction.None)
                //    dir = examineSurroundings(_surroundings.isRobot);
                switch (dir)
                {
                    case Direction.N:
                        if (!examineDirection(Direction.N))
                            break;
                        _matrix.swichIsRobot(_coordinates[0]--, _coordinates[1]);
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                        break;
                    case Direction.NE:
                        if (!examineDirection(Direction.NE))
                            break;
                        _matrix.swichIsRobot(_coordinates[0]--, _coordinates[1]++);
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                        break;
                    case Direction.E:
                        if (!examineDirection(Direction.E))
                            break;
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]++);
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                        break;
                    case Direction.SE:
                        if (!examineDirection(Direction.SE))
                            break;
                        _matrix.swichIsRobot(_coordinates[0]++, _coordinates[1]++);
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                        break;
                    case Direction.S:
                        if (!examineDirection(Direction.S))
                            break;
                        _matrix.swichIsRobot(_coordinates[0]++, _coordinates[1]);
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                        break;
                    case Direction.SW:
                        if (!examineDirection(Direction.SW))
                            break;
                        _matrix.swichIsRobot(_coordinates[0]++, _coordinates[1]--);
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                        break;
                    case Direction.W:
                        if (!examineDirection(Direction.W))
                            break;
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]--);
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                        break;
                    case Direction.NW:
                        if (!examineDirection(Direction.NW))
                            break;
                        _matrix.swichIsRobot(_coordinates[0]--, _coordinates[1]--);
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                        break;
                    case Direction.None:
                        int tempRow = _coordinates[0] + _genRandom.Next(-1, 2);
                        int tempCol = _coordinates[1] + _genRandom.Next(-1, 2);
                        for (; tempRow < 0 || tempCol < 0 || tempRow >= _matrix._row || tempCol >= _matrix._col || _matrix.isRobot(tempRow, tempCol); tempRow = _coordinates[0] + _genRandom.Next(-1, 2))
                            tempCol = _coordinates[1] + _genRandom.Next(-1, 2);
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                        _coordinates[0] = tempRow;
                        _coordinates[1] = tempCol;
                        _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                        break;
                }
            }
            updateSurroundings();
        }
        /*
         * Function: updateSurroundings
         * 
         * Function sync up _isRobot and
         * _isDest fields of _surroundings
         * with _matrix
         * 
         */ 
        void updateSurroundings()
        {
            _surroundings.clearIsDest();
            _surroundings.clearIsRobot();
            for (int row = -2; row < 3; row++)
            {
                for(int col = -2; col < 3; col++)
                {
                    if (_coordinates[0] + row < _matrix._row && _coordinates[0] + row > 0 && _coordinates[1] + col < _matrix._col && _coordinates[1] + col > 0)
                    {
                        if (_matrix.isDest(_coordinates[0] + row, _coordinates[1] + col))
                            _surroundings.setDest(row+2, col+2);

                        if (_matrix.isRobot(_coordinates[0] + row, _coordinates[1] + col))
                            _surroundings.setRobot(row+2, col+2);
                    }
                }
            }
        }
        /*
         * Function: prepareToMove
         * 
         * Function resets _isDecided flag
         * 
         */
        void prepareToMove()
        {
            _isDecided = false;
        }
        /*
         * Function: examineDirection
         * 
         * Function Checks given direction
         * returns false if robot or wall
         * in a way.
         * 
         */ 
        bool examineDirection(Direction dir)
        {
            switch(dir)
            {
                case Direction.N:
                    if (_coordinates[0] - 1 < _matrix._row)
                        return !_matrix.isRobot(_coordinates[0] - 1, _coordinates[1]);
                    else
                        return false;
                case Direction.NE:
                    if (_coordinates[0] - 1 < _matrix._row && _coordinates[1] + 1 < _matrix._col)
                        return !_matrix.isRobot(_coordinates[0] - 1, _coordinates[1] + 1);
                    else
                        return false;
                case Direction.E:
                    if (_coordinates[1] + 1 < _matrix._col)
                        return !_matrix.isRobot(_coordinates[0], _coordinates[1] + 1);
                    else
                        return false;
                case Direction.SE:
                    if (_coordinates[0] + 1 > 0 && _coordinates[1] + 1 < _matrix._col)
                        return !_matrix.isRobot(_coordinates[0] + 1, _coordinates[1] + 1);
                    else
                        return false;
                case Direction.S:
                    if (_coordinates[0] + 1 > 0)
                        return !_matrix.isRobot(_coordinates[0] + 1, _coordinates[1]);
                    else
                        return false;
                case Direction.SW:
                    if (_coordinates[0] + 1 > 0 && _coordinates[1] - 1 > 0)
                        return !_matrix.isRobot(_coordinates[0] + 1, _coordinates[1] - 1);
                    else
                        return false;
                case Direction.W:
                    if (_coordinates[1] - 1 > 0)
                        return !_matrix.isRobot(_coordinates[0], _coordinates[1] - 1);
                    else
                        return false;
                case Direction.NW:
                    if (_coordinates[0] - 1 < _matrix._row && _coordinates[1] - 1 > 0)
                        return !_matrix.isRobot(_coordinates[0] - 1, _coordinates[1] - 1);
                    else
                        return false;
                default:
                    return false;
            }
        }
        /*
         *  Function: examineSurroundings 
         *  
         *  Function checks surroundings parts:
         *  E.g. NW:        E.g. E:
         *  1 1 1 0 0       0 0 0 0 0
         *  1 1 0 0 0       0 0 0 1 1
         *  1 0 0 0 0       0 0 0 1 1
         *  0 0 0 0 0       0 0 0 1 1
         *  0 0 0 0 0       0 0 0 0 0
         *  
         *  And counts existing destinations or
         *  robots (depending on passed delegate).
         * 
         *  Returns direction of highest weight.
         * 
         */
        Direction examineSurroundings(Func<int, int, bool> isBool)
        {
            int weight = 0;
            int tempWeight = 0;
            Direction tempDir = Direction.None;
            for (Direction iterator = 0; iterator <= Direction.None; iterator++)
            {
                switch (iterator)
                {
                    case Direction.NW:
                        for (int i = 0; i < 3; i++)
                            if (isBool(0, i))
                                tempWeight++;
                        for (int i = 0; i < 2; i++)
                            if (isBool(1, i))
                                tempWeight++;
                        if (isBool(2, 0))
                            tempWeight++;
                        if (tempWeight > weight)
                        {
                            tempDir = Direction.NW;
                            weight = tempWeight;
                        }
                        tempWeight = 0;
                        break;
                    case Direction.N:
                        for (int i = 1; i < 4; i++)
                        {
                            if (isBool(0, i))
                                tempWeight++;
                            if (isBool(1, i))
                                tempWeight++;
                        }
                        if (tempWeight > weight)
                        {
                            tempDir = Direction.N;
                            weight = tempWeight;
                        }
                        tempWeight = 0;
                        break;
                    case Direction.NE:
                        for (int i = 2; i < 5; i++)
                            if (isBool(0, i))
                                tempWeight++;
                        for (int i = 3; i < 5; i++)
                            if (isBool(1, i))
                                tempWeight++;
                        if (isBool(2, 4))
                            tempWeight++;
                        if (tempWeight > weight)
                        {
                            tempDir = Direction.NE;
                            weight = tempWeight;
                        }
                        else if (tempWeight == weight)
                            if (tempDir != Direction.E && tempDir != Direction.SE)
                            {
                                tempDir = Direction.NE;
                                weight = tempWeight;
                            }
                        tempWeight = 0;
                        break;
                    case Direction.E:
                        for (int i = 1; i < 4; i++)
                        {
                            if (isBool(i, 3))
                                tempWeight++;
                            if (isBool(i, 4))
                                tempWeight++;
                        }
                        if (tempWeight > weight)
                        {
                            tempDir = Direction.E;
                            weight = tempWeight;
                        }
                        else if (tempWeight == weight)
                        { 
                            tempDir = Direction.E;
                            weight = tempWeight;
                        }
                        tempWeight = 0;
                        break;
                    case Direction.SE:
                        for (int i = 2; i < 5; i++)
                            if (isBool(4, i))
                                tempWeight++;
                        for (int i = 3; i < 5; i++)
                            if (isBool(3, i))
                                tempWeight++;
                        if (isBool(2, 4))
                            tempWeight++;
                        if (tempWeight > weight)
                        {
                            tempDir = Direction.SE;
                            weight = tempWeight;
                        }
                        else if (tempWeight == weight)
                            if (tempDir != Direction.E)
                            {
                                tempDir = Direction.SE;
                                weight = tempWeight;
                            }
                        tempWeight = 0;
                        break;
                    case Direction.S:
                        for (int i = 1; i < 4; i++)
                            if (isBool(3, i))
                                tempWeight++;
                        for (int i = 1; i < 4; i++)
                            if (isBool(4, i))
                                tempWeight++;
                        if (tempWeight > weight)
                        {
                            tempDir = Direction.S;
                            weight = tempWeight;
                        }
                        tempWeight = 0;
                        break;
                    case Direction.SW:
                        for (int i = 0; i < 3; i++)
                            if (isBool(4, i))
                                tempWeight++;
                        for (int i = 0; i < 2; i++)
                            if (isBool(3, i))
                                tempWeight++;
                        if (isBool(2, 0))
                            tempWeight++;
                        if (tempWeight > weight)
                        {
                            tempDir = Direction.SW;
                            weight = tempWeight;
                        }
                        tempWeight = 0;
                        break;
                    case Direction.W:
                        for (int i = 1; i < 4; i++)
                            if (isBool(i, 0))
                                tempWeight++;
                        for (int i = 1; i < 4; i++)
                            if (isBool(i, 1))
                                tempWeight++;
                        if (tempWeight > weight)
                        {
                            tempDir = Direction.W;
                            weight = tempWeight;
                        }
                        tempWeight = 0;
                        break;
                    case Direction.None:
                        if (weight == 0)
                            tempDir = Direction.None;
                        break;
                }
            }
            return tempDir;
        }
        /*
         * Function: fibonacciDirection
         * 
         * Function returns n-th step
         * of golden spiral movement
         * 
         */
         Direction fibonacciDirection(int n)
        {
            double gr = (1 + Math.Sqrt(5)) / 2;   // Golden ratio
            int n_top = n;
            int n_bot = 1;
            int n_mid = (n_top + n_bot) / 2;
            int sum;

            while(true)
            {
                sum = (int) ((1 / Math.Sqrt(5) * Math.Pow(gr,n_mid) - 1 / Math.Sqrt(5) * Math.Pow(((1 - Math.Sqrt(5) / 2)), n_mid)) * gr);
                if (n_top - n_bot <= 1 || sum == n)
                {
                    sum = n_top % 8;
                    break;
                }
                else if (sum > n)
                {
                    n_top = n_mid;
                    n_mid = (n_top + n_bot) / 2;
                }
                else
                {
                    n_bot = n_mid;
                    n_mid = (n_top + n_bot) / 2;
                }
            }
            switch(sum)
            {
                case 0:
                    return Direction.N;
                case 1:
                    return Direction.NE;
                case 2:
                    return Direction.E;
                case 3:
                    return Direction.SE;
                case 4:
                    return Direction.S;
                case 5:
                    return Direction.SW;
                case 6:
                    return Direction.W;
                case 7:
                    return Direction.NW;
                default:
                    return Direction.None;
            }
        }
    }
}
