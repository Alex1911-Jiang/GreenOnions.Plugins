using System.ComponentModel;
using System.Data;

namespace GreenOnions.ReplierWindows
{
    public partial class FrmSetting : Form
    {
        private string _pluginPath;
        private string _ImagePath;
        private List<CommandSetting> _commandTable;

        public FrmSetting(List<CommandSetting> commandTable, string pluginPath)
        {
            _commandTable = commandTable;
            _pluginPath = pluginPath;
            if (!Directory.Exists(_pluginPath))
                Directory.CreateDirectory(_pluginPath);
            _ImagePath = Path.Combine(_pluginPath, "Images");
            if (!Directory.Exists(_ImagePath))
                Directory.CreateDirectory(_ImagePath);

            InitializeComponent();

            imageList.ImageSize = new Size(100, 100);

            string[] imgFiles = Directory.GetFiles(_ImagePath);
            for (int i = 0; i < imgFiles.Length; i++)
            {
                AddImage(imgFiles[i], i);
            }

            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Message");
            dtSource.Columns.Add("MatchMode");
            dtSource.Columns.Add("TriggerMode");
            dtSource.Columns.Add("ReplyValue");
            dtSource.Columns.Add("Priority");
            dtSource.Columns.Add("ReplyMode");
            for (int i = 0; i < commandTable.Count; i++)
            {
                List<string> strTriggers = new List<string>();
                if ((commandTable[i].TriggerMode & TriggerModes.私聊) != 0)
                    strTriggers.Add("私聊");
                if ((commandTable[i].TriggerMode & TriggerModes.群组) != 0)
                    strTriggers.Add("群组");
                dtSource.Rows.Add(commandTable[i].Message, commandTable[i].MatchMode, $"{string.Join('/', strTriggers)}消息", commandTable[i].ReplyValue, commandTable[i].Priority, commandTable[i].ReplyMode);
            }
            dgvReplies.DataSource = dtSource;
        }

        private void AddImage(string fileName, int index)
        {
            using (Bitmap bitmap = new Bitmap(fileName))
            {
                Bitmap bmp = new Bitmap(bitmap);
                imageList.Images.Add(bmp);
                lvImages.Items.Add(Path.GetFileName(fileName), index);
                lvImages.Items[index].ImageIndex = index;
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Filter = "图片|*.bmp;*.dib;*.jpg;*.jpe;*.jpeg;*.jfif;*.png;*.tif;*.tiff;*.gif;*.heic;";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < ofd.FileNames.Length; i++)
                    {
                        string poyiedName = Path.Combine(_ImagePath, Path.GetFileName(ofd.FileNames[i]));
                        File.Copy(ofd.FileNames[i], poyiedName, true);
                        AddImage(poyiedName, lvImages.Items.Count);
                    }
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            btnAddImage.Focus();
            if (dgvReplies.DataSource is DataTable dt)
            {
                _commandTable.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    _commandTable.Add(new CommandSetting
                    {
                        Message = row[0].ToString()!,
                        MatchMode = ToMatchMode(row[1]),
                        TriggerMode = ToTriggerMode(row[2]),
                        ReplyValue = row[3].ToString()!,
                        Priority = ToInt32(row[4]),
                        ReplyMode = ToBoolean(row[5]),
                    });
                }
            }
            base.OnClosing(e);
        }

        private bool ToBoolean(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return false;
            if (bool.TryParse(obj.ToString(), out bool result))
                return result;
            return false;
        }
        private int ToInt32(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;
            if (int.TryParse(obj.ToString(), out int result))
                return result;
            return 0;
        }
        private MatchModes ToMatchMode(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return MatchModes.完全;
            if (Enum.TryParse(obj.ToString(), out MatchModes result))
                return result;
            return MatchModes.完全;
        }
        private TriggerModes ToTriggerMode(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return TriggerModes.私聊;
            TriggerModes triggerMode = 0;
            string[] strValues = obj.ToString()!.Replace("消息", "").Split("/");
            foreach (string strValue in strValues)
            {
                if (Enum.TryParse(strValue, out TriggerModes result))
                    triggerMode |= result;
            }
            return triggerMode;
        }

        private void dgvReplies_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (dgv.CurrentCell.OwningColumn is DataGridViewButtonColumn btnCol)
                {
                    if (btnCol.HeaderText == "删除")
                    {
                        if (dgvReplies.DataSource is DataTable dt)
                        {
                            dt.Rows.RemoveAt(e.RowIndex);
                            dt.AcceptChanges();
                        }
                    }
                }
            }
        }

        private void MenuItemRemove_Click(object sender, EventArgs e)
        {
            int index = lvImages.SelectedItems[0].Index;
            string name = lvImages.SelectedItems[0].Text;
            imageList.Images.RemoveAt(index);
            lvImages.Items.RemoveAt(index);
            for (int i = 0; i < lvImages.Items.Count; i++)
                lvImages.Items[i].ImageIndex = i;
            string imageFileName = Path.Combine(_ImagePath, name);
            if (File.Exists(imageFileName))
                File.Delete(imageFileName);
        }

        private void lvImages_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                lvImages.ContextMenuStrip = null;
                if (lvImages.SelectedItems.Count > 0)
                    contextMenuStrip.Show(lvImages, new Point(e.X, e.Y));
            }
        }
    }
}
