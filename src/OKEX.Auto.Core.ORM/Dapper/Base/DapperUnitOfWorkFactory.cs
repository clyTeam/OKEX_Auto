﻿namespace OKEX.Auto.Core.ORM.Dapper.Base
{
    public class DapperUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly DapperDBContext _context;

        public DapperUnitOfWorkFactory(DapperDBContext context)
        {
            _context = context;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_context);
        }
    }
}
