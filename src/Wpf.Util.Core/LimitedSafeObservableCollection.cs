using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Wpf.Util.Core
{
    public class LimitedSafeObservableCollection<T> : SafeObservableCollection<T>
    {
        /// <summary>
        /// The synchronization object.
        /// </summary>
        private readonly object _syncObj = new object();

        /// <summary>
        /// The maximum occupancy.
        /// </summary>
        private readonly int _maximumOccupancy;

        /// <summary>
        /// The trim value.
        /// </summary>
        private readonly int _trimValue;

        /// <summary>
        /// Flag used for suppressing notifications if we already in remove.
        /// </summary>
        private bool _inTrim;

        public LimitedSafeObservableCollection(int maximumOccupancy, int trimByValue)
        {
            this._maximumOccupancy = maximumOccupancy;
            this._trimValue = trimByValue;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this._inTrim)
            {
                return;
            }

            base.OnCollectionChanged(e);
            this.TrimIfNecessary();
        }

        /// <summary>
        /// Validates the list limit values.
        /// </summary>
        /// <param name="maximumOccupancy">
        /// The maximum occupancy.
        /// </param>
        /// <param name="trimByValue">
        /// The trim by value.
        /// </param>
        /// <exception cref="ArgumentException">If maximum less than or zero or trim value less than or zero. Also if trim value is more than maximum. 
        /// </exception>
        private static void Validate(int maximumOccupancy, int trimByValue)
        {
            if (maximumOccupancy <= 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "maximumOccupancy:{0} cannot be less than or zero.", maximumOccupancy));
            }

            if (trimByValue <= 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "trimByValue:{0} cannot be less than or zero.", trimByValue));
            }

            if ((maximumOccupancy - trimByValue) < 10)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "maximumOccupancy:{0} should be at least more than 10 from trimByValue:{1}.", maximumOccupancy, trimByValue));
            }
        }

        /// <summary>
        /// The trim if necessary.
        /// </summary>
        private void TrimIfNecessary()
        {
            this._inTrim = true;
            try
            {
                if (this.Count <= this._maximumOccupancy)
                {
                    return;
                }

                lock (_syncObj)
                {
                    // suppress notifications
                    for (var i = 0; i < this._trimValue; i++)
                    {
                        this.RemoveAt(i);
                    }

                    // enable notifications.
                }
            }
            finally
            {
                this._inTrim = true;
            }
        }
    }
}