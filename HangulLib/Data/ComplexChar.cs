using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace HangulLib.Data
{
    // ** README **
    // C#6.0 새로운 문법을 사용해봤어양
    // 변태같으면 Ctrl + K + C
    public struct ComplexChar :
        IComparable, IConvertible, 
        IComparable<char>, IComparable<ComplexChar>,
        IEquatable<char>, IEquatable<ComplexChar>
    {
        private List<ComplexChar> subChars;
        private char completion;

        public ComplexChar[] Chars => subChars.ToArray();
        public char Completion => completion;

        public ComplexChar this[int index]
        {
            get
            {
                return subChars[index];
            }
        }

        // Auto-Casting 순환 오류땜에 리팩토링 불가능하다고 판단.

        public ComplexChar(params char[] chars)
        {
            subChars = new List<ComplexChar>();
            completion = default(char);

            for (int i = 0; i < chars.Length - 1; i++)
                subChars.Add(chars[i]);

            completion = chars.LastOrDefault();
        }

        public ComplexChar(params ComplexChar[] chars)
        {
            subChars = new List<ComplexChar>();
            completion = default(char);

            for (int i = 0; i < chars.Length - 1; i++)
                subChars.Add(chars[i]);

            completion = chars.LastOrDefault();
        }

        #region 공용 함수
        [Pure]
        public static bool IsSolo(ComplexChar cc)
        {
            return cc.subChars.Count == 0;
        }
        #endregion

        #region Operator (Auto-Casting)
        public static implicit operator ComplexChar(ComplexChar[] chars)
        {
            if (chars.Length == 0)
                throw new InvalidCastException();

            return new ComplexChar(chars);
        }

        public static implicit operator ComplexChar(char[] chars)
        {
            if (chars.Length == 0)
                throw new InvalidCastException();

            return new ComplexChar(chars);
        }

        public static implicit operator ComplexChar(char ch)
        {
            return new ComplexChar(ch);
        }

        public static implicit operator int(ComplexChar cc)
        {
            return cc.ToInt32(null);
        }

        public static implicit operator char(ComplexChar cc)
        {
            return cc.completion;
        }
        #endregion

        #region Override 구현
#if DEBUG
        // 디버깅용
        public string ToString2()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < subChars.Count; i++)
            {
                sb.Append(subChars[i].ToString2());

                if (i < subChars.Count - 1)
                    sb.Append(", ");
            }

            if (subChars.Count > 0)
                return $"[{sb.ToString()}, {completion}]";
            else
                return completion.ToString();
        }
#endif

        public override string ToString()
        {
            return completion.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is char || obj is ComplexChar))
                return false;

            if (obj is ComplexChar)
                return this.Equals((ComplexChar)obj);
            else
                return this.Equals((char)obj);
        }

        public override int GetHashCode()
        {
            return completion.GetHashCode() +
                   subChars.Sum(c => c.GetHashCode());
        }
        #endregion

        #region Inteface 구현

        #region IComparable
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (!(obj is char || obj is ComplexChar))
                throw new ArgumentException();

            if (obj is ComplexChar)
                return this.CompareTo((ComplexChar)obj);
            else
                return this.CompareTo((char)obj);
        }

        public int CompareTo(char other)
        {
            return completion.CompareTo(other);
        }

        public int CompareTo(ComplexChar other)
        {
            if (subChars.Count != other.subChars.Count)
                return 1;

            int alpha = 0;

            for (int i = 0; i < subChars.Count; i++)
                alpha += subChars[i] - other.subChars[i];

            return (this.completion - other.completion) + alpha;
        }
        #endregion

        #region IEquatable
        public bool Equals(char other)
        {
            return completion.Equals(other);
        }

        public bool Equals(ComplexChar other)
        {
            if (subChars.Count != other.subChars.Count)
                return false;

            bool alpha = true;

            for (int i = 0; i < subChars.Count; i++)
                alpha &= subChars[i].Equals(other.subChars[i]);

            return completion.Equals(other.completion) && alpha;
        }
        #endregion

        #region IConvertible
        public TypeCode GetTypeCode()
        {
            return TypeCode.Char;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException("뭘 바꾸려들어");
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException("뭘 바꾸려들어");
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException("뭘 바꾸려들어");
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException("뭘 바꾸려들어");
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException("뭘 바꾸려들어");
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(completion);
        }

        public char ToChar(IFormatProvider provider)
        {
            return completion;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(completion);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(completion);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(completion);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(completion);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(completion);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(completion);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(completion);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return null;
        }

        public string ToString(IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return this.ToString();
        }
        #endregion

        #endregion
    }
}