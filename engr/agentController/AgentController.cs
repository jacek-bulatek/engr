using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engr.agentController
{
    class AgentController
    {
        int _agentNo;
        Random genRandom;
        Agent[] _agentSet;
        canvasController.CanvasMatrix _matrix;

        public AgentController(ref canvasController.CanvasMatrix matrix, int agentNo, int rowNo, int colNo)
        {
            genRandom = new Random();
            _matrix = matrix;
            _matrix.clearIsRobot();
            _agentNo = agentNo;
            _agentSet = new Agent[_agentNo];
            for (int i = 0; i < _agentNo; i++)
            {
                _agentSet[i] = new Agent(rowNo, colNo);
                while (true)
                {
                    if (_matrix.isRobot(_agentSet[i]._coordinates[0], _agentSet[i]._coordinates[1]))
                    {
                        _agentSet[i].drawCoordinates(rowNo, colNo);
                    }
                    else
                    {
                        _matrix.setRobot(_agentSet[i]._coordinates[0], _agentSet[i]._coordinates[1]);
                        break;
                    }
                }
            }
        }
        public void moveAgents()
        {
            foreach (Agent agent in _agentSet)
            {
                int dirRow;
                int dirCol;
                do
                {
                    dirRow = genRandom.Next(-1, 2);     // -1 - left, 1 - right, 0 - stay
                    dirCol = genRandom.Next(-1, 2);     // -1 - down, 1 - up, 0 stay
                    // Check if moved out of canvas:
                    if (agent._coordinates[0] + dirRow >= _matrix._row || agent._coordinates[0] + dirRow < 0 || agent._coordinates[1] + dirCol >= _matrix._col || agent._coordinates[1] + dirCol < 0)
                        dirCol = dirRow = 0;            // if yes, staying is not possible;
                } while (_matrix.isRobot(agent._coordinates[0] + dirRow, agent._coordinates[1] + dirCol));
                _matrix.swichIsRobot(agent._coordinates[0], agent._coordinates[1]);
                agent._coordinates[0] += dirRow;
                agent._coordinates[1] += dirCol;
                _matrix.swichIsRobot(agent._coordinates[0], agent._coordinates[1]);
            }
        }
    }
}
