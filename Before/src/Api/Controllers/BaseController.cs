using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BaseController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public BaseController(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        protected new IActionResult Ok()
        {
            this._unitOfWork.Commit();
            return base.Ok();
        }

        protected IActionResult Ok<T>(T result)
        {
            this._unitOfWork.Commit();
            return base.Ok(result);
        }

        protected IActionResult Error(string error)
        {
            return BadRequest(error);
        }
    }
}
