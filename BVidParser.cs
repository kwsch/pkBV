using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PKHeX.Core;

namespace pkBV
{
    public class BVidParser
    {
        private readonly bool[] isOut = { false, false, false, false };
        private int turn;
        public string hex;

        private readonly BV6 bvid;
        private readonly PKM[][] teams;
        private readonly string[] trainers;

        public BVidParser(BV6 bv, PKM[][] teams, string[] trainers)
        {
            bvid = bv;
            this.teams = teams;
            this.trainers = trainers;
        }

        // Battle Parsing
        /// <summary>
        /// Main method to carry out a battle.
        /// </summary>
        /// <param name="data">Bytecode instructions to repeat the battle.</param>
        public string[] Parse(byte[] data)
        {
            // Initialize the battle.
            turn = 0;

            // Debug Bytecode instruction storage for quick viewing.
            hex = DumpInstructionHex(data);

            var lines = new List<string>();
            AddLines(data, lines);
            return lines.ToArray();
        }

        private static string DumpInstructionHex(byte[] data)
        {
            var snip = $"Instruction Length: 0x{data.Length:X} ({data.Length} bytes)." + Environment.NewLine;
            return data.Aggregate(snip, (current, t) => current + $"{t:X2} ");
        }

        private void AddLines(byte[] data, List<string> lines)
        {
            using (var ms = new MemoryStream(data))
            using (var br = new BinaryReader(ms))
            {
                var order = br.ReadByte(); // skip order
                lines.Add($"Start Battle: [0x{order:X2}]");
                // Turn 0: Initialize the battle; send out Pokemon.
                switch ((BattleStyle)bvid.Style)
                {
                    case BattleStyle.Single:
                        for (int i = 0; i < 2; i++)
                            lines.Add($"{trainers[i]} leads {teams[i][0].Nickname}");
                        break;
                    case BattleStyle.Double:
                        for (int i = 0; i < 2; i++)
                            lines.Add($"{trainers[i]} leads {teams[i][0].Nickname} and {teams[i][1].Nickname}");
                        break;

                    case BattleStyle.Triple:
                        for (int i = 0; i < 2; i++)
                            lines.Add($"{trainers[i]} leads {teams[i][0].Nickname}, {teams[i][1].Nickname}, and {teams[i][2].Nickname}");
                        break;

                    default:
                        for (int i = 0; i < 4; i++)
                            lines.Add($"{trainers[i]} leads {teams[i][0].Nickname}");
                        break;
                }

                // After the 0th turn is complete, commence the battle.
                while (br.BaseStream.Position < data.Length)
                {
                    var actions = PerformOp(br);
                    lines.AddRange(actions);
                }
            }
        }

        /// <summary>
        /// Consume Instructions to carry out the battle.
        /// </summary>
        /// <param name="br">BinaryReader to consume bytedata from.</param>
        private IEnumerable<string> PerformOp(BinaryReader br)
        {
            byte Op = br.ReadByte();
            var OpCode = Op >> 4;
            var OpLen = Op & 0xF;

            switch (OpCode)
            {
                case 1: // Switch
                    yield return "Switching Required...";
                    break;
                case 9: // Turn
                    yield return string.Empty;
                    yield return $"Start Turn {++turn}";
                    break;
            }

            for (int i = 0; i < OpLen; i++)
            {
                byte Op2 = br.ReadByte();
                var Player = Op2 >> 5;
                var OpLen2 = Op2 & 0xF;

                switch (OpCode)
                {
                    case 1:
                    {
                        var lines = PerformSwitch(Player, br.ReadBytes(4 * OpLen2));
                        foreach (var line in lines)
                            yield return line;
                        break;
                    }
                    case 9:
                    {
                        var lines = PerformAction(Player, br.ReadBytes(4 * OpLen2));
                        foreach (var line in lines)
                            yield return line;
                        break;
                    }
                    default:
                        yield return $"Unknown Instruction Type: {OpCode}";
                        break;
                }
            }
        }

        /// <summary>
        /// Switch Instruction
        /// </summary>
        /// <param name="Player">Which Player is carrying out the action.</param>
        /// <param name="data">Instruction data to carry out.</param>
        private IEnumerable<string> PerformSwitch(int Player, byte[] data)
        {
            var parse = $"[{trainers[Player]}]: ";
            //for (int i = 0; i < data.Length; i++)
            //    parse += data[i].ToString("X2") + " ";

            for (int i = 0; i < data.Length / 4; i++)
            {
                byte[] instruct = data.Skip(i * 4).Take(4).ToArray();
                if (instruct.All(z => z == 0))
                {
                    parse += "None";
                    continue;
                }
                var val = BitConverter.ToUInt16(data, i * 4);
                var Op = val & 0x7F;
                var OpCode = Op & 0xF;
                var SlotIn = val >> 7 & 0x7;
                var SlotOut = val >> 4 & 0x7;
                bool nothing = val >> 10 == 1;
                var Targeting = val >> 4;

                switch (OpCode)
                {
                    case 3:
                        if (isOut[Player])
                        {
                            parse += "None**"; // When defeated, they echo switches out.
                        }
                        else if (nothing)
                        {
                            parse += $"Out: {teams[Player][SlotOut].Nickname}, In: Nothing";
                        }
                        else
                        {
                            parse += $"Out: {teams[Player][SlotOut].Nickname}, In: {teams[Player][SlotIn].Nickname}";
                            BVidUtil.PerformSwap(ref teams[Player][SlotOut], ref teams[Player][SlotIn]);
                        }
                        break;

                    case 6:
                        string curActive = teams[Player][0].Nickname;
                        BVidUtil.PerformRotation(teams[Player], Targeting);
                        string nowActive = teams[Player][0].Nickname;
                        parse += $"Rotates: Out {curActive}, In: {nowActive}";
                        break;

                    default:
                        parse = data.Aggregate(parse, (current, t) => current + (t.ToString("X2") + " "));
                        break;
                }

                // Output raw bytes
                if ((i + 1) * 4 != data.Length)
                    parse += " & "; // Another action is present.
            }
            yield return parse;
        }

        /// <summary>
        ///  Carrying out battle instructions.
        /// </summary>
        /// <param name="Player">Which Player is carrying out the action.</param>
        /// <param name="data">Instruction data to carry out.</param>
        private IEnumerable<string> PerformAction(int Player, byte[] data)
        {
            var parse = $"[{trainers[Player]}]: ";

            //for (int i = 0; i < data.Length; i++)
            //    parse += data[i].ToString("X2") + " ";
            int user = 0;
            for (int i = 0; i < data.Length / 4; i++)
            {
                var val = data[i * 4];
                var OpCode = val & 0xF;
                var Targeting = val >> 4;
                string target = ((MoveTarget)Targeting).ToString();
                var action = (TurnActionCode)OpCode;
                switch (action)
                {
                    case TurnActionCode.None:
                        isOut[Player] = true;
                        parse += "Nothing";
                        break;

                    case TurnActionCode.Fight:
                    {
                        int move = BitConverter.ToUInt16(data, 1 + (4 * i));
                        int targetval = BVidUtil.movedata[move][0x14];
                        if (trainers[Player] == "NPC" && targetval == 0) target = "Opposite Enemy";
                        parse += string.Format(action + ": {3} uses {0} @ {1}{2}", GameInfo.Strings.Move[move], target, data[3 + (4 * i)] > 0 ? " - " + data[3 + (4 * i)] : "", teams[Player][user++].Nickname);
                        break;
                    }

                    case TurnActionCode.Switch:
                        var val2 = BitConverter.ToUInt16(data, i * 4);
                        var SlotIn = val2 >> 7 & 0x7;
                        var SlotOut = val2 >> 10;
                        parse += $"Out: {teams[Player][SlotOut].Nickname}, In: {teams[Player][SlotIn].Nickname}";
                        BVidUtil.PerformSwap(ref teams[Player][SlotOut], ref teams[Player][SlotIn]);
                        break;

                    case TurnActionCode.Rotate:
                    {
                        string rot = ((RotateDirection)Targeting).ToString();
                        // ushort un12 = BitConverter.ToUInt16(data, 1 + (4 * i));
                        // byte un3 = data[3 + (4 * i)];
                        string curIn = teams[Player][user].Nickname;
                        BVidUtil.PerformRotation(teams[Player], Targeting);
                        parse += string.Format(action + ": {1} rotates {0} to {2}", rot, curIn, teams[Player][user++].Nickname);
                        break;
                    }

                    case TurnActionCode.MegaEvolve:
                    {
                        var arg = BitConverter.ToUInt16(data, 1 + (4 * i)).ToString("X4") + data[3 + (4 * i)].ToString("X2");
                        parse += string.Format(action + ": {1}{2}", OpCode, target, arg == "000000" ? "" : " - " + arg);
                        break;
                    }

                    default:
                    {
                        var arg = BitConverter.ToUInt16(data, 1 + (4 * i)).ToString("X4") + data[3 + (4 * i)].ToString("X2");
                        parse += string.Format(action + ": {1}{2}", OpCode, target, arg == "000000" ? "" : " - " + arg);
                        user++;
                        break;
                    }
                }

                if ((i + 1) * 4 != data.Length)
                    parse += " & ";
            }
            yield return parse;
        }
    }
}