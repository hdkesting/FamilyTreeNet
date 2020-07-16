using FamilyTreeNet.Core.Support;

using System;

namespace FamilyTreeNet.Support
{
    /// <summary>
    /// A mutable version of <see cref="GeneaDate"/>, for use in forms.
    /// </summary>
    public class MutableGeneaDate
    {
        /// <summary>Initializes a new instance of the <see cref="MutableGeneaDate" /> class.</summary>
        public MutableGeneaDate() : this(DateTime.Now)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableGeneaDate"/> class, based on the supplied <see cref="GeneaDate"/>.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <exception cref="ArgumentNullException">Date cannot be null.</exception>
        public MutableGeneaDate(GeneaDate date)
        {
            if (!(date is null))
            {
                this.Day = date.Day;
                this.Month = date.Month;
                this.Year = date.Year;
                this.Accuracy = date.Accuracy;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableGeneaDate"/> class, based on the supplied <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date">The date.</param>
        public MutableGeneaDate(DateTime date)
        {
            this.Day = date.Day;
            this.Month = date.Month;
            this.Year = date.Year;
            this.Accuracy = DateAccuracy.Exact;
        }

        /// <summary>
        /// Gets or sets the day part of the date (0 = unknown).
        /// </summary>
        /// <value>
        /// The day.
        /// </value>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the month part of the date (0 = unknown).
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the year part of the date (0 = unknown).
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the accuracy of the date.
        /// </summary>
        /// <value>
        /// The accuracy.
        /// </value>
        public DateAccuracy Accuracy { get; set; }

        /// <summary>
        /// Converts this to <see cref="GeneaDate"/>.
        /// </summary>
        /// <value>
        /// A genea date.
        /// </value>
        public GeneaDate ToGeneaDate()
        {
            if (this.Year <= 0)
            {
                this.Year = 0;
                this.Accuracy = DateAccuracy.Unknown;
            }

            if (this.Month <= 0 || this.Month > 12)
            {
                this.Month = 0;
                this.Accuracy = DateAccuracy.About;
            }

            if (this.Day <= 0 || this.Day > 31)
            {
                this.Day = 0;
                this.Accuracy = DateAccuracy.About;
            }

            var gdate = new GeneaDate(this.Year, this.Month, this.Day, this.Accuracy);

            return gdate;
        }
    }
}
