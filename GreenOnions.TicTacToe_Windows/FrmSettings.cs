using System.ComponentModel;

namespace GreenOnions.TicTacToe_Windows
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            InitializeComponent();

            txbStartTicTacToeCmd.Text = TicTacToeConfig.StartTicTacToeCmd;
            txbTicTacToeStartedReply.Text = TicTacToeConfig.TicTacToeStartedReply;
            txbTicTacToeAlreadyStartReply.Text = TicTacToeConfig.TicTacToeAlreadyStartReply;
            txbStopTicTacToeCmd.Text = TicTacToeConfig.StopTicTacToeCmd;
            txbTicTacToeStoppedReply.Text = TicTacToeConfig.TicTacToeStoppedReply;
            txbTicTacToeAlreadStopReply.Text = TicTacToeConfig.TicTacToeAlreadStopReply;
            txbTicTacToeTimeoutReply.Text = TicTacToeConfig.TicTacToeTimeoutReply;
            txbTicTacToePlayerWinReply.Text = TicTacToeConfig.TicTacToePlayerWinReply;
            txbTicTacToeBotWinReply.Text = TicTacToeConfig.TicTacToeBotWinReply;
            txbTicTacToeDrawReply.Text = TicTacToeConfig.TicTacToeDrawReply;
            txbTicTacToeNoMoveReply.Text = TicTacToeConfig.TicTacToeNoMoveReply;
            txbTicTacToeIllegalMoveReply.Text = TicTacToeConfig.TicTacToeIllegalMoveReply;
            txbTicTacToeMoveFailReply.Text = TicTacToeConfig.TicTacToeMoveFailReply;
            foreach (CheckBox moveMode in pnlTicTacToeMoveMode.Controls.OfType<CheckBox>())
                moveMode.Checked = (TicTacToeConfig.TicTacToeMoveMode & Convert.ToInt32(moveMode.Tag)) != 0;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            TicTacToeConfig.StartTicTacToeCmd = txbStartTicTacToeCmd.Text;
            TicTacToeConfig.TicTacToeStartedReply = txbTicTacToeStartedReply.Text;
            TicTacToeConfig.TicTacToeAlreadyStartReply = txbTicTacToeAlreadyStartReply.Text;
            TicTacToeConfig.StopTicTacToeCmd = txbStopTicTacToeCmd.Text;
            TicTacToeConfig.TicTacToeStoppedReply = txbTicTacToeStoppedReply.Text;
            TicTacToeConfig.TicTacToeAlreadStopReply = txbTicTacToeAlreadStopReply.Text;
            TicTacToeConfig.TicTacToeTimeoutReply = txbTicTacToeTimeoutReply.Text;
            TicTacToeConfig.TicTacToePlayerWinReply = txbTicTacToePlayerWinReply.Text;
            TicTacToeConfig.TicTacToeBotWinReply = txbTicTacToeBotWinReply.Text;
            TicTacToeConfig.TicTacToeDrawReply = txbTicTacToeDrawReply.Text;

            TicTacToeConfig.TicTacToeNoMoveReply = txbTicTacToeNoMoveReply.Text;
            TicTacToeConfig.TicTacToeIllegalMoveReply = txbTicTacToeIllegalMoveReply.Text;
            TicTacToeConfig.TicTacToeMoveFailReply = txbTicTacToeMoveFailReply.Text;
            TicTacToeConfig.TicTacToeMoveMode = 0;
            foreach (CheckBox moveMode in pnlTicTacToeMoveMode.Controls.OfType<CheckBox>())
            {
                if (moveMode.Checked)
                    TicTacToeConfig.TicTacToeMoveMode |= Convert.ToInt32(moveMode.Tag);
            }

            base.OnClosing(e);
        }
    }
}
