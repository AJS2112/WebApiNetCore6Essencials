﻿namespace APICatalogo.Repository
{
    public interface IUnitOfWork
    {
        ICategoriaRepository CategoriaRepository { get; }
        IProdutoRepository ProdutoRepository { get; }
        void Commit();
    }
}
