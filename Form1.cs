using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PKHeX.Core;
using PKHeX.WinForms;

namespace pkBV
{
    public sealed partial class Form1 : Form
    {
        public Form1()
        {
            // Set up Form
            InitializeComponent();
            // Allow Drag&Drop
            AllowDrop = true;
            DragEnter += TabMain_DragEnter;
            DragDrop += TabMain_DragDrop;

            pba = new[] {bpkx1, bpkx2, bpkx3, bpkx4, bpkx5, bpkx6, bpkx7, bpkx8, bpkx9, bpkx10, bpkx11, bpkx12};
        }

        private readonly PictureBox[] pba;

        #region Form and Resource Setup

        private static void TabMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void TabMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            string path = files[0]; // open first D&D
            OpenQuick(path);
        }

        private void OpenQuick(string path)
        {
            var fi = new FileInfo(path);
            if (fi.Length != 0x2E60)
            {
                MessageBox.Show("Not a valid bv.");
                return;
            }
            OpenBV(File.ReadAllBytes(path));
        }

        private void B_DL_Click(object sender, EventArgs e)
        {
            new Request().ShowDialog();
        }

        private void ReloadBV(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control)
                Clipboard.SetText(lastParse?.hex ?? "No Parse Loaded.");
            else
                OpenBV(lastdata);
        }

        #endregion

        // Parsing Variables kept after parsing (until another video is loaded).
        private byte[] lastdata;
        private BVidParser lastParse;

        /// <summary>
        /// Load and Parse the Battle Video
        /// </summary>
        /// <param name="raw">Byte array containing the entire (decrypted) battle video's data.</param>
        private void OpenBV(byte[] raw)
        {
            Reset();

            lastdata = (byte[])raw.Clone();
            var bvid = new BV6(raw);

            var trainers = bvid.PlayerNames;
            var pkms = bvid.BattlePKMs;
            var teams = bvid.PlayerTeams;

            LoadBattleInfo(bvid, trainers);
            LoadTeamData(pkms, trainers);

            LoadPreviewPics(pkms, raw);

            // Move Instructions (0xC00 Max)
            int instructionLength = BitConverter.ToInt32(raw, 0x210);
            byte[] instructions = raw.Skip(0x214).Take(instructionLength).ToArray();

            byte order = raw[0];
            if (order != 0x20) // Switched
                BVidUtil.ReorderTeams(order, bvid.Style, teams, trainers);
            L_Uploader.Location = (order == 0x20) ? new Point(98, 0) : new Point(9, 61);

            var parser = new BVidParser(bvid, teams, trainers);
            RTB_Parse.Lines = parser.Parse(instructions);
            lastParse = parser;
        }

        private void Reset()
        {
            B_Reload.Enabled = true;
            RTB_Team.Clear();
            RTB_Parse.Clear();
        }

        private void LoadBattleInfo(BV6 bvid, IReadOnlyList<string> trainers)
        {
            TB_Mode.Text = ((BattleMode)bvid.Mode).ToString();
            TB_Style.Text = ((BattleStyle)bvid.Style).ToString();

            L_VS.Text = bvid.Style == 4
                ? string.Format("{0} && {2} -VS- {1} && {3}", trainers[0], trainers[1], trainers[2], trainers[3])
                : $"{trainers[0]} -VS- {trainers[1]}";

            rA.Text = bvid.RNGConst1.ToString("X8");
            r0.Text = bvid.RNGConst2.ToString("X8");
            rM.Text = bvid.RNGSeed1.ToString("X16");
            rN.Text = bvid.RNGSeed2.ToString("X16");
            u1t.Text = bvid.Debug1;
            u2t.Text = bvid.Debug2;
            TB_Recorded.Text = bvid.MatchStamp?.ToString("R") ?? "None";
            TB_Uploaded.Text = bvid.UploadStamp?.ToString("R") ?? "None";
        }

        private void LoadTeamData(IReadOnlyList<PKM> dat, IReadOnlyList<string> trainers)
        {
            int team = 0;
            for (int i = 0; i < 24; i++)
            {
                if (dat[i].Species <= 0 || dat[i].Species >= 722)
                    continue;

                if (i % 6 == 0)
                    RTB_Team.AppendText(string.Format("{1}{2}'s Party -- {0}{1}", ++team + Environment.NewLine, "========" + Environment.NewLine, trainers[team - 1]));
                RTB_Team.AppendText(new ShowdownSet(dat[i]).Text);
                RTB_Team.AppendText(Environment.NewLine + Environment.NewLine);
            }
        }

        private void LoadPreviewPics(IReadOnlyList<PKM> dat, byte[] data)
        {
            ushort[] previews = new ushort[12];
            for (int i = 0; i < 12; i++)
                pba[i].Image = (Image)Properties.Resources.ResourceManager.GetObject("_" + (previews[i] = BitConverter.ToUInt16(data, 0x154 + (2 * i))));
            // Apply Better Preview Pics
            for (int i = 0; i < 24; i++)
            {
                if (dat[i].Species <= 0 || dat[i].Species >= 722)
                    continue;

                int spot = Array.IndexOf(previews, dat[i].Species);
                if (spot < 0)
                    continue;
                previews[spot] = 0xFFFF;
                pba[spot].Image = SpriteUtil.GetSprite(dat[i].Species, dat[i].AltForm, dat[i].Gender, dat[i].SpriteItem, false, false);
            }
        }
    }
}