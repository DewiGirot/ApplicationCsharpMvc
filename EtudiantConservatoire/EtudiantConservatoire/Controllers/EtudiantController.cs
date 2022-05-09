using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EtudiantConservatoire.Data;
using EtudiantConservatoire.Models;

namespace EtudiantConservatoire.Controllers
{
    public class EtudiantController : Controller
    {
        private readonly EtudiantConservatoireContext _context;

        public EtudiantController(EtudiantConservatoireContext context)
        {
            _context = context;
        }

        /*
         * Méthode index, liste tous les élèves de la base de donnée, permet de les filtrer et
         * d'accéder aux méthodes Create, Delete et Details
         */
        public ActionResult Index(string? Nom, string? Prenom, string? Email)
        {

            IQueryable<string> filtreQuery = from m in _context.Etudiant
                                             orderby m.Nom, m.Prenom
                                             select m.Nom;

            var student = from m in _context.Etudiant
                          select m;

            if (!string.IsNullOrEmpty(Nom))
            {
                student = student.Where(s => s.Nom.Contains(Nom));
                filtreQuery = from m in _context.Etudiant
                              orderby m.Nom
                              select m.Nom;
            }
            
            if (!string.IsNullOrEmpty(Prenom))
            {
                student = student.Where(s => s.Prenom.Contains(Prenom));
                filtreQuery = from m in _context.Etudiant
                              orderby m.Prenom
                              select m.Prenom;
            }

            if (!string.IsNullOrEmpty(Email))
            {
                student = student.Where(s => s.Mail.Contains(Email));
                filtreQuery = from m in _context.Etudiant
                              orderby m.Mail
                              select m.Mail;
            }

            var studentFiltreQuery = new EtudiantModel
            {
                FiltreList = new SelectList( filtreQuery.Distinct().ToList()),
                Students = student.ToList()
            };

            return View(studentFiltreQuery);
        }

        // GET: Etudiant/Details/5
        public async Task<IActionResult> Details(string id)
        {
            return View(FindById(id));
        }

        // GET: Etudiant/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Etudiant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Prenom,Genre,DateNaissance,Mail,Telephone,CodePostal,Adresse,Ville")] Etudiant etudiant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(etudiant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(etudiant);
        }

        /*
         * Les méthodes suivantes sont pour éditer les élèves
         * 
         * 1ere méthode : méthode GET pour récupérer l'élève
         * 2eme méthode : méthode POST pour modifier l'élève
         */
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return View(FindById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Prenom,Genre,DateNaissance,Mail,Telephone,CodePostal,Adresse,Ville")] Etudiant etudiant)
        {
            if (!id.Equals(etudiant.Id))
            {
                return NotFound();
            }

            //id = int.Parse(EtudiantModel.Decrypt("" + id));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(etudiant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EtudiantExists(etudiant.Id))
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
            return View(etudiant);
        }

        /*
         * Les méthodes suivantes sont pour supprimer les élèves
         * 
         * 1ere méthode : méthode GET pour récupérer l'élève
         * 2eme méthode : méthode POST pour confirmer la supression de l'élève
         */
        public async Task<IActionResult> Delete(string id)
        {
            return View(FindById(id));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string decryptedId = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    decryptedId = EtudiantModel.Decrypt(id);
                }
                catch (Exception)
                {
                }
            }

            var etudiant = await _context.Etudiant.FindAsync(int.Parse(decryptedId));
            try
            {
                _context.Etudiant.Remove(etudiant);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        /*
         * Sert à connaître si un élève existe ou non dans la base de donnée 
         */
        private bool EtudiantExists(int id)
        {
            return _context.Etudiant.Any(e => e.Id == id);
        }

        /*
         * Cette méthode récupère l'ID d'un élève, le décode puis renvoie cet élève
         */
        private Etudiant FindById(string id)
        {
            Etudiant etudiant = null;
            string decryptedId = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    decryptedId = EtudiantModel.Decrypt(id);
                }
                catch (Exception)
                {
                }
            }

            if (int.TryParse(decryptedId, out int etudiantId) && etudiantId > 0)
            {
                etudiant = _context.Etudiant.Find(etudiantId);
            }

            if (etudiant == null)
            {
                return null;
            }
            return etudiant;
        }
    }
}