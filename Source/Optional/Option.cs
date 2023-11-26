namespace CodingHelmet.Optional;

public readonly struct Option<T> : IEquatable<Option<T>> where T : class
{
    private T? Content { get; init; }
    
    public static Option<T> Some(T obj) => new() { Content = obj };
    public static Option<T> None() => new();

    public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class =>
        new() { Content = Content is not null ? map(Content) : null };
    public ValueOption<TResult> MapValue<TResult>(Func<T, TResult> map) where TResult : struct =>
        Content is not null ? ValueOption<TResult>.Some(map(Content)) : ValueOption<TResult>.None();

    public Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map) where TResult : class =>
        Content is not null ? map(Content) : Option<TResult>.None();
    public ValueOption<TResult> MapOptionalValue<TResult>(Func<T, ValueOption<TResult>> map) where TResult : struct =>
        Content is not null ? map(Content) : ValueOption<TResult>.None();

    public T Reduce(T orElse) => Content ?? orElse;
    public T Reduce(Func<T> orElse) => Content ?? orElse();

    public Option<T> Where(Func<T, bool> predicate) =>
        Content is not null && predicate(Content) ? this : Option<T>.None();

    public Option<T> WhereNot(Func<T, bool> predicate) =>
        Content is not null && !predicate(Content) ? this : Option<T>.None();

    public override int GetHashCode() => Content?.GetHashCode() ?? 0;
    public override bool Equals(object? obj) => obj is Option<T> option && Equals(option);

    public bool Equals(Option<T> other) =>
        Content?.Equals(other.Content) ?? other.Content is null;

    public static bool operator ==(Option<T>? a, Option<T>? b) => a is null ? b is null : a.Equals(b);
    public static bool operator !=(Option<T>? a, Option<T>? b) => !(a == b);
}