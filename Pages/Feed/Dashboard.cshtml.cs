using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NiceLIA.Models.Domain;

namespace NiceLIA.Pages.Feed
{
    public class DashboardModel : PageModel
    {

        private FirestoreDb _db;
        public Post _post;
        public List<Dictionary<string, object>> Posts { get; set; }


        public DashboardModel()
        {
            string filePath = "./feed-cb599-firebase-adminsdk-qn48j-5da6bc96f2.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            _db = FirestoreDb.Create("feed-cb599");
        }

        public async Task OnGetAsync()
        {
            // Retrieve all posts from Firestore
            var postsRef = _db.Collection("posts");
            var query = await postsRef.OrderBy("createdAt").GetSnapshotAsync();
            Posts = query.Documents.Select(d => d.ToDictionary()).ToList();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            // Add a new post to Firestore
            string content = Request.Form["postContent"];

            var post = new Dictionary<string, object>
        {
            { "content", content },
            { "createdAt", DateTime.UtcNow }
        };

            await _db.Collection("posts").AddAsync(post);

            return RedirectToPage();
        }
    }
}
