using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engr.agentController
{
    class Agent
    {
        // Fields
        canvasController.CanvasMatrix _matrix;              // Whole terrain used to map surroundings - consider refactor here!
        canvasController.CanvasMatrix _surroundings;        // Map of agents and destination in sight
        int _fibStep;                                       // Number of current step in golden spiral
        Random _genRandom;                                  // Random number generator used to draw starting coordinates
        int weight;                                         // Weight of current direction

        // Types
        enum Direction { N, NE, E, SE, S, SW, W, NW, None } // Direction

        // Properties
        public int[] _coordinates { get; set; }             // Current coordinates of this agent on the whole terrain

        // Methods
        /*
         * Constructor
         */
        public Agent(ref canvasController.CanvasMatrix matrix, int rowNo, int colNo)
        {
            _matrix = matrix;
            _genRandom = new Random();
            _fibStep = 1;
            _coordinates = new int[2];
            _surroundings = new canvasController.CanvasMatrix(10, 5, 5);
            drawCoordinates(rowNo, colNo);
            updateSurroundings();
        }
        /*
         * Method: drawCoordinates
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
         * Method: move
         * 
         * Function invokes other functions
         * to determine moving direction if 
         * any and then changes coordinates
         * accordingly.
         * 
         */
        public void move()
        {
            Direction dir;
            if (_matrix.isDest(_coordinates[0], _coordinates[1]))
            {
                // Code if on destination
                dir = spreadInDest();
                return;
            }
            else
            {
                // Code if seeking destination
                dir = seekDest();
            }
            updateCoordinates(dir);
            updateSurroundings();
        }
        /*
         * Method: updateSurroundings
         * 
         * Function sync up _isRobot and
         * _isDest fields of _surroundings
         * with _matrix
         * 
         */ 
        void updateSurroundings()
        {
            _surroundings.clearDestMatrix();
            _surroundings.clearRobotMatrix();
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
         * Method: examineDirection
         * 
         * Function Checks given direction
         * returns false if given delegate
         * for this direction returns true.
         * 
         */ 
        bool examineDirection(Direction dir, Func<int, int, bool> isBool)
        {
            switch(dir)
            {
                case Direction.N:
                    if (_coordinates[0] - 1 > 0)
                        return !isBool(_coordinates[0] - 1, _coordinates[1]);
                    else
                        return false;
                case Direction.NE:
                    if (_coordinates[0] - 1 > 0 && _coordinates[1] + 1 < _matrix._col)
                        return !isBool(_coordinates[0] - 1, _coordinates[1] + 1);
                    else
                        return false;
                case Direction.E:
                    if (_coordinates[1] + 1 < _matrix._col)
                        return !isBool(_coordinates[0], _coordinates[1] + 1);
                    else
                        return false;
                case Direction.SE:
                    if (_coordinates[0] + 1 < _matrix._row && _coordinates[1] + 1 < _matrix._col)
                        return !isBool(_coordinates[0] + 1, _coordinates[1] + 1);
                    else
                        return false;
                case Direction.S:
                    if (_coordinates[0] + 1 < _matrix._row)
                        return !isBool(_coordinates[0] + 1, _coordinates[1]);
                    else
                        return false;
                case Direction.SW:
                    if (_coordinates[0] + 1 < _matrix._row && _coordinates[1] - 1 > 0)
                        return !isBool(_coordinates[0] + 1, _coordinates[1] - 1);
                    else
                        return false;
                case Direction.W:
                    if (_coordinates[1] - 1 > 0)
                        return !isBool(_coordinates[0], _coordinates[1] - 1);
                    else
                        return false;
                case Direction.NW:
                    if (_coordinates[0] - 1 > 0 && _coordinates[1] - 1 > 0)
                        return !isBool(_coordinates[0] - 1, _coordinates[1] - 1);
                    else
                        return false;
                default:
                    return false;
            }
        }
        /*
         *  Method: examineSurroundings 
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
            weight = 0;
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
         * Method: fibonacciDirection
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
        /*
         * Method: seekDest
         * 
         * Function returns direction
         * that should be taken to find
         * destination.
         * 
         */
         Direction seekDest()
        {
            Direction dir = examineSurroundings(_surroundings.isDest);  // check if destination is in sight
            if (dir == Direction.None)                                  // if it is not, proceed with golden spiral
            {
                if (_fibStep > 200)                                     // reset step counter if we did too many steps
                    _fibStep = 1;
                do
                {
                    dir = fibonacciDirection(_fibStep++);
                } while (!examineDirection(dir, _matrix.isRobot));
            }
            return dir;
        }
        /*
         * Method: updateCoordinates
         * 
         * Function updates coordinates
         * accordingly to given direction.
         * Returns flase if not possible.
         * 
         */
         bool updateCoordinates(Direction dir)
        {
            switch (dir)
            {
                case Direction.N:
                    if (!examineDirection(Direction.N, _matrix.isRobot))
                        return false;
                    _matrix.swichIsRobot(_coordinates[0]--, _coordinates[1]);
                    _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                    return true;
                case Direction.NE:
                    if (!examineDirection(Direction.NE, _matrix.isRobot))
                        return false;
                    _matrix.swichIsRobot(_coordinates[0]--, _coordinates[1]++);
                    _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                    return true;
                case Direction.E:
                    if (!examineDirection(Direction.E, _matrix.isRobot))
                        return false;
                    _matrix.swichIsRobot(_coordinates[0], _coordinates[1]++);
                    _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                    return true;
                case Direction.SE:
                    if (!examineDirection(Direction.SE, _matrix.isRobot))
                        return false;
                    _matrix.swichIsRobot(_coordinates[0]++, _coordinates[1]++);
                    _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                    return true;
                case Direction.S:
                    if (!examineDirection(Direction.S, _matrix.isRobot))
                        return false;
                    _matrix.swichIsRobot(_coordinates[0]++, _coordinates[1]);
                    _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                    return true;
                case Direction.SW:
                    if (!examineDirection(Direction.SW, _matrix.isRobot))
                        return false;
                    _matrix.swichIsRobot(_coordinates[0]++, _coordinates[1]--);
                    _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                    return true;
                case Direction.W:
                    if (!examineDirection(Direction.W, _matrix.isRobot))
                        return false;
                    _matrix.swichIsRobot(_coordinates[0], _coordinates[1]--);
                    _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                    return true;
                case Direction.NW:
                    if (!examineDirection(Direction.NW, _matrix.isRobot))
                        return false;
                    _matrix.swichIsRobot(_coordinates[0]--, _coordinates[1]--);
                    _matrix.swichIsRobot(_coordinates[0], _coordinates[1]);
                    return true;
                default:
                    return true;
            }
        }
        /*
         * Method: spreadInDest
         * 
         * Function returns direction
         * that should be taken to
         * spread inside destination
         * 
         */
         Direction spreadInDest()
         {
            Direction dir = Direction.None;
            // Check if we're on the edge
            if (checkEdge(dir))
            {
                // Check if there is available destination in direction away from the edge
                for (Direction idir = Direction.N; idir <= Direction.None; idir++)
                {
                    if (!checkEdge(idir))
                    {
                        dir = idir;
                        if (examineDirection(dir, _matrix.isRobot))
                            return dir;
                    }
                }
            } // agent is not on the edge, or cannot move away from it
            else 
            {
                Func<int, int, bool> isntRobot = (row, col) => { return !_surroundings.isRobot(row, col); };
                dir = examineSurroundings(isntRobot);
            }
            // Check if we can move away from robots whilst not going on the edge

            // Return concluded direction
            return dir;
         }
        /*
         * Method: checkEdge
         * 
         * Function checks if agent
         * moved to given direction
         * will finnd itself on the
         * edge.
         * Returns true if so and
         * false otherwise
         */
         bool checkEdge(Direction dir)
        {
            int row, col, max_row_ad, max_col_ad;       // those are values used in loop later on
                                                        // on which iteration allows to loop over
                                                        // their neighbour fields
            // depending on direction on which movement
            // is considered, acurate values are assigned
            // e. g. for 'None' direction and
            // surroundings array:
            // [0,0] [0,1] [0,2] [0,3] [0,4]
            // [1,0] [1,1] [1,2] [1,3] [1,4]
            // [2,0] [2,1] [2,2] [2,3] [2,4]
            // [3,0] [3,1] [3,2] [3,3] [3,4]
            // [4,0] [4,1] [4,2] [4,3] [4,4]
            // we will be considering closest
            // fields to field [2,2], so we will
            // be iterating from 1 to maxrow - 1
            // and from 1 to maxcol - 1
            switch (dir)
            {
                case Direction.None:
                    row = col = max_row_ad = max_col_ad = 1;    
                    break;
                case Direction.N:
                    row = 0;
                    col = max_col_ad = 1;
                    max_row_ad = 2;
                    break;
                case Direction.NE:
                    row = max_col_ad = 0;
                    col = max_row_ad = 2;
                    break;
                case Direction.E:
                    row = max_row_ad = 1;
                    max_col_ad = 0;
                    col = 2;
                    break;
                case Direction.SE:
                    row = col = 2;
                    max_col_ad = max_row_ad = 0;
                    break;
                case Direction.S:
                    row = 2;
                    col = max_col_ad = 1;
                    max_row_ad = 0;
                    break;
                case Direction.SW:
                    row = max_col_ad = 2;
                    col = max_row_ad = 0;
                    break;
                case Direction.W:
                    row = max_row_ad = 1;
                    col = 0;
                    max_col_ad = 2;
                    break;
                case Direction.NW:
                    row = col = 0;
                    max_col_ad = max_row_ad = 2;
                    break;
                default:
                    row = col = max_col_ad = max_row_ad = 0;
                    break;
            }
            for ( ; row < _surroundings._row - max_row_ad; row++)
                for( ; col < _surroundings._col - max_col_ad; col++)
                {
                    if (!_surroundings.isDest(row, col))                // One of adjusting fields is not a part of destination
                        return true;
                }
            return false;
        }
    }
}
