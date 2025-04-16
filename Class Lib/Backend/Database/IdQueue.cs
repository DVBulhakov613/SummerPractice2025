using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Database
{
    public static class IdQueue
    {
        private static Queue<uint> _reusableIDs = new Queue<uint>();
        private static uint _nextID = 0;

        public static uint GetNextID()
        {
            if (_reusableIDs.Count > 0)
            {
                return _reusableIDs.Dequeue();
            }
            return _nextID++;
        }

        public static void ReuseID(uint id)
        {
            _reusableIDs.Enqueue(id);
        }
    }
}
