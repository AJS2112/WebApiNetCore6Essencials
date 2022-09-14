using APICatalogo.Contexts;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController] 
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CategoriasController(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow; 
        _mapper = mapper;
    }

    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
    {
        try
        {
            var categorias = await _uow.CategoriaRepository.Get().ToListAsync();
            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDTO;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação");    
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCagegoriasPorProduto()
    {
        try
        {
            var categorias = await _uow.CategoriaRepository.GetCategoriasProdutos();
            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDTO;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação");
        }
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
    public async Task<ActionResult<CategoriaDTO>> Get(int id)
    {
        try
        {
            var categoria = await _uow.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria == null)
                return NotFound();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDTO);

        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação");
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post(CategoriaDTO categoriaDTO)
    {
        try
        {
            if (categoriaDTO == null)
                return BadRequest();

            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            _uow.CategoriaRepository.Add(categoria);
            await _uow.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> Put(int id, CategoriaDTO categoriaDTO)
    {
        try
        {
            if (id != categoriaDTO.CategoriaId)
                return BadRequest();

            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            _uow.CategoriaRepository.Update(categoria);
            await _uow.Commit();
            return Ok(categoriaDTO);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var categoria = await _uow.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria == null)
                return NotFound();

            _uow.CategoriaRepository.Delete(categoria);
            await _uow.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(categoriaDTO);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação");
        }
    }

}
