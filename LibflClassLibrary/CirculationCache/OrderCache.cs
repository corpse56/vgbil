using LibflClassLibrary.Books;
using LibflClassLibrary.Circulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.CirculationCache
{
    public class OrderCache
    {
        MemoryCache cache_ = new MemoryCache("ExemplarCache");
        public OrderInfo GetOrder(int id)
        {
            OrderInfo result;
            if (cache_.Contains(id.ToString()))
            {
                result = (OrderInfo)cache_[id.ToString()];
            }
            else
            {
                CirculationInfo ci = new CirculationInfo();
                result = ci.GetOrder(id);
                Add(result);
            }
            return result;
        }
        private void Add(OrderInfo order)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(3);
            cache_.Add(order.OrderId.ToString(), order, policy);
        }

    }
}
