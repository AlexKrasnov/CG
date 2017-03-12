using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    class PrewitteFilter : SobelFilter // Выделение границ (оператор Прюитта)
    {
        public PrewitteFilter()
        {
            X = new int[3, 3] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
            Y = new int[3, 3] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
        }
    }
}
