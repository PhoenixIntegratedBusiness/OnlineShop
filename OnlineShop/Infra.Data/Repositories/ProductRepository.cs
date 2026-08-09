using Domain.Interfaces;
using Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Repositories
{
    
    public class ProductRepository:IProductRepository
    {
        private readonly MyContext _context;
        public ProductRepository(MyContext context)
        {
            _context = context;
        }


    }
}
