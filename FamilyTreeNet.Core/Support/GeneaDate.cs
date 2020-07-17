using System;
using System.Globalization;

namespace FamilyTreeNet.Core.Support
{
    /// <summary>
    /// A date class specifically for genea-use, where parts may be unknown.
    /// </summary>
    /// <remarks>
    /// Only really supports Gregorian dates.
    /// </remarks>
    /// <seealso cref="System.IComparable{FamilyTreeNet.Core.Support.GeneaDate}" />
    public sealed class GeneaDate
        : IComparable<GeneaDate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneaDate"/> class, based on exact year, month and day values.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        public GeneaDate(int year, int month, int day)
            : this(year, month, day, DateAccuracy.Exact)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneaDate"/> class, based on year, month and day values and an accuracy.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="accuracy">The accuracy.</param>
        public GeneaDate(int year, int month, int day, DateAccuracy accuracy)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.Accuracy = accuracy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneaDate"/> class, based on the int-representation, as stored in the database.
        /// </summary>
        /// <param name="ymd">The ymd.</param>
        /// <seealso cref="GeneaDate.ToInt32"/>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneaDate"/> class, based on a specific (exact) date.
        /// </summary>
        /// <param name="date">The date.</param>
        public GeneaDate(DateTime date)
        {
            this.Year = date.Year;
            this.Month = date.Month;
            this.Day = date.Day;
            this.Accuracy = DateAccuracy.Exact;
        }

        /// <summary>
        /// Gets the year (0 = unknown).
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year { get; }

        /// <summary>
        /// Gets the month (1-12 for real months, 0=unknown).
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        public int Month { get; }

        /// <summary>
        /// Gets the day of the month (0=unknown).
        /// </summary>
        /// <value>
        /// The day.
        /// </value>
        public int Day { get; }

        /// <summary>
        /// Gets the accuracy of this date.
        /// </summary>
        /// <value>
        /// The accuracy.
        /// </value>
        public DateAccuracy Accuracy { get; }

        /// <summary>
        /// Converts this date to an easily storable and sortable <see cref="int"/>.
        /// </summary>
        /// <returns></returns>
        public int ToInt32()
            => ((this.Year * 100 + this.Month) * 100 + this.Day) * 10 + (int)this.Accuracy;

        /// <summary>
        /// Converts this date to a real date.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Converts this date to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
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

        /// <summary>
        /// Compares this date to the supplied one.
        /// </summary>
        /// <param name="other">The other date.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToInt32();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(GeneaDate left, GeneaDate right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(GeneaDate left, GeneaDate right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(GeneaDate left, GeneaDate right)
        {
            return left is null ? right is object : left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <=(GeneaDate left, GeneaDate right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(GeneaDate left, GeneaDate right)
        {
            return left is object && left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >=(GeneaDate left, GeneaDate right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }
    }
}
