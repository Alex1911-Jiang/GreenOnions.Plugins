using System.ComponentModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace GreenOnions.TicTacToe_Windows
{
    public partial class FrmSettings : Form
    {
        private TicTacToeConfig _config;
        private string _configFileName;
        public FrmSettings(TicTacToeConfig config, string _pluginPath)
        {
            _config = config;
            _configFileName = Path.Combine(_pluginPath, "config.json");
            InitializeComponent();

            txbStartTicTacToeCmd.Text = _config.StartTicTacToeCmd;
            txbTicTacToeStartedReply.Text = _config.TicTacToeStartedReply;
            txbTicTacToeAlreadyStartReply.Text = _config.TicTacToeAlreadyStartReply;
            txbStopTicTacToeCmd.Text = _config.StopTicTacToeCmd;
            txbTicTacToeStoppedReply.Text = _config.TicTacToeStoppedReply;
            txbTicTacToeAlreadStopReply.Text = _config.TicTacToeAlreadStopReply;
            txbTicTacToeTimeoutReply.Text = _config.TicTacToeTimeoutReply;
            txbTicTacToePlayerWinReply.Text = _config.TicTacToePlayerWinReply;
            txbTicTacToeBotWinReply.Text = _config.TicTacToeBotWinReply;
            txbTicTacToeDrawReply.Text = _config.TicTacToeDrawReply;
            txbTicTacToeNoMoveReply.Text = _config.TicTacToeNoMoveReply;
            txbTicTacToeIllegalMoveReply.Text = _config.TicTacToeIllegalMoveReply;
            txbTicTacToeMoveFailReply.Text = _config.TicTacToeMoveFailReply;
            foreach (CheckBox moveMode in pnlTicTacToeMoveMode.Controls.OfType<CheckBox>())
                moveMode.Checked = (_config.TicTacToeMoveMode & (TicTacToeMoveMode)Convert.ToInt32(moveMode.Tag)) != 0;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _config.StartTicTacToeCmd = txbStartTicTacToeCmd.Text;
            _config.TicTacToeStartedReply = txbTicTacToeStartedReply.Text;
            _config.TicTacToeAlreadyStartReply = txbTicTacToeAlreadyStartReply.Text;
            _config.StopTicTacToeCmd = txbStopTicTacToeCmd.Text;
            _config.TicTacToeStoppedReply = txbTicTacToeStoppedReply.Text;
            _config.TicTacToeAlreadStopReply = txbTicTacToeAlreadStopReply.Text;
            _config.TicTacToeTimeoutReply = txbTicTacToeTimeoutReply.Text;
            _config.TicTacToePlayerWinReply = txbTicTacToePlayerWinReply.Text;
            _config.TicTacToeBotWinReply = txbTicTacToeBotWinReply.Text;
            _config.TicTacToeDrawReply = txbTicTacToeDrawReply.Text;

            _config.TicTacToeNoMoveReply = txbTicTacToeNoMoveReply.Text;
            _config.TicTacToeIllegalMoveReply = txbTicTacToeIllegalMoveReply.Text;
            _config.TicTacToeMoveFailReply = txbTicTacToeMoveFailReply.Text;
            _config.TicTacToeMoveMode = 0;
            foreach (CheckBox moveMode in pnlTicTacToeMoveMode.Controls.OfType<CheckBox>())
            {
                if (moveMode.Checked)
                    _config.TicTacToeMoveMode |= (TicTacToeMoveMode)Convert.ToInt32(moveMode.Tag);
            }

            string strConfig = JsonSerializer.Serialize(_config, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            File.WriteAllText(_configFileName, strConfig);

            base.OnClosing(e);
        }
    }
}
