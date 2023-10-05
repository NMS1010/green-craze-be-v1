﻿using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Unit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitsController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUnit([FromQuery] GetUnitPagingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _unitService.GetAllUnit(request);

            return Ok(APIResponse<PaginatedResult<UnitDto>>.Create(res, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitById([FromRoute] long id)
        {
            var res = await _unitService.GetUnit(id);

            return Ok(APIResponse<UnitDto>.Create(res, StatusCodes.Status200OK));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUnit([FromBody] CreateUnitRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _unitService.CreateUnit(request);

            return Ok(APIResponse<bool>.Create(res, StatusCodes.Status201Created));
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUnit([FromBody] UpdateUnitRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _unitService.UpdateUnit(request);

            return Ok(APIResponse<bool>.Create(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUnit([FromRoute] long id)
        {
            var res = await _unitService.DeleteUnit(id);

            return Ok(APIResponse<bool>.Create(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("delete/all")]
        public async Task<IActionResult> DeleteManyUnit([FromQuery] List<long> ids)
        {
            var res = await _unitService.DeleteMany(ids);

            return Ok(APIResponse<bool>.Create(res, StatusCodes.Status204NoContent));
        }
    }
}