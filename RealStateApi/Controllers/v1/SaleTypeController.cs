using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.SaleType;
using RealState.Core.Application.Features.SaleType.Commands.CreateSaleType;
using RealState.Core.Application.Features.SaleType.Commands.DeleteSaleType;
using RealState.Core.Application.Features.SaleType.Commands.UpdateSaleType;
using RealState.Core.Application.Features.SaleType.Queries.GetAll;
using RealState.Core.Application.Features.SaleType.Queries.GetById;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class SaleTypeController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de tipos de venta",
            Description = "Obtiene todos los tipos de venta sin filtros."
        )]
        public async Task<IActionResult> Get()
        {
            try
            {
                var saleTypes = await Mediator.Send(new GetAllSaleTypeQuery());
                if (saleTypes is null || saleTypes.Count == 0)
                {
                    return NoContent();
                }

                return Ok(saleTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Buscar tipo de venta por ID",
            Description = "Obtiene un tipo de venta usando su Id."
        )]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var saleTypes = await Mediator.Send(new GetByIdSaleTypeQuery() { Id = id });
                if (saleTypes == null)
                {
                    return NoContent();
                }

                return Ok(saleTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Crear un nuevo tipo de venta",
            Description = "Crea un tipo de venta enviando los datos necesarios."
        )]
        public async Task<IActionResult> Create([FromBody] CreateSaleTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await Mediator.Send(command);
                if (result == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Creation failed");
                }

                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Actualizar tipo de venta",
            Description = "Actualiza un tipo de venta existente usando su Id."
        )]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSaleTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (id != command.Id)
                {
                    return BadRequest();
                }

                await Mediator.Send(command);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Eliminar tipo de venta",
            Description = "Elimina un tipo de venta usando su Id."
        )]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                await Mediator.Send(new DeleteSaleTypeCommand() { Id = id });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
