namespace CodingHelmet.Optional;

public readonly struct ValueOption<T> : IEquatable<ValueOption<T>> where T : struct
{
    private T? Content { get; init; }

    public static ValueOption<T> Some(T obj) => new() { Content = obj };
    public static ValueOption<T> None() => new();

    public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class =>
        Content.HasValue ? Option<TResult>.Some(map(Content.Value)) : Option<TResult>.None();
    public ValueOption<TResult> MapValue<TResult>(Func<T, TResult> map) where TResult : struct =>
        new() { Content = Content.HasValue ? map(Content.Value) : null };

    public Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map) where TResult : class =>
        Content.HasValue ? map(Content.Value) : Option<TResult>.None();
    public ValueOption<TResult> MapOptionalValue<TResult>(Func<T, ValueOption<TResult>> map) where TResult : struct =>
        Content.HasValue ? map(Content.Value) : ValueOption<TResult>.None();

    public T Reduce(T orElse) => Content ?? orElse;
    public T Reduce(Func<T> orElse) => Content ?? orElse();

    public ValueOption<T> Where(Func<T, bool> predicate) =>
        Content.HasValue && predicate(Content.Value) ? this : ValueOption<T>.None();

    public ValueOption<T> WhereNot(Func<T, bool> predicate) =>
        Content.HasValue && !predicate(Content.Value) ? this : ValueOption<T>.None();

    public override int GetHashCode() => Content?.GetHashCode() ?? 0;
    public override bool Equals(object? obj) => obj is ValueOption<T> option && Equals(option);

    public bool Equals(ValueOption<T> other) =>
        Content.HasValue 
            ? other.Content.HasValue && Content.Value.Equals(other.Content.Value)
            : !other.Content.HasValue;

    public static bool operator ==(ValueOption<T> a, ValueOption<T> b) => a.Equals(b);
    public static bool operator !=(ValueOption<T> a, ValueOption<T> b) => !(a.Equals(b));
}