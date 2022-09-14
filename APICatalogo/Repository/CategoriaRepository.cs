using APICatalogo.Contexts;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return await Get().Include(c => c.Produtos).ToListAsync();
        }
    }
}
