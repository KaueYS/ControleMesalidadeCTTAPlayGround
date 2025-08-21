using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControleMesalidadeCTTAPlayGround.Data;
using ControleMesalidadeCTTAPlayGround.Models;

namespace ControleMesalidadeCTTAPlayGround.Controllers
{
    public class AssociadoController : Controller
    {
        private readonly AppDbContext _context;

        public AssociadoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Associado
        public async Task<IActionResult> Index()
        {
            var associadoPagamento = await _context.Associados.Include(x => x.Pagamentos).ToListAsync();
            ViewBag.associadoPagto = associadoPagamento;
            return View(associadoPagamento);

        }


        public IActionResult Details(int id)
        {
            var socio = _context.Associados.Include(s => s.Pagamentos).FirstOrDefault(s => s.Id == id);
            if (socio == null) return NotFound();
            return View(socio);
        }


        // GET: Associado/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var associadoModel = await _context.Associados
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (associadoModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(associadoModel);
        //}

        // GET: Associado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Associado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Documento,Email,Telefone,Endereco,DataAniversario,Categoria,Equipamento,Necessidade,Ativo")] AssociadoModel associadoModel)
        {
            if (ModelState.IsValid)
            {
                associadoModel.Ativo = true;
                _context.Add(associadoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(associadoModel);
        }

        // GET: Associado/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var associadoModel = await _context.Associados.FindAsync(id);
            if (associadoModel == null)
            {
                return NotFound();
            }
            return View(associadoModel);
        }

        // POST: Associado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Documento,Email,Telefone,Endereco,DataAniversario,Categoria,Equipamento,Necessidade,Ativo")] AssociadoModel associadoModel)
        {
            if (id != associadoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(associadoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssociadoModelExists(associadoModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(associadoModel);
        }

        // GET: Associado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var associadoModel = await _context.Associados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (associadoModel == null)
            {
                return NotFound();
            }

            return View(associadoModel);
        }

        // POST: Associado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var associadoModel = await _context.Associados.FindAsync(id);
            if (associadoModel != null)
            {
                _context.Associados.Remove(associadoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssociadoModelExists(int id)
        {
            return _context.Associados.Any(e => e.Id == id);
        }
    }
}
