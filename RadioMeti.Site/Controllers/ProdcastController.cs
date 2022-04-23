﻿using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Prodcast;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Controllers
{
    public class ProdcastController : SiteBaseController
    {
        private readonly IProdcastService _prodcastService;

        public ProdcastController(IProdcastService prodcastService)
        {
            _prodcastService = prodcastService;
        }

        [HttpGet("/prodcasts")]
        public async Task<ActionResult> Index()
        {
            var model = new ProdcastPageDto
            {
                Sliders = await _prodcastService.GetInSliderProdcasts(),
                NewestProdcasts = await _prodcastService.GetNewestProdcasts(25),
                PopularProdcasts = await _prodcastService.GetPopularProdcasts(25),
                ThisMonthProdcasts = await _prodcastService.GetProdcastsByStartDate(30, 16),
                ThisWeekProdcasts = await _prodcastService.GetProdcastsByStartDate(7, 16),
                ThisDayProdcasts = await _prodcastService.GetProdcastsByStartDate(1, 16),
            };
            return View(model);
        }

        [HttpGet("/prodcast/{id}")]
        public async Task<IActionResult> ShowProdcast(long id)
        {
            var prodcast = await _prodcastService.GetProdcastForSiteBy(id);
            if (prodcast == null) return NotFound();
            await _prodcastService.AddPlaysProdcast(prodcast);
            var model = new ShowProdcastPageDto
            {
                Prodcast = prodcast,
                RelatedProdcasts = await _prodcastService.GetRelatedProdcast(prodcast.DjId),
            };
            return View(model);
        }
        [HttpGet("/prodcast/all")]
        public async Task<IActionResult> ShowAllProdcast()
        {
            return View(await _prodcastService.GetAllProdcastForSite());
        }
    }
}