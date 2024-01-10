using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Expense_Tracker.Controllers
{
    public class DashBoard : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashBoard(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {

            //Last 7 Days
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transation> SelectedTransactions = await _context.Transation
                .Include(x=>x.Division)
                //.Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();

            //Total Income
            int TotalIncome = SelectedTransactions
                .Where(i => i.Division.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

            //Total Expense
            int TotalExpense = SelectedTransactions
                .Where(i => i.Division.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            //Balance
            int Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = Balance.ToString("C0"); //String.Format(culture, "{0:C0}", Balance);

            //Doughnut Chart - Expense By Division
            ViewBag.DoughnutChartData = SelectedTransactions
                .Where(i => i.Division.Type == "Expense")
                .GroupBy(j => j.Division.DivisionId)
                .Select(k => new
                {
                    DivisionName = /*k.First().Division.Icon + " " +*/ k.First().Division.Name,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                })
                .OrderByDescending(l => l.amount)
                .ToList();

         
            //Recent Transactions
            ViewBag.RecentTransactions = await _context.Transation
                .Include(i => i.Division)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();


            return View();
        }
    }

    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;

    }

}
