/*
 * Controllers/FornecedoresController.cs (VERSÃO FINAL)
 * Agora com upload de fotos no CREATE e no EDIT.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DESAFIOCRUD.Data;
using DESAFIOCRUD.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DESAFIOCRUD.Controllers
{
    public class FornecedoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FornecedoresController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; 
        }

        // GET: Fornecedores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fornecedores.ToListAsync());
        }

        // GET: Fornecedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var fornecedor = await _context.Fornecedores.FirstOrDefaultAsync(m => m.Id == id);
            if (fornecedor == null) return NotFound();
            return View(fornecedor);
        }

        // GET: Fornecedores/Create
        public IActionResult Create()
        {
            ViewData["SegmentoList"] = new SelectList(Enum.GetValues<Segmento>());
            return View();
        }

        // POST: Fornecedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Cnpj,Segmento,Cep,Endereco,FotoArquivo")] Fornecedor fornecedor)
        {
            ModelState.Remove("FotoCaminho");

            if (ModelState.IsValid)
            {
                if (fornecedor.FotoArquivo != null && fornecedor.FotoArquivo.Length > 0)
                {
                    if (fornecedor.FotoArquivo.ContentType != "image/png")
                    {
                        ModelState.AddModelError("FotoArquivo", "O arquivo deve ser uma imagem no formato PNG.");
                        ViewData["SegmentoList"] = new SelectList(Enum.GetValues<Segmento>());
                        return View(fornecedor);
                    }

                    string nomeArquivoUnico = Guid.NewGuid().ToString() + Path.GetExtension(fornecedor.FotoArquivo.FileName);
                    string pastaUploads = Path.Combine(_webHostEnvironment.WebRootPath, "imagens");
                    
                    if (!Directory.Exists(pastaUploads)) Directory.CreateDirectory(pastaUploads);

                    string caminhoCompletoArquivo = Path.Combine(pastaUploads, nomeArquivoUnico);

                    using (var stream = new FileStream(caminhoCompletoArquivo, FileMode.Create))
                    {
                        await fornecedor.FotoArquivo.CopyToAsync(stream);
                    }
                    fornecedor.FotoCaminho = "/imagens/" + nomeArquivoUnico;
                }
                
                _context.Add(fornecedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["SegmentoList"] = new SelectList(Enum.GetValues<Segmento>());
            return View(fornecedor);
        }

        // GET: Fornecedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null) return NotFound();
            
            ViewData["SegmentoList"] = new SelectList(Enum.GetValues<Segmento>(), fornecedor.Segmento);
            return View(fornecedor);
        }

        // POST: Fornecedores/Edit/5
        // (*** ESTE É O MÉTODO QUE FOI ATUALIZADO ***)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cnpj,Segmento,Cep,Endereco,FotoCaminho,FotoArquivo")] Fornecedor fornecedor)
        {
            if (id != fornecedor.Id) return NotFound();

            // remover o FotoArquivo da validação caso o usuário
            // caso não queira trocar a foto, senão o ModelState.IsValid falha.
            if (fornecedor.FotoArquivo == null)
            {
                 ModelState.Remove("FotoArquivo");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // --- INÍCIO DA LÓGICA DE UPLOAD (EDIT) ---
                    // Se um NOVO arquivo foi enviado, processa ele
                    if (fornecedor.FotoArquivo != null && fornecedor.FotoArquivo.Length > 0)
                    {
                        if (fornecedor.FotoArquivo.ContentType != "image/png")
                        {
                            ModelState.AddModelError("FotoArquivo", "O arquivo deve ser uma imagem no formato PNG.");
                            ViewData["SegmentoList"] = new SelectList(Enum.GetValues<Segmento>(), fornecedor.Segmento);
                            return View(fornecedor);
                        }

                        // (Lógica Opcional: deletar a foto antiga do disco)
                        // string fotoAntiga = fornecedor.FotoCaminho;
                        // if (!string.IsNullOrEmpty(fotoAntiga)) { ... File.Delete(...) ... }
                        
                        // Salva a nova foto
                        string nomeArquivoUnico = Guid.NewGuid().ToString() + Path.GetExtension(fornecedor.FotoArquivo.FileName);
                        string pastaUploads = Path.Combine(_webHostEnvironment.WebRootPath, "imagens");
                        string caminhoCompletoArquivo = Path.Combine(pastaUploads, nomeArquivoUnico);

                        using (var stream = new FileStream(caminhoCompletoArquivo, FileMode.Create))
                        {
                            await fornecedor.FotoArquivo.CopyToAsync(stream);
                        }
                        // ATUALIZA o caminho da foto para o novo arquivo
                        fornecedor.FotoCaminho = "/imagens/" + nomeArquivoUnico;
                    }
                    // Se nenhum arquivo novo foi enviado, o "FotoCaminho" (que veio
                    // do <input type="hidden">) será mantido, preservando a foto antiga.
                    // --- FIM DA LÓGICA DE UPLOAD (EDIT) ---

                    _context.Update(fornecedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FornecedorExists(fornecedor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            // Se o ModelState falhou, recarrega o dropdown e retorna
            ViewData["SegmentoList"] = new SelectList(Enum.GetValues<Segmento>(), fornecedor.Segmento);
            return View(fornecedor);
        }

        // GET: Fornecedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var fornecedor = await _context.Fornecedores.FirstOrDefaultAsync(m => m.Id == id);
            if (fornecedor == null) return NotFound();
            return View(fornecedor);
        }

        // POST: Fornecedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor != null)
            {
                // (Lógica Opcional: aqui poderíamos deletar a foto do disco)
                _context.Fornecedores.Remove(fornecedor);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FornecedorExists(int id)
        {
            return _context.Fornecedores.Any(e => e.Id == id);
        }
    }
}