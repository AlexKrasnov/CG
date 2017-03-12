using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    class ScharrFilter : SobelFilter // Выделение границ (оператор Щарра)
    {
        public ScharrFilter()
        {
            Y = new int[3, 3] { { 3, 10, 3 }, { 0, 0, 0 }, { -3, -10, -3 } };
            X = new int[3, 3] { { 3, 0, -3 }, { 10, 0, -10 }, { 3, 0, -3 } };
        }
    }
}
