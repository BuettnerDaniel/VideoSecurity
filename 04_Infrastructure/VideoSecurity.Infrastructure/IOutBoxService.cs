using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSecurity.Infrastructure
{
    public interface IOutBoxService
    {
        /// <summary>
        /// Speichert ein Integration-Event atomar zusammen mit der Business-Transaktion in der Datenbank.
        /// Dies garantiert "At-Least-Once Delivery" auch bei Systemabstürzen.
        /// </summary>
        /// <typeparam name="T">Der Typ des Events (z.B. VideoSegmentCreatedEvent)</typeparam>
        /// <param name="integrationEvent">Das Event-Objekt</param>
        Task PublishAsync<T>(T integrationEvent) where T : class;
    }
}
