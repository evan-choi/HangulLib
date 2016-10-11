using HangulLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangulLib
{
    public static class Hangul
    {
        #region Const
        private const int HangulBase = 0xAC00;
        private const int ChosungOffset = 28 * 21;
        private const int JungsungOffset = 28;
        #endregion

        #region Preset
        private static char[] CHOSUNG = new char[]
        {
            'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ',
            'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ',
            'ㅌ', 'ㅍ', 'ㅎ'
        };

        private static ComplexChar[] JUNGSUNG = new ComplexChar[]
        {
            'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ',
            new char[] { 'ㅗ', 'ㅏ', 'ㅘ' },
            new char[] { 'ㅗ', 'ㅐ', 'ㅙ' },
            new char[] { 'ㅗ', 'ㅣ', 'ㅚ' },
            'ㅛ', 'ㅜ', 
            new char[] { 'ㅜ', 'ㅓ', 'ㅝ' },
            new char[] { 'ㅜ', 'ㅔ', 'ㅞ' },
            new char[] { 'ㅜ', 'ㅣ', 'ㅟ' },
            'ㅠ', 'ㅡ',
            new char[] { 'ㅡ', 'ㅣ', 'ㅢ' },
            'ㅣ'
        };

        private static ComplexChar[] JONGSUNG = new ComplexChar[]
        {
            default(char), 'ㄱ', 'ㄲ',
            new char[] { 'ㄱ', 'ㅅ', 'ㄳ' },
            'ㄴ',
            new char[] { 'ㄴ', 'ㅈ', 'ㄵ' },
            new char[] { 'ㄱ', 'ㅎ', 'ㄶ' },
            'ㄷ', 'ㄹ',
            new char[] { 'ㄹ', 'ㄱ', 'ㄺ' },
            new char[] { 'ㄹ', 'ㅁ', 'ㄻ' },
            new char[] { 'ㄹ', 'ㅂ', 'ㄼ' },
            new char[] { 'ㄹ', 'ㅅ', 'ㄽ' },
            new char[] { 'ㄹ', 'ㅌ', 'ㄾ' },
            new char[] { 'ㄹ', 'ㅍ', 'ㄿ' },
            new char[] { 'ㄹ', 'ㅎ', 'ㅀ' },
            'ㅁ', 'ㅂ',
            new char[] { 'ㅂ', 'ㅅ', 'ㅄ' },
            'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ'
        };
        #endregion

        public static bool IsHangul(string data)
        {
            foreach (char c in data)
            {
                if (!IsHangul(c))
                    return false;
            }

            return true;
        }

        public static bool IsHangul(char c)
        {
            var dataset = GetDataset(c);

            return IsValidData(dataset);
        }

        public static IEnumerable<ComplexChar> Disassemble(string data, bool onlyHangul = true)
        {
            foreach (var c in data)
            {
                var dataset = GetDataset(c);

                if ((onlyHangul && IsValidData(dataset)) || !onlyHangul)
                    yield return Disassemble(c);
            }
        }

        public static ComplexChar Disassemble(char c)
        {
            var dataset = GetDataset(c);
            var result = new List<ComplexChar>();

            if (!IsValidData(dataset))
                return c;

            result.Add(
                CHOSUNG[dataset.Chosung]);

            result.Add(
                CompletionFromChar(
                    JUNGSUNG[dataset.Jungsung]));

            if (dataset.Jongsung > 0)
                result.Add(
                    CompletionFromChar(
                        JONGSUNG[dataset.Jongsung]));

            result.Add(
                Build(
                    dataset.Chosung,
                    dataset.Jungsung,
                    dataset.Jongsung));

            return result.ToArray();
        }

        public static string Assemble(params ComplexChar[] chars)
        {
            var result = new StringBuilder();

            foreach (ComplexChar cc in chars)
                result.Append(Assemble(cc));

            return result.ToString();
        }

        public static char Assemble(ComplexChar cc)
        {
            if (cc.Chars.Length == 3)
            {
                return RawBuild(cc[0], cc[1], cc[2]);
            }
            else if (cc.Chars.Length == 2)
            {
                return RawBuild(cc[0], cc[1], cc.Completion);
            }
            else if (cc.Chars.Length == 1)
            {
                return RawBuild(cc[0], cc.Completion, 0);
            }
            else
                return cc;
        }

        public static bool IsChosung(char c)
        {
            return CHOSUNG.Contains(c);
        }

        public static bool IsJungsung(char c)
        {
            return JUNGSUNG.Count(j => j.Completion == c) > 0;
        }

        public static bool IsJongsung(char c)
        {
            return JONGSUNG.Contains(c);
        }

		public static bool Contains(string source, string value)
		{
			if (source.Length < value.Length)
				return false;

			var sourceAssm = Hangul.Disassemble(source, false).ToArray();
			var valueAssm = Hangul.Disassemble(value, false).ToArray();

			char[] sourceCho = sourceAssm.Select(cc => ((cc.Chars.Length > 0 ? (char)cc[0] : cc.Completion))).ToArray();
			char[] valueCho = valueAssm.Select(cc => ((cc.Chars.Length > 0 ? (char)cc[0] : cc.Completion))).ToArray();

			char[] sourceJung = sourceAssm.Select(cc => ((cc.Chars.Length == 2 ? (char)cc[1] : cc.Completion))).ToArray();
			char[] valueJung = valueAssm.Select(cc => ((cc.Chars.Length == 2 ? (char)cc[1] : cc.Completion))).ToArray();

			int index = -1;

			while ((index = Array.IndexOf(sourceCho, valueCho[0], index + 1)) != -1)
			{
				for (int i = 0; i < value.Length; i++)
				{
					bool isLast = (i == value.Length - 1);
					int sourceIndex = index + i;

					if (sourceIndex >= source.Length)
						break;

					char sourceChar = source[sourceIndex];
					char valueChar = value[i];

					if (Hangul.IsChosung(value[i]))
						sourceChar = sourceCho[sourceIndex];

					if (Hangul.IsJungsung(valueJung[i]) && isLast)
					{
						var complexSource = sourceAssm[sourceIndex].Chars.Take(2).ToArray();
						var complexValue = valueAssm[i].Chars.Take(2).ToArray();

						if (complexValue.Length > 1 && complexValue[1].Chars.Length == 0 &&
							complexSource.Length > 1 && complexSource[1].Chars.Length > 0)
						{
							complexSource[1] = complexSource[1][0];
						}

						sourceChar = Hangul.Assemble(new ComplexChar[] { complexSource })[0];
						valueChar = Hangul.Assemble(new ComplexChar[] { complexValue })[0];
					}

					if (sourceChar != valueChar)
						break;

					if (isLast)
						return true;
				}
			}

			return false;
		}

        #region 내부 함수
        private static ComplexChar CompletionFromChar(char c)
        {
            return JUNGSUNG
                .Concat(JONGSUNG)
                .SingleOrDefault(cc => cc.Completion == c);
        }

        private static char RawBuild(int cho, int jung, int jong)
        {
            cho = Array.IndexOf(CHOSUNG, (char)cho);
            jung = JUNGSUNG.ToList().FindIndex(cc => cc.Equals((char)jung));
            jong = JONGSUNG.ToList().FindIndex(cc => cc.Equals((char)jong));

            if (jong == -1)
                jong = 0;

            if (cho == -1 || jung == -1)
                throw new ArgumentException();

            return Build(cho, jung, jong);
        }

        private static char Build(int cho, int jung, int jong)
        {
            // 0xAC00 + 28 * 21 * (초성) + 28 * (중성) + (종성)
            // 공식을 따름

            int result = HangulBase + (ChosungOffset * cho) + (JungsungOffset * jung) + jong;

            return (char)result;
        }

        private static HangulDataset GetDataset(char c)
        {
            int dt = (c - HangulBase);

            return new HangulDataset()
            {
                Chosung = dt / ChosungOffset,
                Jungsung = (dt % ChosungOffset) / JungsungOffset,
                Jongsung = dt % JungsungOffset
            };
        }

        private static bool IsValidData(HangulDataset dataset)
        {
            return (dataset.Chosung >= 0 && dataset.Chosung < CHOSUNG.Length) &&
                   (dataset.Jungsung >= 0 && dataset.Jungsung < JUNGSUNG.Length) &&
                   (dataset.Jongsung >= 0 && dataset.Jongsung < JONGSUNG.Length);
        }
        #endregion
    }
}
