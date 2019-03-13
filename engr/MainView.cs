using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace engr
{
    public partial class MainView : Form
    {
        canvasController.CanvasController _canvasController;
        agentController.AgentController _agentController;
        string[] _destPic;
        BackgroundWorker _simulationThread;
        public MainView()
        {
            InitializeComponent();
            _canvasController = new canvasController.CanvasController(ref canvas);
            _simulationThread = new BackgroundWorker();
            _simulationThread.DoWork += SimulationThread_DoWork;
            _simulationThread.WorkerSupportsCancellation = true;
        }

        private void SimulationThread_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                if (_simulationThread.CancellationPending)
                    break;
                System.Threading.Thread.Sleep(500);
                if (_simulationThread.CancellationPending)
                    break;
                _agentController.moveAgents();
                if (_simulationThread.CancellationPending)
                    break;
                Invoke(new Action(() =>
                {
                    _canvasController.refreshCanvas();
                }));
                if (_simulationThread.CancellationPending)
                    break;
            }
        }

        private void choosePicButt_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                string file = openFileDialog.FileName;
                try
                {
                    _destPic = System.IO.File.ReadAllLines(file);
                }
                catch(InvalidOperationException)
                {
                    return;
                }
            }
            else
            {
                return;
            }
            return;
        }

        private void refreshButt_Click(object sender, EventArgs e)
        {
            int agentNo;
            if(agentNoBox.Text != null)
            {
                Int32.TryParse(agentNoBox.Text, out agentNo);
                _agentController = new agentController.AgentController(ref _canvasController._canvasMatrix, agentNo, _canvasController._canvasMatrix._row, _canvasController._canvasMatrix._col);
            }
            if(_destPic != null)
                _canvasController.setDestPic(_destPic);
            _canvasController.refreshCanvas();
            return;
        }

        private void startButt_Click(object sender, EventArgs e)
        {
            _simulationThread.RunWorkerAsync();
        }

        private void stopButt_Click(object sender, EventArgs e)
        {
            _simulationThread.CancelAsync();
        }
    }
}
