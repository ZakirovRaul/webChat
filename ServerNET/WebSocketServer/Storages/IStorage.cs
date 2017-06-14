using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServer.Storages
{
    public interface IStorage<T>
    {
        void Add(T item);
        object GetAll();
        void Remove(T item);
    }
}
