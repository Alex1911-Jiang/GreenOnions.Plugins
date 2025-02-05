using Lagrange.Core.Message;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GreenOnions.NT.TicTacToe
{
    internal class Session : IDisposable
    {
        private int[,] _data = new int[3, 3];  //井字棋棋子位置数据, 0为未下子, 1为玩家下子, -1为机器人下子
        private Image<Rgb24> _lastImage = TicTacToeExtensions.CreateChessboard();

        internal Session(MessageChain chain, Config config)
        {
            Chain = chain;
            TimeOut = DateTime.Now.AddSeconds(config.TimeoutSecond);
        }

        internal MessageChain Chain { get; private set; }
        internal DateTime TimeOut { get; set; }

        internal async Task<byte[]> GetImageBytes()
        {
            return await _lastImage.ToBytesArrayAsync();
        }

        /// <summary>
        /// 玩家通过图片下子
        /// </summary>
        internal MoveValidities PlayerMove(byte[] inputImage)
        {
            using Image img = Image.Load(inputImage);

            Image<Rgb24> playerStepImg = img.CloneAs<Rgb24>();
            Image<Rgb24> botStepImg = _lastImage;

            // 统一尺寸到300x300
            if (playerStepImg.Width != 300 || playerStepImg.Height != 300)
            {
                playerStepImg.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(300, 300),
                    Mode = ResizeMode.Stretch,
                    Sampler = KnownResamplers.Lanczos3
                }));
            }

            var diffWeights = new Dictionary<Point, int>();
            for (int y = 0; y < 300; y++)
            {
                if (y > 98 && y < 102) 
                    continue;
                if (y > 198 && y < 202)
                    continue;

                for (int x = 0; x < 300; x++)
                {
                    if (x > 98 && x < 102) 
                        continue;
                    if (x > 198 && x < 202)
                        continue;

                    var botPixel = botStepImg[x, y];
                    var playerPixel = playerStepImg[x, y];

                    int botGray = (int)(botPixel.R * 0.3 + botPixel.G * 0.59 + botPixel.B * 0.11);
                    int playerGray = (int)(playerPixel.R * 0.3 + playerPixel.G * 0.59 + playerPixel.B * 0.11);
                    int diff = Math.Abs(botGray - playerGray);

                    if (diff > 50)  //差异灰度值超过50
                    {
                        var gridPos = new Point(x / 100, y / 100);
                        if (diffWeights.ContainsKey(gridPos))
                        {
                            diffWeights[gridPos]++;
                        }
                        else
                        {
                            diffWeights.Add(gridPos, 1);
                        }
                    }
                }
            }

            // 过滤绘制面积大于单元格平方数的5%认为有效
            Dictionary<Point, int> moveds = diffWeights.Where(kv => kv.Value > 100 * 100 * 0.05).ToDictionary(kv => kv.Key, kv => kv.Value);
            if (moveds.Count == 0)
            {
                return MoveValidities.Invalid;  // 没有落子
            }
            if (moveds.Count > 1)
            {
                return MoveValidities.MultiSelection; // 落子超过1个
            }
            Point moved = moveds.Single().Key;
            if (_data[moved.X, moved.Y] != 0)
            {
                return MoveValidities.Occupied; // 在已有棋子的格子落子
            }

            _data[moved.X, moved.Y] = 1;
            _lastImage = TicTacToeExtensions.CreateChessboard().DrawPiece(_data);
            return MoveValidities.Valid;
        }

        /// <summary>
        /// 获取胜负情况
        /// </summary>
        internal ScoreTypes GetScore()
        {
            (ScoreTypes scoreX, int resultX) = CheckScoreX();  //在横线上
            if (scoreX != ScoreTypes.NoResult)
            {
                _lastImage = TicTacToeExtensions.CreateChessboard().DrawPiece(_data).DrawWinLine(resultX, -1);
                return scoreX;
            }

            (ScoreTypes scoreY, int resultY) = CheckScoreY();  //在竖线上
            if (scoreY != ScoreTypes.NoResult)
            {
                _lastImage = TicTacToeExtensions.CreateChessboard().DrawPiece(_data).DrawWinLine(-1, resultY);
                return scoreY;
            }

            (ScoreTypes scoreXY, int resultXY) = CheckScoreXY();  //在斜线上
            if (scoreXY != ScoreTypes.NoResult)
            {
                _lastImage = TicTacToeExtensions.CreateChessboard().DrawPiece(_data).DrawWinLine(resultXY, resultXY);
                return scoreXY;
            }

            bool lastOneGrid = OnlyOneGrid(out int lastOneX, out int lastOneY);
            if (!lastOneGrid)
                return ScoreTypes.NoResult;

            //只剩最后一个格子，帮玩家向最终格子下子后判断胜负
            _data[lastOneX, lastOneY] = 1;
            ScoreTypes lastScore = GetScore();
            if (lastScore != ScoreTypes.NoResult)
                return lastScore;

            _data[lastOneX, lastOneY] = 0;
            return ScoreTypes.Draw;
        }

        internal void ComputerMove()
        {
            Point moved = ComputerMoveInner();
            _data[moved.X, moved.Y] = -1;
            _lastImage = TicTacToeExtensions.CreateChessboard().DrawPiece(_data);
        }

        private Point ComputerMoveInner()
        {
            var computerChance = ComputerFindChance(true);
            if (computerChance != null)
                return new Point(computerChance.Value.X, computerChance.Value.Y);  //优先查找电脑获胜的机会

            var playerChance = ComputerFindChance(false);
            if (playerChance != null)
                return new Point(playerChance.Value.X, playerChance.Value.Y);  //随后查找阻止玩家获胜的机会

            //如果第一步用户是下在角落，则抢对角线
            Point[] diagonals = [new Point(0, 0), new Point(0, 2), new Point(2, 0), new Point(2, 2)];
            if (diagonals.Count(d => _data[d.X, d.Y] == 1) == 1 && diagonals.Count(d => _data[d.X, d.Y] == 0) == 3)
            {
                return diagonals.Single(d => _data[d.X, d.Y] == 1) switch
                {
                    (0, 0) => new Point(2, 2),
                    (2, 2) => new Point(0, 0),
                    (0, 2) => new Point(2, 0),
                    (2, 0) => new Point(0, 2),
                    _ => throw new Exception(),
                };
            }

            //如果第一步用户是下在中心，则随机抢四角
            if (_data[1, 1] == 1 && _data[0, 0] == 0 && _data[0, 2] == 0 && _data[2, 0] == 0 && _data[2, 2] == 0)
                return diagonals[new Random(Guid.NewGuid().GetHashCode()).Next(0, diagonals.Length)];

            //如果用户第一步下在行列中，则抢中心
            if (_data[1, 1] == 0)
                return new Point(1, 1);

            //如果还有对角线是空的，先抢对角线
            if (_data[0, 0] == 0 && _data[2, 2] == 0)
            {
                Point[] diagonal = [new Point(0, 0), new Point(2, 2)];
                return diagonal[new Random(Guid.NewGuid().GetHashCode()).Next(0, diagonal.Length)];
            }
            if (_data[2, 0] == 0 && _data[0, 2] == 0)
            {
                Point[] diagonal = [new Point(2, 0), new Point(0, 2)];
                return diagonal[new Random(Guid.NewGuid().GetHashCode()).Next(0, diagonal.Length)];
            }

            //都不符合条件的时候随机下子
            List<Point> emptyGrid = new List<Point>();
            for (int x = 0; x < _data.GetLength(0); x++)
            {
                for (int y = 0; y < _data.GetLength(1); y++)
                {
                    if (_data[x, y] != 0)
                        continue;
                    emptyGrid.Add(new Point(x, y));
                }
            }
            Random random = new Random(Guid.NewGuid().GetHashCode());
            return emptyGrid[random.Next(0, emptyGrid.Count)];
        }

        private Point? ComputerFindChance(bool winChance)
        {
            int[] row = new int[3];  // —
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                    row[x] = _data[x, y];
                if (Chance(row, winChance))
                    return new Point(Array.IndexOf(row, 0), y);
            }

            int[] column = new int[3];  // |
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                    column[y] = _data[x, y];
                if (Chance(column, winChance))
                    return new Point(x, Array.IndexOf(column, 0));
            }

            int[] slash = new int[3]; // /
            for (int i = 0; i < 3; i++)
            {
                int x = 2 - i;
                slash[i] = _data[x, i];
            }
            if (Chance(slash, winChance))
            {
                int index = Array.IndexOf(slash, 0);
                return new Point(2 - index, index);
            }

            int[] backslash = new int[3]; // \
            for (int i = 0; i < 3; i++)
                backslash[i] = _data[i, i];
            if (Chance(backslash, winChance))
            {
                int index = Array.IndexOf(backslash, 0);
                return new Point(index, index);
            }

            return null;

            bool Chance(int[] sourece, bool isComputer)
            {
                if (!sourece.Contains(0))  //没空格子了
                    return false;
                int sum = sourece.Sum();
                if (isComputer && sum == -2)
                    return true;
                if (!isComputer && sum == 2)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 检查是否只剩一个格子
        /// </summary>
        private bool OnlyOneGrid(out int lastOneX, out int lastOneY)
        {
            int zeroCount = 0;
            lastOneX = -1;
            lastOneY = -1;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (_data[x, y] == 0)
                    {
                        zeroCount++;
                        lastOneX = x;
                        lastOneY = y;
                    }
                }
            }
            return zeroCount == 1;
        }

        /// <summary>
        /// 检查水平方向得分
        /// </summary>
        private (ScoreTypes score, int lineX) CheckScoreX()
        {
            int? camp = null;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (x == 0)
                        camp = _data[x, y];
                    else if (_data[x, y] == 0 || _data[x, y] != camp)
                        goto IL_Next;
                }
                return (CampToScore(camp), y);
            IL_Next:;
            }
            return (ScoreTypes.NoResult, default);
        }

        /// <summary>
        /// 检查垂直方向得分
        /// </summary>
        private (ScoreTypes score, int lineY) CheckScoreY()
        {
            int? camp = null;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (y == 0)
                        camp = _data[x, y];
                    else if (_data[x, y] == 0 || _data[x, y] != camp)
                        goto IL_Next;
                }
                return (CampToScore(camp), x);
            IL_Next:;
            }
            return (ScoreTypes.NoResult, default);
        }

        /// <summary>
        /// 检查斜线方向得分
        /// </summary>
        private (ScoreTypes score, int lineXY) CheckScoreXY()
        {
            int? camp = null;
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                    camp = _data[i, i];
                else if (_data[i, i] == 0 || _data[i, i] != camp)
                    goto IL_Next;
            }
            return (CampToScore(camp), 1);  // \
        IL_Next:;
            for (int i = 0; i < 3; i++)
            {
                int x = 2 - i;
                if (i == 0)
                    camp = _data[x, i];
                else if (_data[x, i] == 0 || _data[x, i] != camp)
                    goto IL_0;
            }
            return (CampToScore(camp), -1);  // /
        IL_0:;
            return (ScoreTypes.NoResult, default);
        }

        internal ScoreTypes CampToScore(int? camp)
        {
            return camp switch
            {
                1 => ScoreTypes.PlayerWin,
                -1 => ScoreTypes.ComputeWin,
                _ => ScoreTypes.NoResult,
            };
        }

        public void Dispose()
        {
            _lastImage?.Dispose();
        }
    }

    public enum MoveValidities
    {
        /// <summary>
        /// 有效落子
        /// </summary>
        Valid = 0,
        /// <summary>
        /// 无效落子（没有落子或落在网格上）
        /// </summary>
        Invalid = 1,
        /// <summary>
        /// 多个落子
        /// </summary>
        MultiSelection = 2,
        /// <summary>
        /// 在已有棋子的格子上落子
        /// </summary>
        Occupied = 3,
    }

    public enum ScoreTypes
    {
        /// <summary>
        /// 未分出胜负
        /// </summary>
        NoResult = 0,
        /// <summary>
        /// 平局
        /// </summary>
        Draw = 1,
        /// <summary>
        /// 玩家获胜
        /// </summary>
        PlayerWin = 2,
        /// <summary>
        /// 电脑获胜
        /// </summary>
        ComputeWin = 3,
    }
}