using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GreenOnions.NT.TicTacToe
{
    internal static class TicTacToeExtensions
    {
        internal static Image<Rgb24> CreateChessboard()
        {
            var image = new Image<Rgb24>(300, 300);
            image.Mutate(x => x
                .Fill(Color.White)

                .DrawLine(Color.Black, 1, new PointF(0, 100), new PointF(300, 100))
                .DrawLine(Color.Black, 1, new PointF(0, 200), new PointF(300, 200))

                .DrawLine(Color.Black, 1, new PointF(100, 0), new PointF(100, 300))
                .DrawLine(Color.Black, 1, new PointF(200, 0), new PointF(200, 300))
                );
            return image;
        }

        internal static Image<Rgb24> DrawPiece(this Image<Rgb24> chessboard, int[,] data)
        {
            chessboard.Mutate(ctx =>
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        int centerX = x * 100 + 50;
                        int centerY = y * 100 + 50;

                        switch (data?[x, y])
                        {
                            case -1:
                                DrawO(ctx, centerX, centerY);
                                break;
                            case 1:
                                DrawX(ctx, centerX, centerY);
                                break;
                        }
                    }
                }
            });

            return chessboard;

            void DrawX(IImageProcessingContext ctx, int loactionX, int locationY)
            {
                ctx.DrawLine(Color.Black, 4, new PointF(loactionX - 30, locationY - 30), new PointF(loactionX + 30, locationY + 30));
                ctx.DrawLine(Color.Black, 4, new PointF(loactionX - 30, locationY + 30), new PointF(loactionX + 30, locationY - 30));
            }

            void DrawO(IImageProcessingContext ctx, int loactionX, int locationY)
            {
                var circle = new EllipsePolygon(loactionX, locationY, 60, 60);
                ctx.Draw(Color.Black, 4, circle);
            }
        }

        internal static Image<Rgb24> DrawWinLine(this Image<Rgb24> chessboard, int x, int y)
        {
            if (x == -1 && y == -1) // /
                chessboard.Mutate(img => img.DrawLine(Color.Red, 6, new PointF(280, 20), new PointF(20, 280)));
            else if (x == 1 && y == 1) // \
                chessboard.Mutate(img => img.DrawLine(Color.Red, 6, new PointF(20, 20), new PointF(280, 280)));
            else if (x == -1)  // |
                chessboard.Mutate(img => img.DrawLine(Color.Red, 6, new PointF(y * 100 + 50, 10), new PointF(y * 100 + 50, 290)));
            else if (y == -1)  // —
                chessboard.Mutate(img => img.DrawLine(Color.Red, 6, new PointF(10, x * 100 + 50), new PointF(290, x * 100 + 50)));

            return chessboard;
        }

        internal static async Task<byte[]> ToBytesArrayAsync(this Image<Rgb24> img)
        {
            using MemoryStream ms = new MemoryStream();
            await img.SaveAsBmpAsync(ms);
            return ms.ToArray();
        }
    }
}
