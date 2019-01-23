using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeScales.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        TradeScalesContext dbContext;
        public TradeScalesContext Init()
        {
            return dbContext ?? (dbContext = new TradeScalesContext());
        }
        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
