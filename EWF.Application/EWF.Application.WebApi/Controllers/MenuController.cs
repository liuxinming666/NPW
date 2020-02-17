using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private IMenuService menuService;
        public MenuController(IMenuService _menuService)
        {
            menuService = _menuService;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return menuService.GetUserTopMenu(null);
        }

        [HttpGet]
        public ActionResult<string> GetTopMenu()
        {
            return menuService.GetUserTopMenu(null);
        }

        // GET api/values/5
        [HttpGet("{parentCode}")]
        public ActionResult<string> GetUserMenuByParentCode(string parentCode)
        {
            return menuService.GetUserMenuByParentCode(null, parentCode);
        }

        // GET api/values/5
        [HttpGet("{code}")]
        public ActionResult<string> GetTopMenu(string code)
        {
            return menuService.GetUserTopMenu(null);
        }

        public string Options()
        {
            return null;
        }
    }
}