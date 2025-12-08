using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.Features.PropertyUnit.Queries.GetAll;
using RealState.Core.Application.Features.PropertyUnit.Queries.GetById;
using RealState.Core.Application.Features.PropertyUnit.Queries.GetByIdCode;

namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PropertyUnitController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyUnitDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var propertyUnits = await Mediator.Send(new GetByIdPropertyUnitQuery() { Id = id});
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

        [HttpGet("{Code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyUnitDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCode(int Code)
        {
            try
            {
                var propertyUnits = await Mediator.Send(new GetByCodePropertyUnitQuery() { Code = Code });
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
