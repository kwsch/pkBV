using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;

namespace pkBV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            // Set up Form
            InitializeComponent();
            // Allow Drag&Drop
            AllowDrop = true;
            DragEnter += tabMain_DragEnter;
            DragDrop += tabMain_DragDrop;

            // Set up Strings
            InitLanguage();

            // Fetch Move Data
            for (int i = 0; i < 622; i++)
                movedata[i] = (byte[])Properties.Resources.ResourceManager.GetObject("move_" + i.ToString("000"));
        }
        #region Form and Resource Setup
        private void InitLanguage()
        {
            string l = curlanguage;
            natures = Util.getStringList("Natures", l);
            types = Util.getStringList("Types", l);
            abilitylist = Util.getStringList("Abilities", l);
            movelist = Util.getStringList("Moves", l);
            itemlist = Util.getStringList("Items", l);
            characteristics = Util.getStringList("Character", l);
            specieslist = Util.getStringList("Species", l);
            wallpapernames = Util.getStringList("Wallpaper", l);
            itempouch = Util.getStringList("ItemPouch", l);
            encountertypelist = Util.getStringList("EncounterType", l);
            gamelist = Util.getStringList("Games", l);
            gamelanguages = Util.getNulledStringArray(Util.getSimpleStringList("languages"));
            consoleregions = Util.getNulledStringArray(Util.getSimpleStringList("regions3ds"));

            balllist = new string[Legal.Items_Ball.Length];
            for (int i = 0; i < balllist.Length; i++)
                balllist[i] = itemlist[Legal.Items_Ball[i]];

            // Fix Item Names (Duplicate entries)
            itemlist[456] += " (OLD)"; // S.S. Ticket
            itemlist[463] += " (OLD)"; // Storage Key
            itemlist[478] += " (OLD)"; // Basement Key
            itemlist[626] += " (2)"; // Xtransceiver
            itemlist[629] += " (2)"; // DNA Splicers
            itemlist[637] += " (2)"; // Dropped Item
            itemlist[707] += " (2)"; // Travel Trunk
            itemlist[713] += " (2)"; // Alt Bike
            itemlist[714] += " (2)"; // Holo Caster
            itemlist[729] += " (1)"; // Meteorite
            itemlist[740] += " (2)"; // Contest Costume
            itemlist[751] += " (2)"; // Meteorite
            itemlist[771] += " (3)"; // Meteorite
            itemlist[772] += " (4)"; // Meteorite

            // Get the Egg Name and then replace it with --- for the comboboxes.
            eggname = specieslist[0];
            specieslist[0] = "---";

            // Get the met locations... for all of the games...
            metHGSS_00000 = Util.getStringList("hgss_00000", l);
            metHGSS_02000 = Util.getStringList("hgss_02000", l);
            metHGSS_03000 = Util.getStringList("hgss_03000", l);

            metBW2_00000 = Util.getStringList("bw2_00000", l);
            metBW2_30000 = Util.getStringList("bw2_30000", l);
            metBW2_40000 = Util.getStringList("bw2_40000", l);
            metBW2_60000 = Util.getStringList("bw2_60000", l);

            metXY_00000 = Util.getStringList("xy_00000", l);
            metXY_30000 = Util.getStringList("xy_30000", l);
            metXY_40000 = Util.getStringList("xy_40000", l);
            metXY_60000 = Util.getStringList("xy_60000", l);

            // Fix up some of the Location strings to make them more descriptive:
            metHGSS_02000[1] += " (NPC)";         // Anything from an NPC
            metHGSS_02000[2] += " (" + eggname + ")"; // Egg From Link Trade
            metBW2_00000[36] = metBW2_00000[84] + "/" + metBW2_00000[36]; // Cold Storage in BW = PWT in BW2

            // BW2 Entries from 76 to 105 are for Entralink in BW
            for (int i = 76; i < 106; i++)
                metBW2_00000[i] = metBW2_00000[i] + "●";

            // Localize the Poketransfer to the language (30001)
            metBW2_30000[1 - 1] = "Poké Transfer";
            metBW2_30000[2 - 1] += " (NPC)";              // Anything from an NPC
            metBW2_30000[3 - 1] += " (" + eggname + ")";  // Egg From Link Trade

            // Zorua/Zoroark events
            metBW2_30000[10 - 1] = specieslist[251] + " (" + specieslist[570] + " 1)"; // Celebi's Zorua Event
            metBW2_30000[11 - 1] = specieslist[251] + " (" + specieslist[570] + " 2)"; // Celebi's Zorua Event
            metBW2_30000[12 - 1] = specieslist[571] + " (" + "1)"; // Zoroark
            metBW2_30000[13 - 1] = specieslist[571] + " (" + "2)"; // Zoroark

            metBW2_60000[3 - 1] += " (" + eggname + ")";  // Egg Treasure Hunter/Breeder, whatever...

            metXY_00000[104] += " (X/Y)";              // Victory Road
            metXY_00000[298] += " (OR/AS)";            // Victory Road
            metXY_30000[0] += " (NPC)";                // Anything from an NPC
            metXY_30000[1] += " (" + eggname + ")";    // Egg From Link Trade

            // Set the first entry of a met location to "" (nothing)
            // Fix (None) tags
            abilitylist[0] = itemlist[0] = movelist[0] = metXY_00000[0] = metBW2_00000[0] = metHGSS_00000[0] = "(" + itemlist[0] + ")";

        }
        #region Global String (Arrays)
        public static string curlanguage = "en";
        public static string eggname = "";
        public static string[] gendersymbols = { "♂", "♀", "-" };
        public static string[] specieslist = { };
        public static string[] movelist = { };
        public static string[] itemlist = { };
        public static string[] abilitylist = { };
        public static string[] types = { };
        public static string[] natures = { };
        public static string[] characteristics = { };
        public static string[] memories = { };
        public static string[] genloc = { };
        public static string[] forms = { };
        public static string[] metHGSS_00000 = { };
        public static string[] metHGSS_02000 = { };
        public static string[] metHGSS_03000 = { };
        public static string[] metBW2_00000 = { };
        public static string[] metBW2_30000 = { };
        public static string[] metBW2_40000 = { };
        public static string[] metBW2_60000 = { };
        public static string[] metXY_00000 = { };
        public static string[] metXY_30000 = { };
        public static string[] metXY_40000 = { };
        public static string[] metXY_60000 = { };
        public static string[] trainingbags = { };
        public static string[] trainingstage = { };
        public static string[] wallpapernames = { };
        public static string[] encountertypelist = { };
        public static string[] gamelanguages = { };
        public static string[] consoleregions = { };
        public static string[] balllist = { };
        public static string[] gamelist = { };
        public static string[] puffs = { };
        public static string[] itempouch = { };
        #endregion
        private byte[][] movedata = new byte[622][];
        private void tabMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private void tabMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string path = files[0]; // open first D&D
            openQuick(path);
        }
        private void openQuick(string path)
        {
            var fi = new FileInfo(path);
            if (fi.Length == 0x2E60)
            {
                byte[] data = File.ReadAllBytes(path);
                openBV(data);
            }
            else Util.Alert("Not a valid bv.");
        }
        #endregion
        // Battle Video Data Indexes
        string[] BVstyle = { "Single", "Double", "Triple", "Rotation", "Multi", };
        string[] BVmode = { "Link", "Maison", "Super Maison", "Battle Spot - Free", "Battle Spot - Rating", "Battle Spot - Special", "UNUSED", "JP-1", "JP-2", "BROKEN", };
        // Battle Instruction Parsing
        string[] Action = { "0", "Fight", "2", "Switch", "Run", "5", "Rotate", "7", "MegaEvolve" };
        string[] Target = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "Opposite Enemy", "B", "C", "D", "All except User", "Everyone" };
        string[] Rotate = { "0", "Right", "Left", "3" };

        // Parsing Variables kept after parsing (until another video is loaded).
        byte[] lastdata;
        PKX[] dat;
        byte[][] pkxes;
        string[] trainers;
        PKX[][] teams;
        bool[] isOut;
        int turn;
        int style;
        string snip;

        // Force reload the last battle video.
        private void reloadBV(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control) Clipboard.SetText(snip); 
            else openBV(lastdata);
        }
        /// <summary>
        /// Load and Parse the Battle Video
        /// </summary>
        /// <param name="data">Byte array containing the entire (decrypted) battle video's data.</param>
        private void openBV(byte[] data)
        {
            TB_Mode.Font = PKX.getPKXFont(11);

            lastdata = (byte[])data.Clone(); B_Reload.Enabled = true;
            RTB_Team.Clear(); RTB_Parse.Clear();
            // Intro
            ushort bvtype = BitConverter.ToUInt16(data, 0x0);
            var mode = bvtype & 0xFF; TB_Mode.Text = BVmode[mode];
            style = bvtype >> 8; TB_Style.Text = BVstyle[style];

            trainers = new string[4];
            for (int i = 0; i < 4; i++)
            { trainers[i] = Util.TrimFromZero(Encoding.Unicode.GetString(data, 0xEC + 0x1A * i, 0x1A)); if (trainers[i] == "") trainers[i] = "NPC"; }

            if (style == 4) // 4 player
                L_VS.Text = String.Format("{0} && {2} -VS- {1} && {3}", trainers[0], trainers[1], trainers[2], trainers[3]);
            else
                L_VS.Text = String.Format("{0} -VS- {1}", trainers[0], trainers[1]);

            PictureBox[] pba = { bpkx1, bpkx2, bpkx3, bpkx4, bpkx5, bpkx6, bpkx7, bpkx8, bpkx9, bpkx10, bpkx11, bpkx12 };
            ushort[] previews = new ushort[12];
            for (int i = 0; i < 12; i++)
                pba[i].Image = (Image)Properties.Resources.ResourceManager.GetObject("_" + (previews[i] = BitConverter.ToUInt16(data, 0x154 + 2 * i)));

            // ushort maisonNum = BitConverter.ToUInt16(data, 0x190);

            // RNG
            uint rng1 = BitConverter.ToUInt32(data, 0x1A0); rA.Text = rng1.ToString("X8");
            uint rng1a = BitConverter.ToUInt32(data, 0x1A4); r0.Text = rng1a.ToString("X8");

            ulong rng64a = BitConverter.ToUInt64(data, 0x1A8); rM.Text = rng64a.ToString("X16");
            ulong rng64b = BitConverter.ToUInt64(data, 0x1B0); rN.Text = rng64b.ToString("X16");

            // uint background = BitConverter.ToUInt32(data, 0x1BC);
            // ushort unk_1CE = BitConverter.ToUInt16(data, 0x1CE);
            // ushort intro = BitConverter.ToUInt16(data, 0x1E4);
            // ushort musicID = BitConverter.ToUInt16(data, 0x1F0);

            // Extra Bonus!
            string u1 = Util.TrimFromZero(Encoding.Unicode.GetString(data, 0x6, 24)); u1t.Text = u1;
            string u2 = Util.TrimFromZero(Encoding.Unicode.GetString(data, 0x50, 24)); u2t.Text = u2;

            // Fetch all the Pokemon Data
            pkxes = new byte[24][];
            dat = new PKX[24];
            for (int i = 0; i < 24; i++)
                dat[i] = new PKX(pkxes[i] = PKX.decryptArray(data.Skip(0xE18 + 260 * i + (i / 6) * 8).Take(260).ToArray()), i.ToString());

            int team = 0;
            for (int i = 0; i < 24; i++)
            {
                if (dat[i].mspecies <= 0 || dat[i].mspecies >= 722) continue;

                if (i % 6 == 0) RTB_Team.AppendText(String.Format("{1}{2}'s Party -- {0}{1}", ++team + Environment.NewLine, "========" + Environment.NewLine, trainers[team - 1]));
                RTB_Team.AppendText(PKX.getSummary(dat[i]));
                RTB_Team.AppendText(Environment.NewLine);
            }
            // Apply Better Preview Pics
            for (int i = 0; i < 24; i++)
            {
                if (dat[i].mspecies <= 0 || dat[i].mspecies >= 722) continue;

                int spot = Array.IndexOf(previews, dat[i].mspecies); 
                if (spot < 0) continue; 
                previews[spot] = 0xFFFF;
                pba[spot].Image = PKX.getSlotFiller(pkxes[i]);
            }
            // Prepare Teams for Parsing
            teams = new PKX[4][];
            teams[0] = dat.Skip(0).Take(6).ToArray();
            teams[1] = dat.Skip(6).Take(6).ToArray();
            teams[2] = dat.Skip(12).Take(6).ToArray();
            teams[3] = dat.Skip(18).Take(6).ToArray();

            // Move Instructions (0xC00 Max)
            int instructionLength = BitConverter.ToInt32(data, 0x210);
            byte[] instructions = data.Skip(0x214).Take(instructionLength).ToArray();
            parseBattleInstructions(instructions);

            // Recorded
            {
                ushort year = BitConverter.ToUInt16(data, 0x2E50);
                byte day = data[0x2E52];
                byte month = data[0x2E53];
                byte hour = data[0x2E54];
                byte minute = data[0x2E55];
                byte second = data[0x2E56];
                byte flag = data[0x2E57];
                string timestamp = String.Format("{2}/{1}/{0} @ {3}:{4}:{5}",
                    year.ToString("0000"), month.ToString("00"), day.ToString("00"),
                    hour.ToString("00"), minute.ToString("00"), second.ToString("00"));
                TB_Recorded.Text = timestamp;
            }

            // Uploaded
            {
                ushort year = BitConverter.ToUInt16(data, 0x2E58);
                byte day = data[0x2E5A];
                byte month = data[0x2E5B];
                byte hour = data[0x2E5C];
                byte minute = data[0x2E5D];
                byte second = data[0x2E5E];
                byte flag = data[0x2E5F];
                string timestamp = String.Format("{2}/{1}/{0} @ {3}:{4}:{5}",
                    year.ToString("0000"), month.ToString("00"), day.ToString("00"),
                    hour.ToString("00"), minute.ToString("00"), second.ToString("00"));
                TB_Uploaded.Text = timestamp;
            }
        }
        // Battle Parsing
        /// <summary>
        /// Main method to carry out a battle.
        /// </summary>
        /// <param name="data">Bytecode instructions to repeat the battle.</param>
        private void parseBattleInstructions(byte[] data)
        {
            // Initialize the battle.
            turn = 0;
            isOut = new[] { false, false, false, false };

            // Debug Bytecode instruction storage for quick viewing.
            snip = String.Format("Instruction Length: 0x{0} ({1} bytes).", data.Length.ToString("X"), data.Length) + Environment.NewLine;
            foreach (byte t in data) snip += t.ToString("X2") + " ";
            // richTextBox2.Text = snip;


            MemoryStream ms = new MemoryStream(data);
            BinaryReader br = new BinaryReader(ms);

            byte order = br.ReadByte();
            if (order != 0x20) // Switched
            {
                if ((order & 0xA0) == 0xA0 && style == 4)
                {
                    for (int p = 0; p < 6; p++)
                        performSwap(ref teams[0][p], ref teams[2][p]);

                    var t0 = trainers[0]; var t2 = trainers[2];
                    trainers[2] = t0; trainers[0] = t2;
                }
                else if ((order & 0xA0) == 0xA0)
                {
                    for (int p = 0; p < 6; p++)
                        performSwap(ref teams[0][p], ref teams[1][p]);

                    var t0 = trainers[0]; var t1 = trainers[1];
                    trainers[1] = t0; trainers[0] = t1;
                }
                //}
                //for (int i = 0; i < 4; i+=2)
                //    for (int p = 0; p < 6; p++)
                //    {
                //        performSwap(ref teams[i][p], ref teams[i + 1][p]);
                //        performSwap(ref teams[i][p], ref teams[i + 1][p]);
                //    }

                // trainers = new string[] { trainers[2], trainers[3], trainers[0], trainers[1] };
            }
            else
                L_Uploader.Location = new Point(9, 61);
            string parse = "";
            {
                // Turn 0: Initialize the battle; send out Pokemon.
                parse += String.Format("Start Battle: [0x{0}]", order.ToString("X2")) + Environment.NewLine;
                if (style == 0) // Singles
                    for (int i = 0; i < 2; i++)
                        parse += String.Format("{0} leads {1}", trainers[i], teams[i][0].Species) + Environment.NewLine;
                else if (style == 1) // Doubles
                    for (int i = 0; i < 2; i++)
                        parse += String.Format("{0} leads {1} and {2}", trainers[i], teams[i][0].Species, teams[i][1].Species) + Environment.NewLine;
                else if (style < 4) // Triples
                    for (int i = 0; i < 2; i++)
                        parse += String.Format("{0} leads {1}, {2}, and {3}", trainers[i], teams[i][0].Species, teams[i][1].Species, teams[i][2].Species) + Environment.NewLine;
                else
                    for (int i = 0; i < 4; i++)
                        parse += String.Format("{0} leads {1}", trainers[i], teams[i][0].Species) + Environment.NewLine;

                // After the 0th turn is complete, commence the battle.
                while (br.BaseStream.Position < data.Length)
                    performOp(br, ref parse);
            }
            RTB_Parse.Text = parse;
        }
        /// <summary>
        /// Consume Instructions to carry out the battle.
        /// </summary>
        /// <param name="br">BinaryReader to consume bytedata from.</param>
        /// <param name="parse">Output string to append to.</param>
        private void performOp(BinaryReader br, ref string parse)
        {
            byte Op = br.ReadByte();
            var OpCode = Op >> 4;
            var OpLen = Op & 0xF;

            if (OpCode == 1) // Switch
                parse += "Switching Required..." + Environment.NewLine;
            else if (OpCode == 9) // Move
                parse += Environment.NewLine + String.Format("Start Turn {0}", ++turn) + Environment.NewLine;

            for (int i = 0; i < OpLen; i++)
            {
                byte Op2 = br.ReadByte();
                var Player = Op2 >> 5;
                var OpLen2 = Op2 & 0xF;

                if (OpCode == 1) // Switch
                    performSwitch(Player, br.ReadBytes(4 * OpLen2), ref parse);
                else if (OpCode == 9) // Move
                    performAction(Player, br.ReadBytes(4 * OpLen2), ref parse);
                else Util.Alert("Unknown Instruction Type!");
            }
        }
        /// <summary>
        /// Switch Instruction
        /// </summary>
        /// <param name="Player">Which Player is carrying out the action.</param>
        /// <param name="data">Instruction data to carry out.</param>
        /// <param name="parse">Output string to append to.</param>
        private void performSwitch(int Player, byte[] data, ref string parse)
        {
            parse += String.Format("[{0}]: ", trainers[Player]);
            //for (int i = 0; i < data.Length; i++)
            //    parse += data[i].ToString("X2") + " ";

            for (int i = 0; i < data.Length / 4; i++)
            {
                byte[] instruct = data.Skip(i * 4).Take(4).ToArray();
                if (instruct.SequenceEqual(new byte[4]))
                { parse += "None"; continue; }
                var val = BitConverter.ToUInt16(data, i * 4);
                var Op = val & 0x7F;
                var OpCode = Op & 0xF;
                var SlotIn = val >> 7 & 0x7;
                var SlotOut = val >> 4 & 0x7;
                bool nothing = val >> 10 == 1;
                var Targeting = val >> 4;
                if (OpCode == 3) // Switch
                {
                    if (isOut[Player])
                        parse += "None**"; // When defeated, they echo switches out.
                    else if (nothing)
                        parse += String.Format("Out: {0}, In: {1}", teams[Player][SlotOut].Species, "Nothing");
                    else
                    {
                        parse += String.Format("Out: {0}, In: {1}", teams[Player][SlotOut].Species, teams[Player][SlotIn].Species);
                        performSwap(ref teams[Player][SlotOut], ref teams[Player][SlotIn]);
                    }
                }
                else if (OpCode == 6) // Rotate
                {
                    string curActive = teams[Player][0].Species;
                    performRotation(ref teams[Player], Targeting);
                    string nowActive = teams[Player][0].Species;
                    parse += String.Format("Rotates: Out {0}, In: {1}", curActive, nowActive);
                }
                else
                    parse = data.Aggregate(parse, (current, t) => current + (t.ToString("X2") + " ")); // Output raw bytes

                if ((i + 1) * 4 != data.Length) parse += " & "; // Another action is present.
            }
            parse += Environment.NewLine;
        }
        /// <summary>
        ///  Carrying out battle instructions.
        /// </summary>
        /// <param name="Player">Which Player is carrying out the action.</param>
        /// <param name="data">Instruction data to carry out.</param>
        /// <param name="parse">Output string to append to.</param>
        private void performAction(int Player, byte[] data, ref string parse)
        {
            parse += String.Format("[{0}]: ", trainers[Player]);

            //for (int i = 0; i < data.Length; i++)
            //    parse += data[i].ToString("X2") + " ";
            int user = 0;
            for (int i = 0; i < data.Length / 4; i++)
            {
                var val = data[i * 4];
                var OpCode = val & 0xF;
                var Targeting = val >> 4;
                if (OpCode == 0) // Nothing
                {
                    isOut[Player] = true;
                    parse += "Nothing";
                }
                else if (OpCode == 1) // Fight
                {
                    int move = BitConverter.ToUInt16(data, 1 + 4 * i);
                    int targetval = movedata[move][0x14];
                    string target = (targetval != 0x7 && targetval != 13) ? Target[Targeting] : "Self";
                    if (trainers[Player] == "NPC" && targetval == 0) target = "Opposite Enemy";
                    parse += String.Format(Action[OpCode] + ": {3} uses {0} @ {1}{2}", movelist[move], target, data[3 + 4 * i] > 0 ? " - " + data[3 + 4 * i] : "", teams[Player][user++].Species);
                }
                else if (OpCode == 3) // Switch
                {
                    var val2 = BitConverter.ToUInt16(data, i * 4);
                    var SlotIn = val2 >> 7 & 0x7;
                    var SlotOut = val2 >> 10;
                    parse += String.Format("Out: {0}, In: {1}", teams[Player][SlotOut].Species, teams[Player][SlotIn].Species);
                    performSwap(ref teams[Player][SlotOut], ref teams[Player][SlotIn]);
                }
                else if (OpCode == 6)
                {
                    string rot = Rotate[Targeting];
                    ushort un12 = BitConverter.ToUInt16(data, 1 + 4 * i);
                    byte un3 = data[3 + 4 * i];
                    string curIn = teams[Player][user].Species;
                    performRotation(ref teams[Player], Targeting);
                    parse += String.Format(Action[OpCode] + ": {1} rotates {0} to {2}", rot, curIn, teams[Player][user++].Species);
                }
                else if (OpCode == 8)
                {
                    var arg = BitConverter.ToUInt16(data, 1 + 4 * i).ToString("X4") + data[3 + 4 * i].ToString("X2");
                    parse += String.Format(Action[OpCode] + ": {1}{2}", OpCode, Target[Targeting], arg == "000000" ? "" : " - " + arg);
                }
                else
                {
                    var arg = BitConverter.ToUInt16(data, 1 + 4 * i).ToString("X4") + data[3 + 4 * i].ToString("X2");
                    parse += String.Format(Action[OpCode] + ": {1}{2}", OpCode, Target[Targeting], arg == "000000" ? "" : " - " + arg);
                    user++;
                }
                if ((i + 1) * 4 != data.Length) parse += " & ";
            }
            parse += Environment.NewLine;
        }
        /// <summary>
        /// Rotate Pokemon around the field.
        /// </summary>
        /// <param name="team">Entire Team; first 3 are rotated.</param>
        /// <param name="count">Times to rotate Right; Left = 2 (== -1)</param>
        private void performRotation(ref PKX[] team, int count)
        {
            for (int i = 0; i < count; i++)
            {
                PKX t0 = team[0];
                PKX t1 = team[1];
                PKX t2 = team[2];

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
        private void performSwap(ref PKX a1, ref PKX a2)
        {
            PKX t1 = a1;
            PKX t2 = a2;
            a1 = t2; a2 = t1;
        }

        private void B_DL_Click(object sender, EventArgs e)
        {
            new Request().ShowDialog();
        }
    }
}