using System;
using System.Globalization;

namespace FamilyTreeNet.Core.Support
{
    public sealed class GeneaDate
        : IComparable<GeneaDate>
    {
        public GeneaDate(int year, int month, int day)
            : this(year, month, day, DateAccuracy.Exact)
        {
        }

        public GeneaDate(int year, int month, int day, DateAccuracy accuracy)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.Accuracy = accuracy;
        }

        public GeneaDate(int ymd)
        {
            // yyyy_mm_dd_a
            this.Accuracy = (DateAccuracy)(ymd % 10);
            ymd /= 10;
            this.Day = ymd % 100;
            ymd /= 100;
            this.Month = ymd % 100;
            ymd /= 100;
            this.Year = ymd;
        }

        public GeneaDate(DateTime date)
        {
            this.Year = date.Year;
            this.Month = date.Month;
            this.Day = date.Day;
            this.Accuracy = DateAccuracy.Exact;
        }

        public int Year { get; }

        public int Month { get; }

        public int Day { get; }

        public DateAccuracy Accuracy { get; }

        public int ToInt32()
            => ((this.Year * 100 + this.Month) * 100 + this.Day) * 10 + (int)this.Accuracy;

        public DateTime ToDate()
        {
            if (this.Month <= 0)
            {
                return new DateTime(this.Year, 1, 1);
            }

            if (this.Day <= 0)
            {
                return new DateTime(this.Year, this.Month, 1);
            }

            return new DateTime(this.Year, this.Month, this.Day);
        }

        public override string ToString()
        {
            return $"{DayName(this.Day)} {MonthName(this.Month)} {this.Year}";

            string DayName(int dy) => dy > 0 && dy < 32 ? dy.ToString("00", CultureInfo.InvariantCulture) : "??";

            string MonthName(int monthNumber)
            {
                if (monthNumber >=1 && monthNumber <= 12)
                {
                    return CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[monthNumber - 1];
                }

                return "???";
            }
        }

        public int CompareTo(GeneaDate other)
        {
            if (other == null)
            {
                return -1;
            }

            if (this.Year == other.Year)
            {
                if (this.Month == other.Month)
                {
                    return this.Day.CompareTo(other.Day);
                }
                else
                {
                    return this.Month.CompareTo(other.Month);
                }
            }
            else
            {
                return this.Year.CompareTo(other.Year);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            return this.CompareTo(obj as GeneaDate) == 0;
        }

        public override int GetHashCode()
        {
            return this.ToInt32();
        }

        public static bool operator ==(GeneaDate left, GeneaDate right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(GeneaDate left, GeneaDate right)
        {
            return !(left == right);
        }

        public static bool operator <(GeneaDate left, GeneaDate right)
        {
            return left is null ? right is object : left.CompareTo(right) < 0;
        }

        public static bool operator <=(GeneaDate left, GeneaDate right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(GeneaDate left, GeneaDate right)
        {
            return left is object && left.CompareTo(right) > 0;
        }

        public static bool operator >=(GeneaDate left, GeneaDate right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }
    }
}
