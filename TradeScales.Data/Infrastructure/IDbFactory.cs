using System;

namespace TradeScales.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        TradeScalesContext Init();
    }
}
