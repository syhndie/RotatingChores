﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RotatingChores.Data;
using RotatingChores.Models;
using Microsoft.AspNetCore.Identity;


namespace RotatingChores.Pages.Chores
{
    public class CreateModel : BasePageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ApplicationDbContext _context;

        public Chore Chore { get; set; }

        public CreateModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Chore = new Chore
            {
                UserID = _userManager.GetUserId(User)
            };

            var modelDidUpdate = await TryUpdateModelAsync(
               this.Chore,
               "Chore",
               c => c.Name,
               c => c.DateLastCompleted,
               c => c.FrequencyValue,
               c => c.FrequencyUnits,
               c => c.Notes,
               c => c.IsHighPriority);
            
            if (modelDidUpdate)
            {
                _context.Chores.Add(this.Chore);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            DangerMessage = "New chore did not save correctly.";
            return RedirectToPage();
        }
    }
}