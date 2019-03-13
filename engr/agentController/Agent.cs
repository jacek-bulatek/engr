using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engr.agentController
{
    class Agent
    {
        Random genRandom;
        public int[] _coordinates { get; set; }
        public Agent(int rowNo, int colNo)
        {
            genRandom = new Random();
            _coordinates = new int[2];
            drawCoordinates(rowNo, colNo);
        }
        public void drawCoordinates(int rowNo, int colNo)
        {
            _coordinates[0] = genRandom.Next(0, rowNo);
            _coordinates[1] = genRandom.Next(0, colNo);
        }
    }
}
