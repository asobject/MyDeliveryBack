

namespace BuildingBlocks.Messaging.Events;

public record UserVerifiedEvent(
    int OrderId,
    string? ReceiverUserId);