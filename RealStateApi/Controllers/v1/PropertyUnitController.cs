using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.Features.PropertyUnit.Queries.GetAll;
using RealState.Core.Application.Features.PropertyUnit.Queries.GetById;
using RealState.Core.Application.Features.PropertyUnit.Queries.GetByIdCode;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PropertyUnitController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyUnitDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listar unidades",
            Description = "Devuelve todas las unidades de propiedad."
        )]
        public async Task<IActionResult> Get()
        {
            try
            {
                var propertyUnits = await Mediator.Send(new GetAllPropertyUnitQuery());
                if (propertyUnits is null || propertyUnits.Count == 0)
                {
                    return NoContent();
                }

                return Ok(propertyUnits);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyUnitDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Buscar unidad por ID",
            Description = "Obtiene una unidad usando su Id."
        )]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var propertyUnits = await Mediator.Send(new GetByIdPropertyUnitQuery() { Id = id });
                if (propertyUnits == null)
                {
                    return NoContent();
                }

                return Ok(propertyUnits);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("by-code/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyUnitDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Buscar unidad por código",
            Description = "Obtiene una unidad usando su código único."
        )]
        public async Task<IActionResult> GetByCode(int code)
        {
            try
            {
                var propertyUnits = await Mediator.Send(new GetByCodePropertyUnitQuery() { Code = code });
                if (propertyUnits == null)
                {
                    return NoContent();
                }

                return Ok(propertyUnits);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
