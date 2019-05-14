using System;

namespace WBS.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        WBSContext Init();
    }
}
