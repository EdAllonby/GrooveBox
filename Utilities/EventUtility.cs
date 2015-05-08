using System;

namespace Utilities
{
    /// <summary>
    /// Utility methods for Events, such as firing safely.
    /// </summary>
    public static class EventUtility
    {
        /// <summary>
        /// Safely fires an event with no <see cref="EventArgs" />.
        /// </summary>
        /// <param name="unsafeEvent">The event to safely fire.</param>
        /// <param name="sender">The class that sent the event.</param>
        public static void SafeFireEvent(EventHandler unsafeEvent, object sender)
        {
            var eventCopy = unsafeEvent;

            if (eventCopy != null)
            {
                eventCopy(sender, new EventArgs());
            }
        }

        /// <summary>
        /// Safely fires an event with type <see cref="EventArgs" />.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="EventArgs" /> to fire.</typeparam>
        /// <param name="unsafeEvent">The event to safely fire.</param>
        /// <param name="sender">The class that called the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> the event has.</param>
        public static void SafeFireEvent<T>(EventHandler<T> unsafeEvent, object sender, T eventArgs) where T : EventArgs
        {
            var eventCopy = unsafeEvent;

            if (eventCopy != null)
            {
                eventCopy(sender, eventArgs);
            }
        }
    }
}