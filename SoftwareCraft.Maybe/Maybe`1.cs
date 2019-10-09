﻿using System;
using System.Linq;

namespace SoftwareCraft.Functional
{
	public abstract class Maybe<T> : IEquatable<Maybe<T>>
	{
		protected readonly T[] Items;

		protected Maybe() => Items = Array.Empty<T>();

		protected Maybe(T value)
		{
			if (null == value) throw new ArgumentNullException();

			Items = new[]
			{
				value
			};
		}

		public bool Equals(Maybe<T> other)
		{
			if (null == other) return false;

			switch (Items.Length)
			{
				case 1 when other.Items.Length == 0: // Some equals None?
				case 0 when other.Items.Length == 1: // None equals Some?
					return false;
				case 0 when other.Items.Length == 0: // None equals None?
					return true;
				default:
					return Items[0].Equals(other.Items[0]); // Some equals Some?
			}
		}

		public abstract Maybe<U> Select<U>(Func<T, U> some, Func<U> none);

		public abstract Maybe<U> Select<U>(Func<T, U> some);

		public abstract Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some, Func<Maybe<U>> none);

		public abstract Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some);

		public abstract void Match(Action<T> some, Action none);

		public abstract TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none);

		public override bool Equals(object obj)
		{
			if (obj is null) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((Maybe<T>) obj);
		}

		public static bool operator ==(Maybe<T> left, Maybe<T> right) => Equals(left, right);

		public static bool operator !=(Maybe<T> left, Maybe<T> right) => !Equals(left, right);

		public override int GetHashCode() => Items.Length == 0 ? 0 : Items[0].GetHashCode();
	}

	public static class MaybeExtensions
	{
		public static Maybe<U> Apply<T, U>(this Maybe<Func<T, U>> m, Maybe<T> other)
			=> m.Match(f => other.Match(o => Maybe.Some(f(o)), Maybe.None<U>), Maybe.None<U>);
	}
}