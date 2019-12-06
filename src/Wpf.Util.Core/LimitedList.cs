using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Wpf.Util.Core
{
    /// <summary>
    /// The limited list. Used to store the messages to up to certain size.
    /// </summary>
    /// <typeparam name="T">Type name.
    /// </typeparam>
    internal class LimitedList<T> : IList<T>
    {
        /// <summary>
        /// The internal list.
        /// </summary>
        private readonly List<T> _internalList = new List<T>();

        /// <summary>
        /// The synchronization object.
        /// </summary>
        private readonly object _syncObj = new object();

        /// <summary>
        /// The maximum occupancy.
        /// </summary>
        private int _maximumOccupancy;

        /// <summary>
        /// The trim value.
        /// </summary>
        private int _trimValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedList{T}"/> class.
        /// </summary>
        /// <param name="maximumOccupancy">
        /// The maximum occupancy.
        /// </param>
        /// <param name="trimByValue">
        /// The trim value.
        /// </param>
        /// <exception cref="ArgumentException">If maximum less than or zero or trim value less than or zero. Also if trim value is more than maximum.
        /// </exception>
        public LimitedList(int maximumOccupancy, int trimByValue)
        {
            Validate(maximumOccupancy, trimByValue);

            this._maximumOccupancy = maximumOccupancy;
            this._trimValue = trimByValue;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get { return this._internalList.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether is read only.
        /// </summary>
        /// <value>
        /// The is read only.
        /// </value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T this[int index]
        {
            get { return this._internalList[index]; }
            set { this._internalList[index] = value; }
        }

        /// <summary>
        /// The set limits.
        /// </summary>
        /// <param name="maximumOccupancy">
        /// The maximum occupancy.
        /// </param>
        /// <param name="trimByValue">
        /// The trim by value.
        /// </param>
        public void SetLimits(int maximumOccupancy, int trimByValue)
        {
            Validate(maximumOccupancy, trimByValue);
            this._maximumOccupancy = maximumOccupancy;
            this._trimValue = trimByValue;
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>List Enumerator.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this._internalList.GetEnumerator();
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>List Enumerator.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._internalList.GetEnumerator();
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Add(T item)
        {
            lock (this._syncObj)
            {
                this._internalList.Add(item);
            }

            this.TrimIfNecessary();
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public void Clear()
        {
            this._internalList.Clear();
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Contains(T item)
        {
            return this._internalList.Contains(item);
        }

        /// <summary>
        /// The copy to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// The array index.
        /// </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this._internalList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Remove(T item)
        {
            return this._internalList.Remove(item);
        }

        /// <summary>
        /// The index of.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int IndexOf(T item)
        {
            return this._internalList.IndexOf(item);
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Insert(int index, T item)
        {
            lock (this._syncObj)
            {
                this._internalList.Insert(index, item);
            }

            this.TrimIfNecessary();
        }

        /// <summary>
        /// The remove at.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public void RemoveAt(int index)
        {
            this._internalList.RemoveAt(index);
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
            if (this.Count <= this._maximumOccupancy)
            {
                return;
            }

            lock (this._syncObj)
            {
                this._internalList.RemoveRange(0, this._trimValue);
            }
        }
    }
}
