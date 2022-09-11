using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow; 
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = _uow.ProdutoRepository.GetProdutos(produtosParameters);
            if (produtos is null)
                return NotFound("Produtos Não Encontrados");

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDTO;
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uow.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto == null)
                return NotFound("Produto não encontrado");

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return produtoDTO; 
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetByPreco()
        {
            var produtos = _uow.ProdutoRepository.GetProdutosPorPreco().ToList();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDTO;
        }

        [HttpPost]
        public ActionResult Post(ProdutoDTO produtoDTO)
        {
            if (produtoDTO == null)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            _uow.ProdutoRepository.Add(produto);
            _uow.Commit();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId}, produtoDTO);
        }

        [HttpPut("{int:id:min(1)}")]
        public ActionResult Put(int id, ProdutoDTO produtoDTO)
        {
            if (produtoDTO.ProdutoId == id)    
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            _uow.ProdutoRepository.Update(produto);
            _uow.Commit();

            return Ok(produtoDTO);
        }

        [HttpDelete("{int:id:min(1)}")]
        public ActionResult Delete(int id)
        {
            var produto = _uow.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto == null)
                return NotFound("Produto não encontrado");

            _uow.ProdutoRepository.Delete(produto);
            _uow.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }
    }
}
