﻿using System.Collections.Generic;
using ServiceStack.Webhooks.ServiceModel.Types;

namespace ServiceStack.Webhooks
{
    public interface ISubscriptionStore
    {
        /// <summary>
        ///     Returns the identity of the added subscription
        /// </summary>
        string Add(WebhookSubscription subscription);

        /// <summary>
        ///     Returns all subscription for the specified userId
        /// </summary>
        List<WebhookSubscription> Find(string userId);

        /// <summary>
        ///     Gets the subscription for the specified user and eventName
        /// </summary>
        WebhookSubscription Get(string userId, string eventName);

        /// <summary>
        ///     Gets the specified subscription
        /// </summary>
        WebhookSubscription Get(string subscriptionId);

        /// <summary>
        ///     Updates the specified subscription
        /// </summary>
        void Update(string subscriptionId, WebhookSubscription subscription);

        /// <summary>
        ///     Deletes the specified subscription
        /// </summary>
        void Delete(string subscriptionId);

        /// <summary>
        ///     Returns all subscription configurations for all users for the specified event,
        ///     and optionally whether they are currently active or not
        /// </summary>
        List<SubscriptionRelayConfig> Search(string eventName, bool? isActive);

        /// <summary>
        ///     Adds a new delivery result to the subscription
        /// </summary>
        void Add(string subscriptionId, SubscriptionDeliveryResult result);

        /// <summary>
        ///     Returns the top specified delivery results for the specified subscription (in descending date order)
        /// </summary>
        List<SubscriptionDeliveryResult> Search(string subscriptionId, int top);
    }
}