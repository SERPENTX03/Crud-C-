using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyMongoApi.Models;

namespace MyMongoApi.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _postsCollection;

        public PostService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _postsCollection = database.GetCollection<Post>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<Post>> GetAsync() =>
            await _postsCollection.Find(_ => true).ToListAsync();

        public async Task<Post?> GetByIdAsync(string id) =>
            await _postsCollection.Find(post => post.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Post post) =>
            await _postsCollection.InsertOneAsync(post);

        public async Task UpdateAsync(string id, Post post) =>
            await _postsCollection.ReplaceOneAsync(p => p.Id == id, post);

        public async Task DeleteAsync(string id) =>
            await _postsCollection.DeleteOneAsync(p => p.Id == id);
    }
}
