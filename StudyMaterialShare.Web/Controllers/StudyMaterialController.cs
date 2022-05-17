using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyMaterialShare.Database.Models;
using StudyMaterialShare.Database.Repositories;
using StudyMaterialShare.Web.Models;
using System.Linq.Expressions;

namespace StudyMaterialShare.Web.Controllers
{
    public class StudyMaterialController : Controller
    {
        private readonly static int _pageSize = 20;
        private readonly StudyMaterialRepository _studyMaterialRepository;
        private readonly SubjectRepository _subjectRepository;
        private readonly RatingRepository _ratingRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;


        public StudyMaterialController(StudyMaterialRepository studyMaterialRepository,
            SubjectRepository subjectRepository,
            RatingRepository ratingRepository,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMapper _mapper)
        {
            _studyMaterialRepository = studyMaterialRepository;
            _subjectRepository = subjectRepository;
            _ratingRepository = ratingRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            this._mapper = _mapper;
        }
        
        // GET: StudyMaterial
        public IActionResult Index()
        {
            var studyMaterials = _studyMaterialRepository
                .Get(null,query => query.OrderByDescending(sm => sm.UploadedAt));
            return View(_mapper
                .Map<IEnumerable<StudyMaterial>,IEnumerable<StudyMaterialViewModel>>(studyMaterials));
        }
        
        public async Task<IActionResult> Browse(BrowseQueryViewModel vm)
        {
            ViewBag.Page = vm.Page;
            ViewBag.Sort = vm.Sort;
            ViewBag.Order = vm.Order;
            ViewBag.Subject = vm.Subject;
            ViewBag.TitleFilter = vm.TitleFilter;
            ViewBag.Subjects = new SelectList(_subjectRepository.Get(),"Name","Name");

            if(_signInManager.IsSignedIn(User))
            {
                string userId = (await _userManager.GetUserAsync(User)).Id;
                ViewBag.RatingsForUser = _ratingRepository
                    .Get(rating => rating.User.Id == userId, null);
            }

            IEnumerable<StudyMaterial> studyMaterials = _studyMaterialRepository
                .Get(GetFilter(vm.Subject,vm.TitleFilter), query => GetOrder(query,vm.Order,vm.Sort))
                .Skip(_pageSize * vm.Page)
                .Take(_pageSize);

            ViewBag.EndPage = studyMaterials.Count() < _pageSize;

            return View(_mapper
                .Map<IEnumerable<StudyMaterial>,IEnumerable<StudyMaterialViewModel>>(studyMaterials));
        }
        
        [Authorize]
        public FileResult? Download(int id)
        {
            var studyMaterial = _studyMaterialRepository.Get(id);

            if (studyMaterial == null) return null;

            var file = studyMaterial.File.ToArray();
            studyMaterial.Downloads += 1;

            if (_studyMaterialRepository.Update(studyMaterial) == null) return null;

            if (file == null) return null;

            return File(file, "application/octet-stream",
                studyMaterial.Title.Replace(" ","_") + "." + studyMaterial.FileType);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rate(int id,int rateValue,BrowseQueryViewModel browseQuery)
        {
            var studyMaterial = _studyMaterialRepository.Get(id);
            var user = (await _userManager.GetUserAsync(User));

            if(studyMaterial == null)
            {
                ModelState.AddModelError("", "Ilyen tananyag nem létezik");
                return RedirectToAction("Browse", "StudyMaterial", browseQuery);
            }

            var newRating = _ratingRepository.Create(studyMaterial,user,rateValue);

            if (newRating is null)
            {
                ViewBag.RateError = "Hiba történt az értékelés közben!";
            }

            return RedirectToAction("Browse", "StudyMaterial", browseQuery);
        }

        // GET: StudyMaterial/Create
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Subjects = new SelectList(_subjectRepository.Get(),"Name","Name");
            return View();
        }

        // POST: StudyMaterial/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(StudyMaterialViewModel vm, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Subjects = new SelectList(_subjectRepository.Get(), "Name", "Name");
                return View(vm);
            }

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file","A fájl megadása kötelező!");
                ViewBag.Subjects = new SelectList(_subjectRepository.Get(), "Name", "Name");
                return View(vm);
            }

            var subject = _subjectRepository.Get(subject => subject.Name == vm.Subject);
            if (subject == null)
            {
                ModelState.AddModelError("Subject", "Ilyen tárgy nem létezik!");
                ViewBag.Subjects = new SelectList(_subjectRepository.Get(), "Name", "Name");
                return View(vm);
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                vm.File = stream.ToArray();
            }
            var user = await _userManager.GetUserAsync(User);
            var studyMaterial = new StudyMaterial()
            {
                Title = vm.Title,
                FileType = file.FileName.Split('.')[1],
                Subject = subject,
                User = user,
                UploadedAt = DateTime.Now,
            };

            studyMaterial.File = vm.File.ToArray();

            if(_studyMaterialRepository.Create(studyMaterial) == null)
            {
                ModelState.AddModelError("", "Hiba történt a létrehozás során!");
                ViewBag.Subjects = new SelectList(_subjectRepository.Get(), "Name", "Name");
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        private static IOrderedQueryable<StudyMaterial> GetOrder(IQueryable<StudyMaterial> query,string order,string sort)
        {
            IOrderedQueryable<StudyMaterial> orderedQuery;
            if (order == "asc")
            {
                if (sort == "upload") orderedQuery = query
                        .OrderBy(x => x.UploadedAt);
                else if (sort == "rating") orderedQuery = query
                        .OrderBy(x => x.Ratings.Average(r => r.RateValue));
                else orderedQuery = query
                    .OrderBy(x => x.Title);
            }
            else
            {
                if (sort == "upload") orderedQuery = query
                     .OrderByDescending(x => x.UploadedAt);
                else if (sort == "rating") orderedQuery = query
                     .OrderByDescending(x => x.Ratings.Average(r => r.RateValue));
                else orderedQuery = query
                    .OrderByDescending(x => x.Title);
            }

            return orderedQuery;
        }

        private static Expression<Func<StudyMaterial, bool>>? GetFilter(string subject, string title)
        {
            if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(title))
            {
                return sm => sm.Subject.Name == subject && sm.Title.Contains(title);
            }
            else if (!string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(title))
            {
                return sm => sm.Subject.Name == subject;
            }
            else if (string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(title))
            {
                return sm => sm.Title.Contains(title);
            }
            else
            {
                return null;
            }
        }
    }
}
