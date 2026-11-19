using System;

namespace RelatedMoney
{
    /// <summary>
    /// 金額を表す構造体
    /// </summary>
    public readonly struct Money : IComparable<Money>
    {
        private readonly int _amount;

        public Money(int amount)
        {
            _amount = amount;
        }
        
        public int CompareTo(Money other)
        {
            return _amount.CompareTo(other._amount);
        }
        
        public static bool operator <(Money left, Money right)
        {
            return left.CompareTo(right) < 0;
        }
        
        public static bool operator <=(Money left, Money right)
        {
            return left.CompareTo(right) <= 0;
        }
        
        public static bool operator >(Money left, Money right)
        {
            return left.CompareTo(right) > 0;
        }
        
        public static bool operator >=(Money left, Money right)
        {
            return left.CompareTo(right) >= 0;
        }
        
        public static Money operator +(Money left, Money right)
        {
            return new Money(left._amount + right._amount);
        }
        
        public static Money operator -(Money left, Money right)
        {
            var result = Math.Clamp(left._amount - right._amount, 0, int.MaxValue);
            return new Money(result);
        }
        
        public static Money operator *(Money left, int multiplier)
        {
            if (multiplier < 0)
                throw new ArgumentException("乗数は0以上である必要があります");
            return new Money(left._amount * multiplier);
        }
        
        public static Money operator /(Money left, int divisor)
        {
            if (divisor <= 0)
                throw new ArgumentException("除数は0以上である必要があります");
            return new Money(left._amount / divisor);
        }
        
        public override string ToString()
        {
            return _amount.ToString();
        }
    }
}
