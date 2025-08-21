
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControleMesalidadeCTTAPlayGround.Data;
using ControleMesalidadeCTTAPlayGround.Models;

namespace ControleMesalidadeCTTAPlayGround.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly AppDbContext _context;

        public PagamentoController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult Pagamento(int id)
        {
            var socio = _context.Associados.FirstOrDefault(s => s.Id == id);
            if (socio == null) return NotFound();

            ViewBag.Socio = socio;
            return View();
        }

        [HttpPost]
        public IActionResult Pagamento(int id, decimal valorPago)
        {
            var socio = _context.Associados.Include(s => s.Pagamentos).FirstOrDefault(s => s.Id == id);
            if (socio == null) return NotFound();

            var dias = PagamentoModel.CalcularDiasAdimplencia(valorPago);

            var pagamento = new PagamentoModel
            {
                SocioId = id,
                DataPagamento = DateTime.Today,
                Valor = valorPago,
                DiasAdimplencia = dias
            };

            var vencimentoAnterior = socio.ObterVencimentoAtual();
            var novoVencimento = pagamento.CalcularVencimento(vencimentoAnterior);

            socio.Pagamentos.Add(pagamento);
            _context.SaveChanges();

            TempData["Msg"] = $"Pagamento de R$ {pagamento.Valor:F2} registrado! Adimplência de {pagamento.DiasAdimplencia} dias. Novo vencimento: {novoVencimento:dd/MM/yyyy}";
            return RedirectToAction("Index","Associado");
        }

        // GET: Pagamento
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Pagamentos.Include(p => p.Socio);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Pagamento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pagamentoModel = await _context.Pagamentos
                .Include(p => p.Socio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pagamentoModel == null)
            {
                return NotFound();
            }

            return View(pagamentoModel);
        }

        // GET: Pagamento/Create
        public IActionResult Create()
        {
            ViewData["SocioId"] = new SelectList(_context.Associados, "Id", "Id");
            return View();
        }

        // POST: Pagamento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SocioId,DataPagamento,DiasAdimplencia,Valor")] PagamentoModel pagamentoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pagamentoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SocioId"] = new SelectList(_context.Associados, "Id", "Id", pagamentoModel.SocioId);
            return View(pagamentoModel);
        }

        // GET: Pagamento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pagamentoModel = await _context.Pagamentos.FindAsync(id);
            if (pagamentoModel == null)
            {
                return NotFound();
            }
            ViewData["SocioId"] = new SelectList(_context.Associados, "Id", "Id", pagamentoModel.SocioId);
            return View(pagamentoModel);
        }

        // POST: Pagamento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SocioId,DataPagamento,DiasAdimplencia,Valor")] PagamentoModel pagamentoModel)
        {
            if (id != pagamentoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pagamentoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagamentoModelExists(pagamentoModel.Id))
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
            ViewData["SocioId"] = new SelectList(_context.Associados, "Id", "Id", pagamentoModel.SocioId);
            return View(pagamentoModel);
        }

        // GET: Pagamento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pagamentoModel = await _context.Pagamentos
                .Include(p => p.Socio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pagamentoModel == null)
            {
                return NotFound();
            }

            return View(pagamentoModel);
        }

        // POST: Pagamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pagamentoModel = await _context.Pagamentos.FindAsync(id);
            if (pagamentoModel != null)
            {
                _context.Pagamentos.Remove(pagamentoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagamentoModelExists(int id)
        {
            return _context.Pagamentos.Any(e => e.Id == id);
        }
    }
}
