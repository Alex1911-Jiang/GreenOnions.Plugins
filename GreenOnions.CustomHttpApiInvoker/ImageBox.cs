namespace GreenOnions.CustomHttpApiInvoker
{
    public partial class ImageBox : Form
    {
        private ImageBox(Image img, string caption)
        {
            InitializeComponent();
            Text = caption;
            BackgroundImage = img;
        }

        public static void Show(Image img, string caption = "")
        {
            ImageBox box = new ImageBox(img, caption);
            int borderWidth = box.Width - box.ClientRectangle.Width;
            int borderHeight = box.Height - box.ClientRectangle.Height;
            box.Size = new Size(img.Width + borderWidth, img.Height + borderHeight);
            box.ShowDialog();
        }
    }
}
