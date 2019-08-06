using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml;
using Cyriller.Model;
using Cyriller.Web.Models;

namespace Cyriller.Web.Controllers
{
    public class DeclineController : BaseController
    {
        protected int MaxPhrasesToDecline = 50;
        protected string TempFolder = Path.Combine(HttpRuntime.AppDomainAppPath, "Temp");

        [GzipFilter]
        public ActionResult Noun(string w)
        {
            ViewBag.Page = "Decline.Noun";

            if (string.IsNullOrEmpty(w))
            {
                return View();
            }

            List<string> errors = new List<string>();
            CyrNounCollection collection = this.NounCollection;
            List<CyrNounDeclineResult> results = new List<CyrNounDeclineResult>();

            foreach (string s in w.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                CyrNoun word;
                string foundWord;
                CasesEnum foundCase;
                NumbersEnum foundNumber;

                try
                {
                    word = collection.Get(s, out foundWord, out foundCase, out foundNumber);
                }
                catch (CyrWordNotFoundException)
                {
                    errors.Add(string.Format("Слово \"<strong>{0}</strong>\" не найдено в коллекции. Попбробуйте другое слово.", s));
                    continue;
                }

                CyrNounDeclineResult result = new CyrNounDeclineResult()
                {
                    Name = word.Name,
                    OriginalWord = s,
                    FoundWord = foundWord,
                    FoundCase = foundCase,
                    FoundNumber = foundNumber,
                    Singular = word.Decline(),
                    Plural = word.DeclinePlural(),
                    Gender = word.Gender,
                    WordType = word.WordType,
                    Animate = word.Animate
                };

                results.Add(result);
            }

            ViewBag.Text = w;
            ViewBag.Errors = errors;
            ViewBag.Results = results;
            ViewBag.Cases = CyrDeclineCase.GetEnumerable().ToArray();

            return View();
        }

        [GzipFilter]
        public ActionResult Adjective(string w)
        {
            ViewBag.Page = "Decline.Adjective";

            if (string.IsNullOrEmpty(w))
            {
                return View();
            }

            List<string> errors = new List<string>();
            List<CyrAdjectiveDeclineResult> results = new List<CyrAdjectiveDeclineResult>();
            CyrAdjectiveCollection collection = this.AdjectiveCollection;

            foreach (string s in w.Split(' ').Where(val => !string.IsNullOrEmpty(val)))
            {
                CyrAdjective word;
                string foundWord;
                GendersEnum foundGender;
                CasesEnum foundCase;
                NumbersEnum foundNumber;
                AnimatesEnum foundAnimate;

                try
                {
                    word = collection.Get(s, out foundWord, out foundGender, out foundCase, out foundNumber, out foundAnimate);
                }
                catch (CyrWordNotFoundException)
                {
                    errors.Add(string.Format("Слово \"<strong>{0}</strong>\" не найдено в коллекции. Попбробуйте другое слово.", s));
                    continue;
                }

                CyrAdjectiveDeclineResult result = new CyrAdjectiveDeclineResult()
                {
                    Name = word.Name,
                    OriginalWord = s,
                    FoundWord = foundWord,
                    FoundGender = foundGender,
                    FoundCase = foundCase,
                    FoundNumber = foundNumber,
                    FoundAnimate = foundAnimate,
                    Singular = word.Decline(foundGender == 0 ? GendersEnum.Masculine : foundGender, foundAnimate),
                    Plural = word.DeclinePlural(foundAnimate)
                };

                results.Add(result);
            }

            ViewBag.Text = w;
            ViewBag.Errors = errors;
            ViewBag.Results = results;
            ViewBag.Cases = CyrDeclineCase.GetEnumerable().ToArray();

            return View();
        }

        [GzipFilter]
        public ActionResult Phrase(string w)
        {
            ViewBag.Page = "Decline.Phrase";

            if (string.IsNullOrEmpty(w))
            {
                return View();
            }

            List<string> errors = new List<string>();
            CyrPhrase phrase = new CyrPhrase(this.NounCollection, this.AdjectiveCollection);
            CyrResult singular = null;
            CyrResult plural = null;

            try
            {
                singular = phrase.Decline(w, GetConditionsEnum.Similar);
                plural = phrase.DeclinePlural(w, GetConditionsEnum.Similar);
            }
            catch (CyrWordNotFoundException ex)
            {
                errors.Add(string.Format("Слово \"<strong>{0}</strong>\" не найдено в коллекции. Попбробуйте убрать слово из фразы.", ex.Word));
                return View();
            }

            ViewBag.Text = w;
            ViewBag.Errors = errors;
            ViewBag.Singular = singular;
            ViewBag.Plural = plural;
            ViewBag.Cases = CyrDeclineCase.GetEnumerable().ToArray();

            return View();
        }

        [GzipFilter]
        public ActionResult Number(string w, string a, string i)
        {
            ViewBag.Page = "Decline.Number";

            if (w.IsNullOrEmpty() || a.IsNullOrEmpty())
            {
                ViewBag.Action = "Item";
                return View();
            }

            List<string> errors = new List<string>();
            CyrResult result = null;
            decimal v;

            if (decimal.TryParse(w, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out v))
            {
                CyrNumber cyr = new CyrNumber();
                CyrNumber.Item item;

                switch (a)
                {
                    case "Item":
                        CyrNoun noun;

                        try
                        {
                            noun = this.NounCollection.Get(i, out string fw, out CasesEnum c, out NumbersEnum n);
                        }
                        catch (CyrWordNotFoundException ex)
                        {
                            errors.Add(string.Format("Слово \"<strong>{0}</strong>\" не найдено в коллекции. Попбробуйте другое слово.", ex.Word));
                            break;
                        }

                        item = new CyrNumber.Item(noun);
                        result = cyr.Decline(v, item);

                        if ((long)v == v)
                        {
                            string[] strings = item.GetName(CasesEnum.Nominative, (long)v);
                            ViewBag.ItemText = cyr.Case((long)v, strings[0], strings[1], strings[2]);
                        }
                        else
                        {
                            string[] strings = item.GetName(CasesEnum.Nominative, 2);
                            ViewBag.ItemText = strings[1];
                        }

                        break;
                    case "Masculine":
                        result = cyr.Decline(v, Model.GendersEnum.Masculine, Model.AnimatesEnum.Inanimated);
                        break;
                    case "Feminine":
                        result = cyr.Decline(v, Model.GendersEnum.Feminine, Model.AnimatesEnum.Inanimated);
                        break;
                    case "Rub":
                        result = cyr.Decline(v, new CyrNumber.RurCurrency());
                        ViewBag.CurrencyClass = "fa-ruble";
                        ViewBag.CurrencyText = "руб.";
                        break;
                    case "Usd":
                        result = cyr.Decline(v, new CyrNumber.UsdCurrency());
                        ViewBag.CurrencyClass = "fa-usd";
                        ViewBag.CurrencyText = "$";
                        break;
                    case "Eur":
                        result = cyr.Decline(v, new CyrNumber.EurCurrency());
                        ViewBag.CurrencyClass = "fa-eur";
                        ViewBag.CurrencyText = "€";
                        break;
                    case "Yuan":
                        result = cyr.Decline(v, new CyrNumber.YuanCurrency());
                        ViewBag.CurrencyClass = "fa-yen";
                        ViewBag.CurrencyText = "¥";
                        break;
                    default:
                        errors.Add(string.Format("{0} не является правильной операцией!", a));
                        break;
                }
            }
            else
            {
                errors.Add(string.Format("{0} не является правильным числом!", w));
            }

            ViewBag.Item = i;
            ViewBag.Text = w;
            ViewBag.Action = a;
            ViewBag.Result = result;
            ViewBag.Errors = errors;
            ViewBag.Cases = CyrDeclineCase.GetEnumerable().ToArray();

            return View();
        }

        [GzipFilter]
        public ActionResult List()
        {
            ViewBag.Page = "Decline.List";

            return View();
        }

        [GzipFilter]
        [HttpPost]
        public ActionResult List(string w)
        {
            ViewBag.Page = "Decline.List";

            if (w.IsNullOrEmpty())
            {
                return View();
            }

            string[] items = w.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (!items.Any())
            {
                return View();
            }

            List<CyrResult> singulars = new List<CyrResult>();
            List<CyrResult> plurals = new List<CyrResult>();
            List<string> errors = new List<string>();

            if (items.Length > MaxPhrasesToDecline)
            {
                errors.Add($"За один раз можно склонять максимум {MaxPhrasesToDecline} фраз!");
            }

            for (int i = 0; i < items.Length && i < MaxPhrasesToDecline; i++)
            {
                string item = items[i];
                CyrPhrase phrase = new CyrPhrase(this.NounCollection, this.AdjectiveCollection);
                CyrResult singular = null;
                CyrResult plural = null;

                try
                {
                    singular = phrase.Decline(item, GetConditionsEnum.Similar);
                    plural = phrase.DeclinePlural(item, GetConditionsEnum.Similar);
                }
                catch (CyrWordNotFoundException ex)
                {
                    errors.Add(string.Format("Слово \"<strong>{0}</strong>\" не найдено в коллекции. Попбробуйте убрать слово из фразы.", ex.Word));
                    continue;
                }

                singulars.Add(singular);
                plurals.Add(plural);
            }

            ViewBag.Text = w;
            ViewBag.Errors = errors;
            ViewBag.Singulars = singulars;
            ViewBag.Plurals = plurals;
            ViewBag.Cases = CyrDeclineCase.GetEnumerable().ToArray();

            return View();
        }

        [GzipFilter]
        public ActionResult File()
        {
            ViewBag.Page = "Decline.File";

            return View();
        }

        [HttpPost]
        public ActionResult File(bool Upload, bool Plural = false)
        {
            ViewBag.Page = "Decline.File";
            ViewBag.Plural = Plural;

            HttpPostedFileBase file = Request.Files["w"];

            if (file == null || file.ContentLength <= 0)
            {
                ViewBag.Errors = new string[] { "Необходимо выбрать файл для склонения!" };
                return View();
            }

            ViewBag.OriginalFileName = file.FileName;

            string extension = Path.GetExtension(file.FileName);
            List<string> errors = new List<string>();
            List<CyrResult> singulars = new List<CyrResult>();
            List<CyrResult> plurals = new List<CyrResult>();
            DirectoryInfo di = new DirectoryInfo(TempFolder);

            if (!di.Exists)
            {
                di.Create();
            }

            switch (extension)
            {
                case ".xlsx":
                    this.DeclineExcelFile(file, Plural);
                    break;
                case ".txt":
                    this.DeclineTextFile(file, Plural);
                    break;
                case ".xls":
                    errors.Add("Xls файлы не поддерживаются. Сохраните файл в формате xlsx и попробуйте заново.");
                    break;
                default:
                    errors.Add($"{extension} файлы не поддерживаются.");
                    break;
            }

            ViewBag.Errors = errors;

            return View();
        }

        protected void DeclineExcelFile(HttpPostedFileBase File, bool Plural)
        {
            ExcelPackage package;

            try
            {
                package = new ExcelPackage(File.InputStream);
            }
            catch
            {
                ViewBag.Errors = new string[] { $"Не удается открыть {File.FileName} файл, возможно файл поврежден или не является правильным документом Microsoft Excel." };
                return;
            }

            string name = Guid.NewGuid() + ".xlsx";
            string path = Path.Combine(TempFolder, name);
            CyrDeclineCase[] cases = CyrDeclineCase.GetEnumerable().ToArray();
            CyrPhrase phrase = new CyrPhrase(NounCollection, AdjectiveCollection);

            ExcelWorksheet input = package.Workbook.Worksheets[1];
            ExcelWorksheet output = package.Workbook.Worksheets.Add("Результат");
            int rowIndex = 1;
            string line = input.Cells[rowIndex, 1].Value?.ToString();
            int totalPhrases = 0;
            int errorPhrases = 0;

            foreach (CyrDeclineCase dc in cases)
            {
                output.Cells[rowIndex, dc.Index].Value = $"{dc.NameRu}, {dc.Description}";
            }

            while (line.IsNotNullOrEmpty())
            {
                rowIndex++;
                totalPhrases++;

                try
                {
                    CyrResult result;

                    if (Plural)
                    {
                        result = phrase.DeclinePlural(line, GetConditionsEnum.Similar);
                    }
                    else
                    {
                        result = phrase.Decline(line, GetConditionsEnum.Similar);
                    }

                    foreach (CyrDeclineCase dc in cases)
                    {
                        output.Cells[rowIndex, dc.Index].Value = result[dc.Index];
                    }
                }
                catch (CyrWordNotFoundException ex)
                {
                    output.Cells[rowIndex, 1].Value = string.Format("Слово \"{0}\" не найдено в коллекции. Попбробуйте убрать слово из фразы.", ex.Word);
                    errorPhrases++;
                }

                line = input.Cells[rowIndex, 1].Value?.ToString();
            }

            output.Cells.AutoFitColumns();
            package.SaveAs(new FileInfo(path));
            package.Dispose();
            ViewBag.DownloadFileName = name;
            ViewBag.TotalPhrases = totalPhrases;
            ViewBag.ErrorPhrases = errorPhrases;
        }

        protected void DeclineTextFile(HttpPostedFileBase File, bool Plural)
        {
            string name = Guid.NewGuid() + ".txt";
            string path = Path.Combine(TempFolder, name);
            CyrDeclineCase[] cases = CyrDeclineCase.GetEnumerable().ToArray();
            CyrPhrase phrase = new CyrPhrase(NounCollection, AdjectiveCollection);

            TextReader reader = new StreamReader(File.InputStream, true);
            TextWriter writer = new StreamWriter(path, false, System.Text.Encoding.UTF8);
            string line = reader.ReadLine();
            int totalPhrases = 0;
            int errorPhrases = 0;

            for (int i = 0; i < cases.Length; i++)
            {
                if (i > 0)
                {
                    writer.Write(" | ");
                }

                writer.Write(cases[i].NameRu);
                writer.Write(" ");
                writer.Write(cases[i].Description);
            }

            while (line.IsNotNullOrEmpty())
            {
                totalPhrases++;

                try
                {
                    CyrResult result;

                    if (Plural)
                    {
                        result = phrase.DeclinePlural(line, GetConditionsEnum.Similar);
                    }
                    else
                    {
                        result = phrase.Decline(line, GetConditionsEnum.Similar);
                    }

                    writer.Write(Environment.NewLine);

                    for (int i = 0; i < cases.Length; i++)
                    {
                        if (i > 0)
                        {
                            writer.Write(" | ");
                        }

                        writer.Write(result[cases[i].Index]);
                    }
                }
                catch (CyrWordNotFoundException ex)
                {
                    writer.Write(Environment.NewLine);
                    writer.Write(string.Format("Слово \"{0}\" не найдено в коллекции. Попбробуйте убрать слово из фразы.", ex.Word));
                    errorPhrases++;
                }

                line = reader.ReadLine();
            }

            reader.Dispose();
            writer.Close();
            writer.Dispose();
            ViewBag.DownloadFileName = name;
            ViewBag.TotalPhrases = totalPhrases;
            ViewBag.ErrorPhrases = errorPhrases;
        }

        protected CyrNounCollection NounCollection
        {
            get
            {
                CyrNounCollection collection = System.Runtime.Caching.MemoryCache.Default.Get("CyrNounCollection") as CyrNounCollection;

                if (collection != null)
                {
                    return collection;
                }

                collection = new CyrNounCollection();

                System.Runtime.Caching.MemoryCache.Default.Set("CyrNounCollection", collection, DateTime.Now.AddMonths(1));

                return collection;
            }
        }

        protected CyrAdjectiveCollection AdjectiveCollection
        {
            get
            {
                CyrAdjectiveCollection collection = System.Runtime.Caching.MemoryCache.Default.Get("CyrAdjectiveCollection") as CyrAdjectiveCollection;

                if (collection != null)
                {
                    return collection;
                }

                collection = new CyrAdjectiveCollection();

                System.Runtime.Caching.MemoryCache.Default.Set("CyrAdjectiveCollection", collection, DateTime.Now.AddMonths(1));

                return collection;
            }
        }
    }
}