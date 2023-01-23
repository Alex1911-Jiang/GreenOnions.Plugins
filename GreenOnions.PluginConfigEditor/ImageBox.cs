namespace GreenOnions.PluginConfigEditor
{
    public partial class ImageBox : Form
    {
        private ImageBox(Image img, string caption, Size size)
        {
            InitializeComponent();
            Text = caption;
            BackgroundImage = img;

            int borderWidth = Width - ClientRectangle.Width;
            int borderHeight = Height - ClientRectangle.Height;
            if (size == Size.Empty)
                Size = new Size(img.Width + borderWidth, img.Height + borderHeight);
            else
                Size = new Size(size.Width + borderWidth, size.Height + borderHeight);
        }

        public static void Show(Image img)
        {
            ImageBox box = new ImageBox(img, "", Size.Empty);
            box.ShowDialog();
        }

        public static void Show(Image img, string caption)
        {
            ImageBox box = new ImageBox(img, caption, Size.Empty);
            box.ShowDialog();
        }

        public static void Show(Image img, string caption, int width, int height)
        {
            ImageBox box = new ImageBox(img, caption, new Size(width,height));
            box.ShowDialog();
        }
    }
}
