using PKHeX.Core;

namespace pkBV
{
    /// <summary>
    /// Utility logic for parsing
    /// </summary>
    public static class BVidUtil
    {
        public static readonly byte[][] movedata = new byte[622][];

        static BVidUtil()
        {
            // Fetch Move Data
            for (int i = 0; i < 622; i++)
                movedata[i] = (byte[])Properties.Resources.ResourceManager.GetObject($"move_{i:000}");
        }

        public static void ReorderTeams(byte order, int style, PKM[][] teams, string[] trainers)
        {
            if ((order & 0xA0) == 0xA0 && style == 4)
            {
                for (int p = 0; p < 6; p++)
                    PerformSwap(ref teams[0][p], ref teams[2][p]);

                var t0 = trainers[0];
                var t2 = trainers[2];
                trainers[2] = t0;
                trainers[0] = t2;
            }
            else if ((order & 0xA0) == 0xA0)
            {
                for (int p = 0; p < 6; p++)
                    PerformSwap(ref teams[0][p], ref teams[1][p]);

                var t0 = trainers[0];
                var t1 = trainers[1];
                trainers[1] = t0;
                trainers[0] = t1;
            }
        }

        /// <summary>
        /// Rotate Pokemon around the field.
        /// </summary>
        /// <param name="team">Entire Team; first 3 are rotated.</param>
        /// <param name="count">Times to rotate Right; Left = 2 (== -1)</param>
        public static void PerformRotation(PKM[] team, int count)
        {
            for (int i = 0; i < count; i++)
            {
                PKM t0 = team[0];
                PKM t1 = team[1];
                PKM t2 = team[2];

                team[0] = t1;
                team[1] = t2;
                team[2] = t0;
            }
        }

        /// <summary>
        /// Swap positions of Participating Pokemon.
        /// </summary>
        /// <param name="a1">Pokemon to switch out.</param>
        /// <param name="a2">Pokemon to switch in.</param>
        public static void PerformSwap(ref PKM a1, ref PKM a2)
        {
            PKM t1 = a1;
            a1 = a2;
            a2 = t1;
        }
    }
}