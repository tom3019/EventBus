namespace EventBus.Subscriptions.Exceptions;

/// <summary>
/// SubscriptionCollectionReadOnlyException
/// </summary>
public class SubscriptionCollectionReadOnlyException() : Exception("Subscription collection is read-only.");