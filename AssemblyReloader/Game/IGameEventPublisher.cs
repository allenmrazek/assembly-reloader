using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AssemblyReloader.Game
{
    public delegate void GameEventHandler<T>(T data);

    public interface IGameEventPublisher<T>
    {
        event GameEventHandler<T> OnEvent;

        void Raise(T data);
    }
}
