﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Transfer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        protected readonly ITransferService _transferService;

        public TransfersController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<TransferLog>> Get()
        {
            return Ok(_transferService.GetTransferLogs());
        }

        
    }
}
