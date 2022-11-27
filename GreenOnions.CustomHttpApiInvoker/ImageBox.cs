namespace GreenOnions.CustomHttpApiInvoker
{
    public partial class ImageBox : Form
    {
        private ImageBox(Image img, string caption)
        {
            InitializeComponent();
            Text = caption;
            pic.Image = img;
        }

        public static void Show(Image img, string caption = "")
        {
            ImageBox box = new ImageBox(img, caption);
            box.ShowDialog();
        }
    }
}
