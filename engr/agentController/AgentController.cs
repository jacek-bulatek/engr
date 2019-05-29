using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engr.agentController
{
    delegate Agent CollisionEventHandler(int row, int col);
    /*
     * The point of this delegate is to open
     * the communication stream between two
     * agents.
     */ 
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
            _matrix.clearRobotMatrix();
            _agentNo = agentNo;
            _agentSet = new Agent[_agentNo];
            for (int i = 0; i < _agentNo; i++)
            {
                _agentSet[i] = new Agent(ref _matrix, rowNo, colNo);
            }
        }
        /*
         * Function: moveAgents
         * 
         * Function moves each agent separately
         * 
         */
        public void moveAgents()
        {
            foreach (Agent agent in _agentSet)
            {
                agent.move();
            }
        }
    }
}
