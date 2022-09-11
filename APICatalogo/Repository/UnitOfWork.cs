using APICatalogo.Contexts;

namespace APICatalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private CategoriaRepository _categoriasRepository;
        private ProdutoRepository _produtoRepository;
        public AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public ICategoriaRepository CategoriaRepository 
        { 
            get { return _categoriasRepository ?? new CategoriaRepository(_context); }
        }

        public IProdutoRepository ProdutoRepository
        {
            get { return _produtoRepository ?? new ProdutoRepository(_context); }
        }

        public void Commit()
        {
            _context.SaveChanges(); 
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
