using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBS.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        WBSContext dbContext;
        public WBSContext Init()
        {
            return dbContext ?? (dbContext = new WBSContext());
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
